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
        #region Fields

        public int Count;
        public FileData[] Data;

        #endregion Fields

        #region Properties

        public long ExpectedFileSize => Data.Last().Size + Data.Last().Offset;

        public int TotalBytes => sizeof(int) + (from x in Data select x.TotalBytes).Sum();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new header that contains data not replaced from old file and replaced data from
        /// new file; This will modify out header to remove any files that are being replaced.
        /// </summary>
        /// <param name="in">in files header</param>
        /// <param name="out">
        /// out files header, This will modify out to remove any files that are being replaced.
        /// </param>
        /// <returns>merged header</returns>
        public static Header Merge(Header @in, ref Header @out)
        {
            Console.WriteLine("Merging Headers");
            Header r;
            List<FileData> data = new List<FileData>(@out.Count);
            List<FileData> out2 = new List<FileData>(@out.Data);
            List<FileData> in2 = new List<FileData>();
            // grab the files that are unique to @out. Replacing that bit of the header
            Console.WriteLine("Eliminating Duplicates...");
            //for (int i = 0; i < out2.Count; i++)
            //{
            //    if (@in.Data.Any(x => x.Filename.Equals(out2[i].Filename, StringComparison.OrdinalIgnoreCase)))
            //        out2.RemoveAt(i--);
            //}
            for (int i = 0; i < @in.Count; i++)
            {
                int ind = 0;
                if ((ind = out2.FindIndex(x => x.Filename.Equals(@in.Data[i].Filename, StringComparison.OrdinalIgnoreCase))) > -1)
                    out2.RemoveAt(ind);
                else
                    in2.Add(@in.Data[i]);
            }
            Console.WriteLine($"Eliminated {@out.Count - out2.Count}");
            if (@out.Count - out2.Count < @in.Count)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARNING you are not replacing all {@in.Count} files. \n" +
                    $"There are going to be {Math.Abs(@out.Count - out2.Count - @in.Count)} files added!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.WriteLine("-- List of new files --");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (FileData i in in2)
                {
                    Console.WriteLine(i);
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            @out.Count = out2.Count;
            @out.Data = out2.ToArray();

            foreach (FileData i in @out.Data)
            {
                data.Add(i);
            }
            foreach (FileData i in @in.Data)
            {
                data.Add(i);
            }
            r.Count = data.Count();
            r.Data = data.ToArray();
            long offset = r.TotalBytes;
            for (int i = 0; i < r.Count; i++)
            {
                r.Data[i].Offset = offset;
                offset += r.Data[i].Size;
            }
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
            foreach (FileData r in Data)
            {
                Console.WriteLine($"Writing FileData {r}");
                r.Write(bw);
            }
            Console.WriteLine($"Header data written {TotalBytes} bytes");
        }

        #endregion Methods
    }

    public class Zzz
    {
        #region Fields

        private static HashAlgorithm sha;
        private string _in;
        private string _out;
        private string _path;

        #endregion Fields

        #region Methods

        private static void TestSize(Header head, Stream stream)
        {
            if (head.ExpectedFileSize != stream.Length)
            {
                throw new Exception($"expected filesize ({head.ExpectedFileSize}) != resulting filesize ({stream.Length})");
            }
        }

        #endregion Methods

        #region Constructors

        public Zzz() => sha = new SHA1CryptoServiceProvider();

        public Zzz(string path, string @in, string @out = null)
        {
            sha = new SHA1CryptoServiceProvider();
            Path = path;
            Out = @out;
            In = @in;
        }

        #endregion Constructors

        #region Properties

        public string In { get => _in; set => _in = value; }
        public string Out
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_out))
                    return _out = @"out.zzz";
                return _out;
            }
            set => _out = value;
        }

        public string Path { get => _path; set => _path = value; }

        #endregion Properties

        public string Extract()
        {
            Header head;
            using (FileStream fs = File.Open(In, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    head = Header.Read(br);
                    Console.WriteLine(head);

                    //Directory.CreateDirectory(_path);
                    foreach (FileData d in head.Data)
                    {
                        Console.WriteLine($"Writing {d}");
                        string path = System.IO.Path.Combine(Path, d.Filename);
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
            Console.WriteLine($"Saved to: {Path}");
            return Path;
        }

        public string Merge()
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), Out);
            (BinaryReader _in, BinaryReader _out) br;
            using (br._in = new BinaryReader(File.Open(In, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            using (br._out = new BinaryReader(File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                (Header _in, Header _out) head = (Header.Read(br._in), Header.Read(br._out));
                (Header head, BinaryWriter bw, BinaryReader br) merged;

                TestSize(head._in, br._in.BaseStream);
                TestSize(head._out, br._out.BaseStream);
                merged.head = Header.Merge(head._in, ref head._out);

                using (FileStream fs = GetFs(ref path))
                using (merged.bw = new BinaryWriter(fs))
                using (merged.br = new BinaryReader(fs))
                {
                    merged.head.Write(merged.bw);
                    Console.WriteLine($"Writing raw file data from {Path}");
                    Write(br._out, head._out.Data);
                    Console.WriteLine($"Writing raw file data from {In}");
                    Write(br._in, head._in.Data);

                    void Write(BinaryReader _br, FileData[] data)
                    {
                        foreach (FileData i in data)
                        {
                            _br.BaseStream.Seek(i.Offset, SeekOrigin.Begin);
                            Console.WriteLine($"Writing {i.Filename} {i.Size} bytes");
                            ReadUInt(merged.bw, _br, i.Size);
                        }
                    }
                    Console.WriteLine($"Saved to: {path}");
                    Console.WriteLine($"Verifing output");
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
                        FileData tmphead = head._in.Data.FirstOrDefault(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                        string src;
                        if (tmphead.Equals(new FileData()))
                        {
                            src = Path;
                            tmphead = head._out.Data.First(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                            br._out.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = GetHash(br._out, tmphead.Size);
                        }
                        else
                        {
                            src = In;
                            br._in.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = GetHash(br._in, tmphead.Size);
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
                            Console.WriteLine($"Verified ({item.Filename}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                        }
                    }
                }
            }
            return System.IO.Path.GetDirectoryName(path);
        }

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
                    Console.Write($"{e.Message} :: Error writing to: {path}\n Going to increment file and try again...");
                    path = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(path_),
                        $"{System.IO.Path.GetFileNameWithoutExtension(path_)}{i++}.zzz");

                }
            }
            while (fs == null);

            return fs;
        }

        private static void ReadUInt(BinaryWriter bw, BinaryReader br, uint size)
        {
            while (size > 0)
            {

                int s = (size > int.MaxValue)? int.MaxValue : (int)size;
                bw.Write(br.ReadBytes(s));
                size -= (uint)s;
            }
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

        public string Write()
        {
            Header head = Header.Read(Path, out string[] files, Path);
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), Out);
            Console.WriteLine(head);
            using (FileStream fs =GetFs(ref path))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    head.Write(bw);
                    Console.WriteLine($"Writing raw file data from {Path}");
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        Console.WriteLine($"Writing {file} {fi.Length} bytes");
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

                    Console.WriteLine($"Saved to: {path}");
                    Console.WriteLine($"Verifing output");
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
                            string testpath = System.IO.Path.Combine(Path, item.Filename);
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
                                Console.WriteLine($"Verified ({testpath}) sha1({BitConverter.ToString(osha).Replace("-", "")})");
                            }
                        }
                    }
                }
            }
            return System.IO.Path.GetDirectoryName(path);
        }
    }
}