using _Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZzzFile
{ /// <summary>
  /// Part of header that contains info on the files. </summary> <see cref="https://github.com/myst6re/qt-zzz/blob/master/zzztoc.h"/>
    public struct FileData
    {
        #region Fields

        private byte[] filenamebytes;
        public int FilenameLength;
        public long Offset;

        /// <summary>
        /// Needs to be uint to support 4gb files? Though probably over kill.
        /// </summary>
        public uint Size;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Decode/Encode the filename string as bytes.
        /// </summary>
        /// <remarks>
        /// Could be Ascii or UTF8, I see no special characters and the first like 127 of UTF8 is the
        /// same as Ascii.
        /// </remarks>
        public string Filename
        {
            get => Encoding.UTF8.GetString(filenamebytes); set
            {
                filenamebytes = Encoding.UTF8.GetBytes(value);
                FilenameLength = filenamebytes.Length;
            }
        }

        public int TotalBytes => sizeof(int) * 4 + FilenameLength;

        #endregion Properties

        #region Methods

        // readonly char[] invalid = Path.GetInvalidPathChars();
        public static FileData Read(BinaryReader br)
        {
            FileData r = new FileData
            {
                FilenameLength = br.ReadInt32()
            };
            r.filenamebytes = br.ReadBytes(r.FilenameLength);
            //var tmp = r.Filename.Where(x => invalid.Contains(x));
            //if (tmp.Count() > 0)
            //    throw new InvalidDataException($"String ({r.Filename}) contains invalid characters! ({tmp})");
            r.Offset = br.ReadInt64(); // if we are reading more than this we are on future tech.
            r.Size = br.ReadUInt32();
            return r;
        }

        public static FileData Read(string path, string _path)
        {
            string safe = path;
            safe = safe.Replace(_path, "");
            safe = safe.Replace('/', '\\');
            safe = safe.Trim('\\');
            FileInfo fi = new FileInfo(path);
            FileData r = new FileData
            {
                Filename = safe,
                Size = checked((uint)fi.Length) // zzz file only supports indivitual files of size uint.
            };
            return r;
        }

        public override string ToString() => $"({Filename}, {Offset}, {Size})";

        public void Write(BinaryWriter bw)
        {
            bw.Write(FilenameLength);
            bw.Write(filenamebytes);
            bw.Write(Offset);
            bw.Write(Size);
        }

        #endregion Methods
    }

    /// <summary>
    /// Header for ZZZ file.
    /// </summary>
    public struct Header
    {
        #region Methods

        private static Header CorrectOffsets(ref Header r)
        {
            long offset = r.TotalBytes;
            for (int i = 0; i < r.Count; i++)
            {
                r.Data[i].Offset = offset;
                offset += r.Data[i].Size;
            }

            return r;
        }

        private static void EliminateDuplicates(ref Header @out, Header[] @in,bool skipwarning = false)
        {
            Logger.WriteLine("Eliminating Duplicates for input zzz files from other input zzz files...");
            for (int i = 0; i < @in.Length; i++)
                for (int j = i + 1; j < @in.Length; j++)
                    EliminateDuplicates(ref @in[i], @in[j], true);

            Logger.WriteLine("Eliminating Duplicates for input zzz files from the original zzz file...");
            for (int i = 0; i < @in.Length; i++)
                EliminateDuplicates(ref @out, @in[i],skipwarning);
        }

        private static void EliminateDuplicates(ref Header @out, Header @in, bool skipwarning)
        {
            List<FileData> out2 = new List<FileData>(@out.Data);
            List<FileData> in2 = new List<FileData>();
            // grab the files that are unique to @out. Replacing that bit of the header
            //Logger.WriteLine("Eliminating Duplicates...");
            //for (int i = 0; i < out2.Count; i++)
            //{
            //    if (@in.Data.Any(x => x.Filename.Equals(out2[i].Filename, StringComparison.OrdinalIgnoreCase)))
            //        out2.RemoveAt(i--);
            //}
            for (int i = 0; i < @in.Count; i++)
            {
                int ind = 0;
                string fn = @in.Data[i].Filename;
                if ((ind = out2.FindIndex(x => x.Filename.Equals(fn, StringComparison.OrdinalIgnoreCase))) > -1)
                    out2.RemoveAt(ind);
                else
                    in2.Add(@in.Data[i]);
            }
            if (@out.Count - out2.Count > 0)
                Logger.WriteLine($"Eliminated {@out.Count - out2.Count}");
            if (@out.Count - out2.Count < @in.Count && !skipwarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.WriteLine($"WARNING you are not replacing all {@in.Count} files. \n" +
                    $"There are going to be {Math.Abs(@out.Count - out2.Count - @in.Count)} files added!\n" +
                    $"The game may ignore any new files it is not expecting...");
                Console.ForegroundColor = ConsoleColor.White;
                Logger.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Logger.WriteLine("-- List of new files --");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (FileData i in in2)
                {
                    Logger.WriteLine(i.ToString());
                }
                Console.ForegroundColor = ConsoleColor.White;
                Logger.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            @out.Count = out2.Count;
            @out.Data = out2.ToArray();
        }

        #endregion Methods

        #region Fields

        public int Count;
        public FileData[] Data;

        #endregion Fields

        #region Properties

        public long ExpectedFileSize => (Data?.Last().Size ?? 0) + (Data?.Last().Offset ?? 0);

        public int TotalBytes
        {
            get
            {
                if ((Data?.Length ?? 0) > 0)
                    return sizeof(int) + (from x in Data select x.TotalBytes).Sum();
                return sizeof(int);
            }
        }

        #endregion Properties

        public static Header Merge(ref Header @out, ref Header[] @in, bool skipwarning = false)
        {
            Logger.WriteLine("Merging Headers");
            Header r = new Header();
            EliminateDuplicates(ref @out, @in,skipwarning);

            for (int i = 0; i < @in.Length; i++)
                Merge(ref @out, ref @in[i], ref r);

            return CorrectOffsets(ref r);
        }

        public static Header Merge(ref Header @out, ref Header @in)
        {
            Header r = new Header();
            return Merge(ref @out, ref @in, ref r);
        }

        /// <summary>
        /// Create a new header that contains data not replaced from old file and replaced data from
        /// new file; This will modify out header to remove any files that are being replaced.
        /// </summary>
        /// <param name="in">in files header</param>
        /// <param name="out">
        /// out files header, This will modify out to remove any files that are being replaced.
        /// </param>
        /// <returns>merged header</returns>
        public static Header Merge(ref Header @out, ref Header @in, ref Header r)
        {
            List<FileData> data = r.Data != null ? new List<FileData>(r.Data) : new List<FileData>();
            if (r.Count == 0)
                data.AddRange(@out.Data);
            data.AddRange(@in.Data);
            r.Count = data.Count();
            r.Data = data.ToArray();
            return r;
        }

        public static Header Read(BinaryReader br)
        {
            Header r = new Header
            {
                Count = br.ReadInt32()
            };
            r.Data = new FileData[r.Count];
            for (int i = 0; i < r.Count; i++)
                r.Data[i] = FileData.Read(br);
            return r;
        }

        /// <summary>
        /// This creates the header using the files in a directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Header Read(string path, out string[] files, string _path)
        {
            Header r = new Header();
            files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            r.Count = files.Length;
            r.Data = new FileData[r.Count];

            for (int i = 0; i < r.Count; i++)
                r.Data[i] = FileData.Read(files[i], _path);

            long pos = r.TotalBytes;

            //cannot know the size of the header till i had loaded the rest of the data.
            //so now we are updating the offset to be past the header. in the same order as the files.
            for (int i = 0; i < r.Count; i++)
            {
                r.Data[i].Offset = pos;
                pos += r.Data[i].Size;
            }
            if (r.Count != files.Length)
            {
                throw new Exception($"Header count {r.Count} != number of files in directory {files.Length}.");
            }
            return r;
        }

        public override string ToString() => $"({Count} files)";

        public void Write(BinaryWriter bw)
        {
            bw.Write(Count);
            if (Data != null)
                foreach (FileData r in Data)
                {
                    Logger.WriteLine($"Writing FileData {r}");
                    r.Write(bw);
                }
            Logger.WriteLine($"Header data written {TotalBytes} bytes");
        }
    }

    public class Zzz
    {
        #region Fields

        private static HashAlgorithm sha;
        private readonly string other;
        private List<string> _in = new List<string>(1);
        private string _out;
        private string _path;

        #endregion Fields

        #region Methods

        private static FileStream GetFs(ref string path)
        {
            string path_ = path;
            FileStream fs;
            int i = 0;
            do
            {
                try
                {
                    fs = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                }
                catch (IOException e)
                {
                    fs = null;
                    Logger.Write($"{e.Message} :: Error writing to: {path}\n Going to increment file and try again...");
                    path = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(path_),
                        $"{System.IO.Path.GetFileNameWithoutExtension(path_)}{i++}.zzz");
                }
            }
            while (fs == null);

            return fs;
        }

        private static byte[] GetHash(BinaryReader br, uint size)
        {
            byte[] sha;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter tmp = new BinaryWriter(ms))
            {
                ReadUInt(tmp, br, size);
                //ms.SetLength(size);
                ms.Seek(0, SeekOrigin.Begin);
                sha = Zzz.sha.ComputeHash(ms);
            }

            return sha;
        }

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
            if (head.ExpectedFileSize != stream.Length)
            {
                throw new Exception($"expected filesize ({head.ExpectedFileSize}) != resulting filesize ({stream.Length})");
            }
        }

        #endregion Methods

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
            sha = new SHA1CryptoServiceProvider();
        }

        //public Zzz(string path, List<string> @in, string @out = null)
        //{
        //    sha = new SHA1CryptoServiceProvider();
        //    Path_ = path;
        //    Out = @out;
        //    In = @in;
        //}

        #endregion Constructors

        #region Properties

        public string id1 { get; }
        public string id2 { get; }
        public List<string> In { get => _in; set => _in = value; }
        public string main { get; }
        public string od { get; }
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

        public bool SkipWarning { get; set; } = false;
        public string Path_ { get => _path; set => _path = value; }
        public string Main { get; set; }
        public string Other { get; set; }

        #endregion Properties

        public string Extract()
        {
            Header head;
            using (FileStream fs = File.Open(In.First(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    head = Header.Read(br);
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
                                else throw new ArgumentOutOfRangeException($"d.offset is too large! ({d.Offset})");
                            }
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
            Merge(f1, main,Main);
            Merge(f2, other,Other);
            return od;
        }

        private void Write(List<string> d1)
        {
            var path = Path_;
            foreach (var d in d1)
            {
                Path_ = d;
                Out = $"{d}.zzz";
                Write();
            }
            Path_ = path;
        }

        private void Merge(List<string> f1, string main,string arg = null)
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

        private bool IsMainOrOther(string x) => IsMainOrOther(main, x) || IsMainOrOther(other, x);

        private static bool IsMainOrOther(string main, string x) => Path.GetFileName(x).Equals(main, StringComparison.OrdinalIgnoreCase);

        public string Merge()
        {
            if (Out != null) { };
            (BinaryReader[] _in, BinaryReader _out) br;
            (Header[] _in, Header _out) head;
            Logger.WriteLine($"Opening {Path_}");
            using (br._out = new BinaryReader(File.Open(Path_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                head._out = Header.Read(br._out);
                TestSize(head._out, br._out.BaseStream);
                br._in = new BinaryReader[In.Count];
                head._in = new Header[In.Count];
                for (int i = 0; i < In.Count; i++)
                {
                    Logger.WriteLine($"Opening {In[i]}");
                    br._in[i] = new BinaryReader(File.Open(In[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    head._in[i] = Header.Read(br._in[i]);
                    TestSize(head._in[i], br._in[i].BaseStream);
                }
                (Header head, BinaryWriter bw, BinaryReader br) merged;

                merged.head = Header.Merge(ref head._out, ref head._in, SkipWarning);

                using (FileStream fs = GetFs(ref _out))
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
                        byte[] osha = null;
                        osha = GetHash(merged.br, item.Size);
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
                                isha = GetHash(br._in[i], tmphead.Size);
                                break;
                            }
                        }
                        if (tmphead.Equals(new FileData()))
                        {
                            src = Path_;
                            tmphead = head._out.Data.First(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            br._out.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = GetHash(br._out, tmphead.Size);
                        }
                        if (isha == null)
                        {
                            throw new Exception($"failed to verify ({item.Filename}) sha1 value is null");
                        }
                        else if (!isha.SequenceEqual(osha))
                        {
                            throw new Exception($"failed to verify ({item.Filename}) sha1 mismatch \n" +
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
            Header head = Header.Read(Path_, out string[] files, Path_);
            Logger.WriteLine(head.ToString());
            if (Out != null)
                using (FileStream fs = GetFs(ref _out))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        head.Write(bw);
                        Logger.WriteLine($"Writing raw file data from {Path_}");
                        foreach (string file in files)
                        {
                            FileInfo fi = new FileInfo(file);
                            Logger.WriteLine($"Writing {file} {fi.Length} bytes");
                            using (BinaryReader br = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
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

                                byte[] osha = GetHash(br, item.Size);
                                byte[] isha = null;
                                string testpath = System.IO.Path.Combine(Path_, item.Filename);
                                using (BinaryReader br2 = new BinaryReader(File.Open(testpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                                {
                                    isha = sha.ComputeHash(br2.BaseStream);
                                }
                                if (isha == null)
                                {
                                    throw new Exception($"failed to verify ({testpath}) sha1 value is null");
                                }
                                else if (!isha.SequenceEqual(osha))
                                {
                                    throw new Exception($"failed to verify ({testpath}) sha1 mismatch \n" +
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