using ZzzArchive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ZzzArchive
{

    public class Zzz
    {

        #region Fields

        private const int max_path = 260;

        private readonly string other;
        private List<string> _in = new List<string>(1);
        private string _out;
        private string _path;

        #endregion Fields

        #region Methods

        private static bool IsMainOrOther(string main, string x) => Path.GetFileName(x).Equals(main, StringComparison.OrdinalIgnoreCase);

        private static void ReadUInt(BinaryWriter bw, BinaryReader br, uint size)
        {
            while (size > 0)
            {
                int s = (size > int.MaxValue) ? int.MaxValue : (int)size;
                bw.Write(br.ReadBytes(s));
                size -= (uint)s;
            }
        }

        private static void TestSize(Header head, Stream stream)
        {
            string msg = $"Expected filesize ({head.ExpectedFileSize}) == resulting filesize ({stream.Length})";
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

        private FileStream GetFsWrite(ref string path, FileAccess fa = FileAccess.ReadWrite, FileShare fs = FileShare.Read)
        {
            string path_ = path;
            FileStream fstream;
            int i = 0;
            do
            {
                try
                {
                    fstream = File.Open(path, FileMode.Create, fa, fs);
                }
                catch (IOException e)
                {
                    fstream = null;
                    Logger.Write($"{e.Message} :: Error writing to: {path}\n Going to increment file and try again...");
                    path = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(path_),
                        $"{System.IO.Path.GetFileNameWithoutExtension(path_)}{i++}.zzz");
                }
            }
            while (fstream == null);

            return fstream;
        }
        private bool IsMainOrOther(string x) => IsMainOrOther(main, x) || IsMainOrOther(other, x);

        private void Merge(List<string> f1, string main, string arg = null)
        {
            if (f1.Count() > 1)
            {
                if (!string.IsNullOrWhiteSpace(Main))
                {
                    Path_ = arg;
                    In = f1;
                    Out = Path.Combine(od, main);
                }
                else
                {
                    int ind = f1.FindIndex(x => IsMainOrOther(main, x));
                    if (ind >= 0)
                    {
                        Path_ = f1[ind];
                        f1.RemoveAt(ind);
                        In = f1;
                        Out = Path.Combine(od, main);
                    }
                    else
                    {
                        Path_ = f1.First();
                        f1.Remove(Path_);
                        In = f1;
                        Out = Path.Combine(od, $"part_main");
                    }
                }
                Merge();
            }
        }

        private Header TestLength(Header head)
        {
            int maxPathLength = head.MaxPathLength;
            string str = $"Max path length of {Path.GetFileName(In.First())}: {maxPathLength}\n" +
                    $"Dest path length: {Path_.Length}\n" +
                    $"Total+1 ({Path_.Length + maxPathLength + 1}) must be less than {max_path}\n" +
                    $"And the path of {Path.GetFileName(In.First())}: {In.First().Length}\n" +
                    $"must also be less than {max_path}";
            if (In.First().Length >= max_path || Path_.Length + maxPathLength + 1 >= max_path)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }
            else
            {
                Logger.WriteLine(str, true);
            }
            return head;
        }

        private string[] TestLength(string[] files)
        {
            int maxPathLength = files.Max(x => x.Length);
            string str = $"Max path length of {Path.GetFileName(Path_)}: {maxPathLength}\n" +
                    $"Dest path length: {Out.Length}\n" +
                    $"Both must be less than {max_path}";
            if (Out.Length >= max_path || maxPathLength >= max_path)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }
            else
            {
                Logger.WriteLine(str, true);
            }
            return files;
        }

        private List<string> TestLength(List<string> files)
        {
            int maxPathLength = files.Max(x => x.Length);
            string str =
                $"Max path length of {Path_}: {Path_.Length}\n" +
                $"Dest path length: {Out.Length}\n" +
                $"Max path Length of input files: {maxPathLength}\n" +
                $"Both all be less than {max_path}";
            if (Out.Length >= max_path || Path_.Length >= max_path || maxPathLength >= max_path)
            {
                throw new PathTooLongException(Logger.WriteLine(str));
            }
            else
            {
                Logger.WriteLine(str, true);
            }
            return files;
        }

        private void Write(List<string> d1)
        {
            string path = Path_;
            foreach (string d in d1)
            {
                Path_ = d;
                Out = $"{d}.zzz";
                Write();
            }
            Path_ = path;
        }

        #endregion Methods

        #region Structs

        private struct Merged
        {
            #region Fields

            public BinaryReader br;
            public BinaryWriter bw;
            public Header head;

            #endregion Fields
        }

        private struct TwoBR
        {
            #region Fields

            public BinaryReader[] _in;
            public BinaryReader _out;

            #endregion Fields
        }

        private struct TwoHeader
        {
            #region Fields

            public Header[] _in;
            public Header _out;

            #endregion Fields
        }

        #endregion Structs

        #region Constructors

        public Zzz()
        {
            main = "main.zzz";
            other = "other.zzz";
            id1 = Path.Combine(Directory.GetCurrentDirectory(), "IN", "main");
            id2 = Path.Combine(Directory.GetCurrentDirectory(), "IN", "other");
            Directory.CreateDirectory(id1);
            Directory.CreateDirectory(id2);
            od = Path.Combine(Directory.GetCurrentDirectory(), "OUT");
            Directory.CreateDirectory(od);
        }

        #endregion Constructors

        #region Properties

        public string id1 { get; }
        public string id2 { get; }
        public List<string> In { get => _in; set => _in = value; }
        public string main { get; }
        public string Main { get; set; }
        public string od { get; }
        public string Other { get; set; }
        public string Out
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_out))
                {
                    if (IsMainOrOther(main, Path_))
                    {
                        Out = Path.Combine(od, main);
                    }
                    else if (IsMainOrOther(other, Path_))
                    {
                        Out = Path.Combine(od, main);
                    }
                    else
                        return _out = Path.Combine(od, "out.zzz");
                }
                return _out;
            }
            set => _out = value;
        }

        public string Path_ { get => _path; set => _path = value; }
        public bool SkipWarning { get; set; } = false;

        #endregion Properties

        public string Extract()
        {
            Logger.WriteLine($"Extracting {In.First()} to {Path_}");
            Header head;
            using (FileStream fs = GetFsRead(In.First()))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    head = TestLength(Header.Read(br));
                    Logger.WriteLine(head.ToString());

                    //Directory.CreateDirectory(_path);
                    foreach (FileData d in head.Data)
                    {
                        Logger.WriteLine($"Writing {d}");
                        string path = System.IO.Path.Combine(Path_, d.Filename);
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                        using (FileStream fso = File.Create(path))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fso))
                            {
                                if (d.Offset <= long.MaxValue)
                                {
                                    fs.Seek(d.Offset, SeekOrigin.Begin);
                                    ReadUInt(bw, br, d.Size);
                                }
                                else throw new ArgumentOutOfRangeException(Logger.WriteLine($"d.offset is too large! ({d.Offset})"));
                            }
                        }
                    }
                    Logger.WriteLine($"Verifing output");
                    TestSize(head, br.BaseStream);

                    foreach (FileData item in head.Data)
                    {
                        if (item.Offset > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine($"Offset too large!\n" +
                                "offset: {item.Offset}"));
                        }
                        if (item.Offset + item.Size > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine($"Offset too large!\n" +
                                "offset: {item.Offset}\n" +
                                "size: {item.Size}"));
                        }
                        fs.Seek(item.Offset, SeekOrigin.Begin);

                        byte[] osha = HashTester.GetHash(br.BaseStream, item.Size);
                        byte[] isha = null;
                        string testpath = System.IO.Path.Combine(Path_, item.Filename);
                        isha = HashTester.GetHashClose(GetFsRead(testpath));
                        if (isha == null)
                        {
                            Logger.WriteLineThrow($"failed to verify ({testpath}) sha1 value is null");
                        }
                        else if (!isha.SequenceEqual(osha))
                        {
                            Logger.WriteLineThrow($"failed to verify ({testpath}) sha1 mismatch \n" +
                                $"sha1:   {BitConverter.ToString(isha).Replace("-", "")} != {BitConverter.ToString(osha).Replace("-", "")}\n" +
                                $"offset: {item.Offset}\n" +
                                $"size:   {item.Size}");
                        }
                        else
                        {
                            Logger.WriteLine($"Verified ({testpath}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                        }
                    }
                }
            }
            Logger.WriteLine($"Saved to: {Path_}");
            return Path_;
        }

        public string FolderMerge()
        {
            List<string> d1 = Directory.EnumerateDirectories(id1).ToList();
            List<string> d2 = Directory.EnumerateDirectories(id2).ToList();
            Write(d1);
            Write(d2);
            List<string> f1 = Directory.EnumerateFiles(id1).ToList();
            List<string> f2 = Directory.EnumerateFiles(id2).ToList();
            Merge(f1, main, Main);
            Merge(f2, other, Other);
            return od;
        }
        public string Merge()
        {
            Logger.WriteLine($"Merging files:");
            foreach (string i in In)
                Logger.WriteLine($"{i}");
            Logger.WriteLine($"Into: {Path_}");
            Logger.WriteLine($"Output: {Out}");
            TestLength(In);
            if (Out != null) { };
            TwoBR br;
            TwoHeader head;
            Logger.WriteLine($"Opening {Path_}");
            using (br._out = new BinaryReader(GetFsRead(Path_)))
            {
                if (Path_.Equals(Out, StringComparison.OrdinalIgnoreCase))
                {
                    Out = Path.Combine(Path.GetDirectoryName(Out), $"{Path.GetFileNameWithoutExtension(Out)}0{Path.GetExtension(Out)}");
                }
                head._out = Header.Read(br._out);
                TestSize(head._out, br._out.BaseStream);
                br._in = new BinaryReader[In.Count];
                head._in = new Header[In.Count];
                for (int i = 0; i < In.Count; i++)
                {
                    Logger.WriteLine($"Opening {In[i]}");
                    if (In[i].Equals(Path_, StringComparison.OrdinalIgnoreCase) || In[i].Equals(Out, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(Logger.WriteLine($"{In[i]}\n" +
                            "cannot match\n" +
                            $"{Path_}\n" +
                            "or\n" +
                            $"{Out}\n" +
                            $"This may caused undesired results. Like writing to a file you are reading from or undoing changes you are trying to make"));
                    }
                    else if (Path.GetFileName(In[i]).Equals("main.zzz", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetFileName(In[i]).Equals("other.zzz", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(Logger.WriteLine($"{In[i]}\n" +
                            "Should not match main.zzz or other.zzz\n" +
                            "As this could be a mistake and you would be replacing the mods with your source files"));
                    }
                    br._in[i] = new BinaryReader(GetFsRead(In[i]));
                    head._in[i] = Header.Read(br._in[i]);
                    TestSize(head._in[i], br._in[i].BaseStream);
                }
                Merged merged;

                merged.head = Header.Merge(ref head._out, ref head._in, SkipWarning);

                using (FileStream fs = GetFsWrite(ref _out))
                using (merged.bw = new BinaryWriter(fs))
                using (merged.br = new BinaryReader(fs))
                {
                    merged.head.Write(merged.bw);
                    Logger.WriteLine($"Writing raw file data from {Path_}");
                    Write(br._out, head._out.Data);

                    for (int i = 0; i < In.Count; i++)
                    {
                        Logger.WriteLine($"Writing raw file data from {In[i]}");
                        Write(br._in[i], head._in[i].Data);
                    }

                    void Write(BinaryReader _br, FileData[] data)
                    {
                        foreach (FileData i in data)
                        {
                            _br.BaseStream.Seek(i.Offset, SeekOrigin.Begin);
                            Logger.WriteLine($"Writing {i.Filename} {i.Size} bytes");
                            ReadUInt(merged.bw, _br, i.Size);
                        }
                    }
                    Logger.WriteLine($"Saved to: {Out}");
                    Logger.WriteLine($"Verifing output");
                    TestSize(merged.head, merged.bw.BaseStream);

                    foreach (FileData item in merged.head.Data)
                    {
                        if (item.Offset > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine($"Offset too large!\n" +
                                "offset: {item.Offset}"));
                        }
                        if (item.Offset + item.Size > fs.Length)
                        {
                            throw new ArgumentOutOfRangeException(Logger.WriteLine($"Offset too large!\n" +
                                "offset: {item.Offset}\n" +
                                "size: {item.Size}"));
                        }
                        fs.Seek(item.Offset, SeekOrigin.Begin);
                        byte[] osha = null;
                        osha = HashTester.GetHash(merged.br.BaseStream, item.Size);
                        byte[] isha = null;
                        FileData tmphead = new FileData();
                        int i = 0;
                        string src = "";
                        for (; i < In.Count; i++)
                        {
                            tmphead = head._in[i].Data.FirstOrDefault(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            if (!tmphead.Equals(new FileData()))
                            {
                                src = In[i];
                                br._in[i].BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                                isha = HashTester.GetHash(br._in[i].BaseStream, tmphead.Size);
                                break;
                            }
                        }
                        if (tmphead.Equals(new FileData()))
                        {
                            src = Path_;
                            tmphead = head._out.Data.First(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            br._out.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = HashTester.GetHash(br._out.BaseStream, tmphead.Size);
                        }
                        if (isha == null)
                        {
                            Logger.WriteLineThrow($"failed to verify ({item.Filename}) sha1 value is null");
                        }
                        else if (!isha.SequenceEqual(osha))
                        {
                            Logger.WriteLineThrow($"failed to verify ({item.Filename}) sha1 mismatch \n" +
                                $"sha1:   {BitConverter.ToString(isha).Replace("-", "")} != {BitConverter.ToString(osha).Replace("-", "")}\n" +
                                $"merged offset: {item.Offset}\n" +
                                $"merged size:   {item.Size}\n" +
                                $"source:        {src}\n" +
                                $"source offset: {tmphead.Offset}\n" +
                                $"source size:   {tmphead.Size}");
                        }
                        else
                        {
                            Logger.WriteLine($"Verified ({item.Filename}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                        }
                    }
                }
            }
            for (int i = 0; i < In.Count; i++)
            {
                br._in[i].Close();
            }
            return System.IO.Path.GetDirectoryName(Out);
        }

        public string Write()
        {
            Logger.WriteLine($"Writing {Path_} to {Out}");
            Header head = Header.Read(Path_, out string[] files, Path_);
            files = TestLength(files);
            Logger.WriteLine(head.ToString());
            if (Out != null)
                using (FileStream fs = GetFsWrite(ref _out))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        head.Write(bw);
                        Logger.WriteLine($"Writing raw file data from {Path_}");
                        foreach (string file in files)
                        {
                            FileInfo fi = new FileInfo(file);
                            Logger.WriteLine($"Writing {file} {fi.Length} bytes");
                            using (BinaryReader br = new BinaryReader(GetFsRead(file)))
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
                        Logger.WriteLine($"Verifing output");
                        TestSize(head, bw.BaseStream);
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            foreach (FileData item in head.Data)
                            {
                                if (item.Offset > fs.Length)
                                {
                                    throw new ArgumentOutOfRangeException($"Offset too large!\n" +
                                        "offset: {item.Offset}");
                                }
                                if (item.Offset + item.Size > fs.Length)
                                {
                                    throw new ArgumentOutOfRangeException($"Offset too large!\n" +
                                        "offset: {item.Offset}\n" +
                                        "size: {item.Size}");
                                }
                                fs.Seek(item.Offset, SeekOrigin.Begin);

                                byte[] osha = HashTester.GetHash(br.BaseStream, item.Size);
                                string testpath = System.IO.Path.Combine(Path_, item.Filename);
                                byte[] isha = HashTester.GetHashClose(GetFsRead(testpath));
                                if (isha == null)
                                {
                                    Logger.WriteLineThrow($"failed to verify ({testpath}) sha1 value is null");
                                }
                                else if (!isha.SequenceEqual(osha))
                                {
                                    Logger.WriteLineThrow($"failed to verify ({testpath}) sha1 mismatch \n" +
                                        $"sha1:   {BitConverter.ToString(isha).Replace("-", "")} != {BitConverter.ToString(osha).Replace("-", "")}\n" +
                                        $"offset: {item.Offset}\n" +
                                        $"size:   {item.Size}");
                                }
                                else
                                {
                                    Logger.WriteLine($"Verified ({testpath}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                                }
                            }
                        }
                    }
                }
            return System.IO.Path.GetDirectoryName(Out);
        }
    }
}