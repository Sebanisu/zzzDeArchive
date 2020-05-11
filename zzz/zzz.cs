using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZzzArchive
{
    public class Zzz
    {
        #region Fields

        private const int MaxPath = 260;

        private readonly string _other;
        private string _out;

        #endregion Fields

        #region Constructors

        public Zzz()
        {
            OriginalMain = "main.zzz";
            _other = "other.zzz";
            ID1 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "IN", "main");
            ID2 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "IN", "other");
            Directory.CreateDirectory(ID1);
            Directory.CreateDirectory(ID2);
            OutputDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "OUT");
            Directory.CreateDirectory(OutputDirectory);
        }

        #endregion Constructors

        #region Properties

        public string ID1 { get; }

        public string ID2 { get; }

        public List<string> In { get; set; } = new List<string>(1);

        public string Main { get; set; }

        public string OriginalMain { get; }

        public string Other { get; set; }

        public string Out
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_out))
                {
                    if (IsMainOrOther(OriginalMain, Path))
                    {
                        Out = System.IO.Path.Combine(OutputDirectory, OriginalMain);
                    }
                    else if (IsMainOrOther(_other, Path))
                    {
                        Out = System.IO.Path.Combine(OutputDirectory, OriginalMain);
                    }
                    else
                        return _out = System.IO.Path.Combine(OutputDirectory, "out.zzz");
                }
                return _out;
            }
            set => _out = value;
        }

        public string OutputDirectory { get; }

        public string Path { get; set; }

        public bool SkipWarning { get; set; } = false;

        #endregion Properties

        #region Methods

        public Header ReadHeader()
        {
            using (var fs = GetFsRead(In.First()))
            {
                using (var br = new BinaryReader(fs))
                {
                    var head = TestLength(Header.Read(br));
                    Logger.WriteLine(head.ToString());
                    return head;
                }
            }
        }

            public string Extract()
        {
            Logger.WriteLine($"Extracting {In.First()} to {Path}");
            using (var fs = GetFsRead(In.First()))
            {
                using (var br = new BinaryReader(fs))
                {
                    var head = TestLength(Header.Read(br));
                    Logger.WriteLine(head.ToString());

                    //Directory.CreateDirectory(_path);
                    foreach (var d in head.Data)
                    {
                        Logger.WriteLine($"Writing {d}");
                        var path = System.IO.Path.Combine(Path, d.Filename);
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
                        using (var fso = File.Create(path))
                        {
                            using (var bw = new BinaryWriter(fso))
                            {
                                fs.Seek(d.Offset, SeekOrigin.Begin);
                                ReadUInt(bw, br, d.Size);
                            }
                        }
                    }
                    Logger.WriteLine("Verifying output");
                    TestSize(head, br.BaseStream);

                    foreach (var item in head.Data)
                    {
                        if (item.Offset > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine("Offset too large!\n" +
                                $"offset: {item.Offset}"));
                        }
                        if (item.Offset + item.Size > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine("Offset too large!\n" +
                                $"offset: {item.Offset}\n" +
                                $"size: {item.Size}"));
                        }
                        fs.Seek(item.Offset, SeekOrigin.Begin);

                        var outHash = HashTester.GetHash(br.BaseStream, item.Size);
                        var testPath = System.IO.Path.Combine(Path, item.Filename);
                        var inHash = HashTester.GetHashClose(GetFsRead(testPath));
                        if (inHash == null)
                        {
                            Logger.WriteLineThrow($"failed to verify ({testPath}) sha1 value is null");
                        }
                        else if (!inHash.SequenceEqual(outHash))
                        {
                            Logger.WriteLineThrow($"failed to verify ({testPath}) sha1 mismatch \n" +
                                $"sha1:   {BitConverter.ToString(inHash).Replace("-", "")} != {BitConverter.ToString(outHash).Replace("-", "")}\n" +
                                $"offset: {item.Offset}\n" +
                                $"size:   {item.Size}");
                        }
                        else
                        {
                            Logger.WriteLine($"Verified ({testPath}) sha1({BitConverter.ToString(outHash).Replace("-", "")})");
                        }
                    }
                }
            }
            Logger.WriteLine($"Saved to: {Path}");
            return Path;
        }

        public string FolderMerge()
        {
            var d1 = Directory.EnumerateDirectories(ID1).ToList();
            var d2 = Directory.EnumerateDirectories(ID2).ToList();
            Write(d1);
            Write(d2);
            var f1 = Directory.EnumerateFiles(ID1).ToList();
            var f2 = Directory.EnumerateFiles(ID2).ToList();
            Merge(f1, OriginalMain, Main);
            Merge(f2, _other, Other);
            return OutputDirectory;
        }

        public string Merge()
        {
            Logger.WriteLine("Merging files:");
            foreach (var i in In)
                Logger.WriteLine($"{i}");
            Logger.WriteLine($"Into: {Path}");
            Logger.WriteLine($"Output: {Out}");
            TestLength(In);
            if (Out != null) { }

            TwoBr br;
            Logger.WriteLine($"Opening {Path}");
            using (br.Out = new BinaryReader(GetFsRead(Path)))
            {
                if (Path.Equals(Out, StringComparison.OrdinalIgnoreCase))
                {
                    Out = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Out) ?? throw new InvalidOperationException(), $"{System.IO.Path.GetFileNameWithoutExtension(Out)}0{System.IO.Path.GetExtension(Out)}");
                }

                TwoHeader head;
                head.Out = Header.Read(br.Out);
                TestSize(head.Out, br.Out.BaseStream);
                br.In = new BinaryReader[In.Count];
                head.In = new Header[In.Count];
                for (var i = 0; i < In.Count; i++)
                {
                    Logger.WriteLine($"Opening {In[i]}");
                    if (In[i].Equals(Path, StringComparison.OrdinalIgnoreCase) || In[i].Equals(Out, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(Logger.WriteLine($"{In[i]}\n" +
                            "cannot match\n" +
                            $"{Path}\n" +
                            "or\n" +
                            $"{Out}\n" +
                            "This may caused undesired results. Like writing to a file you are" +
                            "reading from or undoing changes you are trying to make"));
                    }

                    if (System.IO.Path.GetFileName(In[i]).Equals("main.zzz", StringComparison.OrdinalIgnoreCase) ||
                        System.IO.Path.GetFileName(In[i]).Equals("other.zzz", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(Logger.WriteLine($"{In[i]}\n" +
                                                                     "Should not match main.zzz or other.zzz\n" +
                                                                     "As this could be a mistake and you would be replacing the mods with your source files"));
                    }
                    br.In[i] = new BinaryReader(GetFsRead(In[i]));
                    head.In[i] = Header.Read(br.In[i]);
                    TestSize(head.In[i], br.In[i].BaseStream);
                }
                Merged merged;

                merged.Head = Header.Merge(ref head.Out, ref head.In, SkipWarning);

                using (var fs = GetFsWrite(ref _out))
                using (merged.Bw = new BinaryWriter(fs))
                using (merged.Br = new BinaryReader(fs))
                {
                    merged.Head.Write(merged.Bw);
                    Logger.WriteLine($"Writing raw file data from {Path}");
                    Write(br.Out, head.Out.Data);

                    for (var i = 0; i < In.Count; i++)
                    {
                        Logger.WriteLine($"Writing raw file data from {In[i]}");
                        Write(br.In[i], head.In[i].Data);
                    }

                    void Write(BinaryReader outBr, IEnumerable<FileData> data)
                    {
                        foreach (var i in data)
                        {
                            outBr.BaseStream.Seek(i.Offset, SeekOrigin.Begin);
                            Logger.WriteLine($"Writing {i.Filename} {i.Size} bytes");
                            ReadUInt(merged.Bw, outBr, i.Size);
                        }
                    }
                    Logger.WriteLine($"Saved to: {Out}");
                    Logger.WriteLine("Verifying output");
                    TestSize(merged.Head, merged.Bw.BaseStream);

                    foreach (var item in merged.Head.Data)
                    {
                        if (item.Offset > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine("Offset too large!\n" +
                                $"offset: {item.Offset}"));
                        }
                        if (item.Offset + item.Size > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine("Offset too large!\n" +
                                $"offset: {item.Offset}\n" +
                                $"size: {item.Size}"));
                        }
                        fs.Seek(item.Offset, SeekOrigin.Begin);
                        var outHash = HashTester.GetHash(merged.Br.BaseStream, item.Size);
                        byte[] inHash = null;
                        var tempHeader = new FileData();
                        var i = 0;
                        var src = "";
                        for (; i < In.Count; i++)
                        {
                            tempHeader = head.In[i].Data.FirstOrDefault(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            if (tempHeader.Equals(new FileData())) continue;
                            src = In[i];
                            br.In[i].BaseStream.Seek(tempHeader.Offset, SeekOrigin.Begin);
                            inHash = HashTester.GetHash(br.In[i].BaseStream, tempHeader.Size);
                            break;
                        }
                        if (tempHeader.Equals(new FileData()))
                        {
                            src = Path;
                            tempHeader = head.Out.Data.First(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            br.Out.BaseStream.Seek(tempHeader.Offset, SeekOrigin.Begin);
                            inHash = HashTester.GetHash(br.Out.BaseStream, tempHeader.Size);
                        }
                        if (inHash == null)
                        {
                            Logger.WriteLineThrow($"failed to verify ({item.Filename}) sha1 value is null");
                        }
                        else if (!inHash.SequenceEqual(outHash))
                        {
                            Logger.WriteLineThrow($"failed to verify ({item.Filename}) sha1 mismatch \n" +
                                $"sha1:   {BitConverter.ToString(inHash).Replace("-", "")} != {BitConverter.ToString(outHash).Replace("-", "")}\n" +
                                $"merged offset: {item.Offset}\n" +
                                $"merged size:   {item.Size}\n" +
                                $"source:        {src}\n" +
                                $"source offset: {tempHeader.Offset}\n" +
                                $"source size:   {tempHeader.Size}");
                        }
                        else
                        {
                            Logger.WriteLine($"Verified ({item.Filename}) sha1({BitConverter.ToString(outHash).Replace("-", "")})");
                        }
                    }
                }
            }
            for (var i = 0; i < In.Count; i++)
            {
                br.In[i].Close();
            }
            return System.IO.Path.GetDirectoryName(Out);
        }

        public string Write()
        {
            Logger.WriteLine($"Writing {Path} to {Out}");
            var head = Header.Read(Path, out var files, Path);
            files = TestLength(files);
            Logger.WriteLine(head.ToString());
            if (Out != null)
                using (var fs = GetFsWrite(ref _out))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        head.Write(bw);
                        Logger.WriteLine($"Writing raw file data from {Path}");
                        foreach (var file in files)
                        {
                            var fi = new FileInfo(file);
                            Logger.WriteLine($"Writing {file} {fi.Length} bytes");
                            using (var br = new BinaryReader(GetFsRead(file)))
                            {
                                byte[] buffer;
                                do
                                {
                                    buffer = br.ReadBytes((int)br.BaseStream.Length);
                                    bw.Write(buffer);
                                }
                                while (buffer.Length > 0);
                            }
                        }

                        Logger.WriteLine($"Saved to: {Out}");
                        Logger.WriteLine("Verifying output");
                        TestSize(head, bw.BaseStream);
                        using (var br = new BinaryReader(fs))
                        {
                            foreach (var item in head.Data)
                            {
                                if (item.Offset > fs.Length)
                                {
                                    throw new ArgumentOutOfRangeException("Offset too large!\n" +
                                        $"offset: {item.Offset}");
                                }
                                if (item.Offset + item.Size > fs.Length)
                                {
                                    throw new ArgumentOutOfRangeException("Offset too large!\n" +
                                        $"offset: {item.Offset}\n" +
                                        $"size: {item.Size}");
                                }
                                fs.Seek(item.Offset, SeekOrigin.Begin);

                                var osha = HashTester.GetHash(br.BaseStream, item.Size);
                                var testPath = System.IO.Path.Combine(Path, item.Filename);
                                var inputHash = HashTester.GetHashClose(GetFsRead(testPath));
                                if (inputHash == null)
                                {
                                    Logger.WriteLineThrow($"failed to verify ({testPath}) sha1 value is null");
                                }
                                else if (!inputHash.SequenceEqual(osha))
                                {
                                    Logger.WriteLineThrow($"failed to verify ({testPath}) sha1 mismatch \n" +
                                        $"sha1:   {BitConverter.ToString(inputHash).Replace("-", "")} != {BitConverter.ToString(osha).Replace("-", "")}\n" +
                                        $"offset: {item.Offset}\n" +
                                        $"size:   {item.Size}");
                                }
                                else
                                {
                                    Logger.WriteLine($"Verified ({testPath}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                                }
                            }
                        }
                    }
                }
            return System.IO.Path.GetDirectoryName(Out);
        }

        private static FileStream GetFsWrite(ref string path, FileAccess fa = FileAccess.ReadWrite, FileShare fs = FileShare.Read)
        {
            var nonRefPath = path;
            FileStream stream;
            var i = 0;
            do
            {
                try
                {
                    stream = new FileStream(path, FileMode.Create, fa, fs);
                }
                catch (IOException e)
                {
                    stream = null;
                    Logger.Write($"{e.Message} :: Error writing to: {path}\n Going to increment file and try again...");
                    path = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(nonRefPath) ?? throw new InvalidOperationException(),
                        $"{System.IO.Path.GetFileNameWithoutExtension(nonRefPath)}{i++}.zzz");
                }
            }
            while (stream == null);

            return stream;
        }

        private static bool IsMainOrOther(string main, string x) => x != null && System.IO.Path.GetFileName(x).Equals(main, StringComparison.OrdinalIgnoreCase);

        private static void ReadUInt(BinaryWriter bw, BinaryReader br, uint size)
        {
            while (size > 0)
            {
                var s = (size > int.MaxValue) ? int.MaxValue : (int)size;
                bw.Write(br.ReadBytes(s));
                size -= (uint)s;
            }
        }

        private static void TestSize(Header head, Stream stream)
        {
            var msg = $"Expected file size ({head.ExpectedFileSize}) == resulting file size ({stream.Length})";
            Logger.WriteLine(msg);
            if (head.ExpectedFileSize != stream.Length)
            {
                throw new InvalidDataException(msg);
            }
        }

        private FileStream GetFsRead(string path, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.Read)
        {
            try
            {
                return File.Open(path, FileMode.Open, fa, fs);
            }
            catch (IOException err)
            {
                Logger.WriteLine($"{this} :: {path}\n{err.Message}");
                Logger.WriteLine($"Will attempt to open file with {fa} & {fs} only. Could be issue if reading from a file as someone else is writing to it.\n" +
                    $"Will try again with {FileShare.ReadWrite}");
                return File.Open(path, FileMode.Open, fa, FileShare.ReadWrite);
            }
        }

        /*
                private bool IsMainOrOther(string x) => IsMainOrOther(main, x) || IsMainOrOther(_other, x);
        */

        private void Merge(List<string> f1, string input, string arg = null)
        {
            if (f1.Count <= 1) return;
            if (!string.IsNullOrWhiteSpace(Main))
            {
                Path = arg;
                In = f1;
                Out = System.IO.Path.Combine(OutputDirectory, input);
            }
            else
            {
                var ind = f1.FindIndex(x => IsMainOrOther(input, x));
                if (ind >= 0)
                {
                    Path = f1[ind];
                    f1.RemoveAt(ind);
                    In = f1;
                    Out = System.IO.Path.Combine(OutputDirectory, input);
                }
                else
                {
                    Path = f1.First();
                    f1.Remove(Path);
                    In = f1;
                    Out = System.IO.Path.Combine(OutputDirectory, "part_main");
                }
            }
            Merge();
        }

        private Header TestLength(Header head)
        {
            var maxPathLength = head.MaxPathLength;
            if (Path == null) return head;
            var str = $"Max path length of {System.IO.Path.GetFileName(In.First())}: {maxPathLength}\n" +
                      $"Dest path length: {Path.Length}\n" +
                      $"Total+1 ({Path.Length + maxPathLength + 1}) must be less than {MaxPath}\n" +
                      $"And the path of {System.IO.Path.GetFileName(In.First())}: {In.First().Length}\n" +
                      $"must also be less than {MaxPath}";
            if (In.First().Length >= MaxPath || Path.Length + maxPathLength + 1 >= MaxPath)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }
            

            Logger.WriteLine(str, true);
            return head;
        }

        private string[] TestLength(string[] files)
        {
            var maxPathLength = files.Max(x => x.Length);
            var str = $"Max path length of {System.IO.Path.GetFileName(Path)}: {maxPathLength}\n" +
                      $"Dest path length: {Out.Length}\n" +
                      $"Both must be less than {MaxPath}";
            if (Out.Length >= MaxPath || maxPathLength >= MaxPath)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }

            Logger.WriteLine(str, true);
            return files;
        }

        private void TestLength(IEnumerable<string> files)
        {
            var maxPathLength = files.Max(x => x.Length);
            var str =
                $"Max path length of {Path}: {Path.Length}\n" +
                $"Dest path length: {Out.Length}\n" +
                $"Max path Length of input files: {maxPathLength}\n" +
                $"Both all be less than {MaxPath}";
            if (Out.Length >= MaxPath || Path.Length >= MaxPath || maxPathLength >= MaxPath)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }

            Logger.WriteLine(str, true);
        }

        private void Write(IEnumerable<string> listOfDirectories)
        {
            var path = Path;
            foreach (var directory in listOfDirectories)
            {
                Path = directory;
                Out = $"{directory}.zzz";
                Write();
            }
            Path = path;
        }

        #endregion Methods

        #region Structs

        private struct Merged
        {
            #region Fields

            public BinaryReader Br;
            public BinaryWriter Bw;
            public Header Head;

            #endregion Fields
        }

        private struct TwoBr
        {
            #region Fields

            public BinaryReader[] In;
            public BinaryReader Out;

            #endregion Fields
        }

        private struct TwoHeader
        {
            #region Fields

            public Header[] In;
            public Header Out;

            #endregion Fields
        }

        #endregion Structs
    }
}