using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace zzzDeArchive
{
    public class Program
    {
        //private const string _in = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\main.zzz.old";
        //private const string _in = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\other.zzz";

        #region Fields

        private const string _out = @"out.zzz";
        private static string _in;
        private static string _path;

        #endregion Fields

        #region Methods

        private static string Extract()
        {
            ZzzHeader head;
            using (FileStream fs = File.Open(_in, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    head = ZzzHeader.Read(br);
                    Console.WriteLine(head);

                    //Directory.CreateDirectory(_path);
                    foreach (FileData d in head.Data)
                    {
                        Console.WriteLine($"Writing {d}");
                        string path = Path.Combine(_path, d.Filename);
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        using (FileStream fso = File.Create(path))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fso))
                            {
                                if (d.Offset <= long.MaxValue)
                                {
                                    fs.Seek(d.Offset, SeekOrigin.Begin);
                                    bw.Write(br.ReadBytes(d.Size));
                                }
                                else throw new ArgumentOutOfRangeException($"d.offset is too large! ({d.Offset})");
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Saved to: {_path}");
            return _path;
        }

        private static string ExtractMenu()
        {
            string path;
            bool good = false;
            const string title = "\n     Extract zzz Screen\n";
            do
            {
                Console.Write(
                    title +
                    "Enter the path to zzz file: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = File.Exists(path);
                if (!good)
                    Console.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            _in = path;
            do
            {
                Console.Write(
                    title +
                    "Enter the path to extract contents: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                path = Path.GetFullPath(path);
                Console.WriteLine();
                Directory.CreateDirectory(path);
                good = Directory.Exists(path);
                if (!good)
                    Console.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            _path = path;
            return Extract();
        }

        private static void Main(string[] args)
        {
            sha = new SHA1CryptoServiceProvider();
            Args = new List<string>(args);
            Args.ForEach(x => x.Trim('"'));
            if (Args.Count == 2 && File.Exists(Args[0] = Path.GetFullPath(Args[0])) && File.Exists(Args[1] = Path.GetFullPath(Args[1])))
            {
                //merge
                _in = Args[0];
                _path = Args[1];
                Merge();
            }
            else if (Args.Count == 2 && File.Exists(Args[0] = Path.GetFullPath(Args[0])))
            {
                Args[1] = Path.GetFullPath(Args[1]);
                Directory.CreateDirectory(Args[1]);
                if (Directory.Exists(Args[1]))
                {
                    _in = Args[0];
                    _path = Args[1];
                    Extract();
                }
                else
                    Console.WriteLine("Invalid Directory");
            }
            else if (Args.Count == 1 && Directory.Exists(Args[0] = Path.GetFullPath(Args[0])))
            {
                _path = Args[0];
                Write();
            }
            else
            {
                ConsoleKeyInfo k = MainMenu();
                if (k.Key == ConsoleKey.D1 || k.Key == ConsoleKey.NumPad1)
                {
                    openfolder(ExtractMenu());
                }
                else if (k.Key == ConsoleKey.D2 || k.Key == ConsoleKey.NumPad2)
                {
                    openfolder(WriteMenu());
                }
                else if (k.Key == ConsoleKey.D3 || k.Key == ConsoleKey.NumPad3)
                {
                    openfolder(MergeMenu());
                }
                else if(k.Key == ConsoleKey.T)
                {
                    TestMenu();
                }
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            void openfolder(string folder)
            {
                try
                {
                    folder = Path.GetFullPath(folder);
                    if (Directory.Exists(folder))
                        Process.Start(folder);
                }
                catch
                {
                }
            }
        }

        private static void TestMenu()
        {
            string path;
            bool good = false;
            do
            {
                Console.Write(
                    "\n  Test Writes zzz Debug Screen\n" +
                    "Warning! this is a test screen\n" +
                    "This will keep making zzz files till it's done or errors\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = Directory.Exists(path);
                if (!good)
                    Console.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            LoadSubDirs(path);
            void LoadSubDirs(string dir)
            {
                Console.WriteLine($"Testing: {dir}\n");
                string[] subdirectoryEntries = Directory.GetDirectories(dir);
                _path = dir;
                Write();
                foreach (string subdir in subdirectoryEntries)
                    LoadSubDirs(subdir);
            }
        }

        private static ConsoleKeyInfo MainMenu()
        {
            ConsoleKeyInfo k;
            do
            {
                Console.Write(
                    "            --- Welcome to the zzzDeArchive 0.1.6.0 ---\n" +
                    "     Code C# written by Sebanisu, Reversing and Python by Maki\n\n" +
                    "1) Extract - Extract zzz file\n" +
                    "2) Write - Write folder contents to a zzz file\n" +
                    "3) Merge - Write unique data from two zzz files into one zzz file.\n" +
                    "  Select: ");
                k = Console.ReadKey();
                Console.WriteLine();
                if (k.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
            }
            while (k.Key != ConsoleKey.T && k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2 && k.Key != ConsoleKey.D3 && k.Key != ConsoleKey.NumPad3);
            return k;
        }

        private static string Merge()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), _out);
            (BinaryReader _in, BinaryReader _out) br;
            using (br._in = new BinaryReader(File.Open(_in, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            using (br._out = new BinaryReader(File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                (ZzzHeader _in, ZzzHeader _out) head = (ZzzHeader.Read(br._in), ZzzHeader.Read(br._out));
                (ZzzHeader head, BinaryWriter bw, BinaryReader br) merged;

                TestSize(head._in, br._in.BaseStream);
                TestSize(head._out, br._out.BaseStream);
                merged.head = ZzzHeader.Merge(head._in, ref head._out);

                using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                using (merged.bw = new BinaryWriter(fs))
                using (merged.br = new BinaryReader(fs))
                {
                    merged.head.Write(merged.bw);

                    Console.WriteLine($"Writing raw file data from {_path}");
                    Write(br._out, head._out.Data);
                    Console.WriteLine($"Writing raw file data from {_in}");
                    Write(br._in, head._in.Data);

                    void Write(BinaryReader _br, FileData[] data)
                    {
                        foreach (FileData i in data)
                        {
                            _br.BaseStream.Seek((int)i.Offset, SeekOrigin.Begin);
                            Console.WriteLine($"Writing {i.Filename} {i.Size} bytes");
                            merged.bw.Write(_br.ReadBytes(i.Size));
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
                        byte[] osha = sha.ComputeHash(merged.br.ReadBytes(item.Size));
                        byte[] isha = null;
                        FileData tmphead = head._in.Data.FirstOrDefault(x => x.Filename.Equals(item.Filename, StringComparison.OrdinalIgnoreCase));
                        string src;
                        if (tmphead.Equals(new FileData()))
                        {
                            src = _path;
                            tmphead = head._out.Data.First(x => x.Filename.Equals(item.Filename,StringComparison.OrdinalIgnoreCase));
                            br._out.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = sha.ComputeHash(br._out.ReadBytes(tmphead.Size));
                        }
                        else
                        {
                            src = _in;
                            br._in.BaseStream.Seek(tmphead.Offset, SeekOrigin.Begin);
                            isha = sha.ComputeHash(br._in.ReadBytes(tmphead.Size));
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
            return Path.GetDirectoryName(path);
        }

        private static void TestSize(ZzzHeader head, Stream stream)
        {
            if (head.ExpectedFileSize != stream.Length)
            {
                throw new Exception($"expected filesize ({head.ExpectedFileSize}) != resulting filesize ({stream.Length})");
            }
        }

        private static string MergeMenu()
        {
            string path;
            bool good = false;
            const string title = "\n     Merge zzz Screen\n";
            do
            {
                Console.Write(
                    title +
                    "  Only unchanged data will be kept, rest will be replaced...\n" +
                    "Enter the path to zzz file with new data: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = File.Exists(path);
                if (!good)
                    Console.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            _in = path;
            do
            {
                Console.Write(
                    title +
                    "Enter the path to zzz file with old data: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = File.Exists(path);
                if (!good)
                    Console.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);
            _path = path;
            return Merge();
        }

        private static HashAlgorithm sha;

        private static string Write()
        {
            ZzzHeader head = ZzzHeader.Read(_path, out string[] files);
            string path = Path.Combine(Directory.GetCurrentDirectory(), _out);
            Console.WriteLine(head);
            using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    head.Write(bw);
                    Console.WriteLine($"Writing raw file data from {_path}");
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
                            byte[] osha = sha.ComputeHash(br.ReadBytes(item.Size));
                            byte[] isha = null;
                            string testpath = Path.Combine(_path, item.Filename);
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
            return Path.GetDirectoryName(path);
        }

        //private const string _path = @"D:\ext";
        private static string WriteMenu()
        {
            string path;
            bool good = false;
            do
            {
                Console.Write(
                    "\n     Write zzz Screen\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = Directory.Exists(path);
                if (!good)
                    Console.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);

            _path = path;
            return Write();
        }

        #endregion Methods

        #region Properties

        public static List<string> Args { get; private set; }

        #endregion Properties

        #region Structs

        /// <summary>
        /// Part of header that contains info on the files.
        /// </summary>
        /// <see cref="https://github.com/myst6re/qt-zzz/blob/master/zzztoc.h"/>
        public struct FileData
        {
            #region Fields

            private byte[] filenamebytes;
            public int FilenameLength;
            public long Offset;
            public int Size;

            #endregion Fields

            #region Properties

            /// <summary>
            /// Decode/Encode the filename string as bytes.
            /// </summary>
            /// <remarks>
            /// Could be Ascii or UTF8, I see no special characters and the first like 127 of UTF8 is
            /// the same as Ascii.
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

            //static readonly char[] invalid = Path.GetInvalidPathChars();
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
                r.Offset = br.ReadInt64();
                r.Size = br.ReadInt32();
                return r;
            }

            public static FileData Read(string path)
            {
                string safe = path;
                safe = safe.Replace(_path, "");
                safe = safe.Replace('/', '\\');
                safe = safe.Trim('\\');
                FileInfo fi = new FileInfo(path);
                FileData r = new FileData
                {
                    Filename = safe,
                    Size = (int)fi.Length
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
        public struct ZzzHeader
        {
            #region Fields

            public int Count;
            public FileData[] Data;
            public long ExpectedFileSize => Data.Last().Size + Data.Last().Offset;

            #endregion Fields

            #region Properties

            public int TotalBytes => sizeof(int) + (from x in Data select x.TotalBytes).Sum();

            #endregion Properties

            #region Methods

            /// <summary>
            /// Create a new header that contains data not replaced from old file and replaced data
            /// from new file; This will modify out header to remove any files that are being replaced.
            /// </summary>
            /// <param name="in">in files header</param>
            /// <param name="out">
            /// out files header, This will modify out to remove any files that are being replaced.
            /// </param>
            /// <returns>merged header</returns>
            public static ZzzHeader Merge(ZzzHeader @in, ref ZzzHeader @out)
            {
                Console.WriteLine("Merging Headers");
                ZzzHeader r;
                List<FileData> data = new List<FileData>(@out.Count);
                List<FileData> out2 = new List<FileData>(@out.Data);
                // grab the files that are unique to @out. Replacing that bit of the header
                Console.WriteLine("Eliminating Duplicates...");
                for (int i = 0; i < out2.Count; i++)
                {
                    if (@in.Data.Any(x => x.Filename.Equals(out2[i].Filename, StringComparison.OrdinalIgnoreCase)))
                        out2.RemoveAt(i--);
                }
                Console.WriteLine($"Eliminated {@out.Count - out2.Count}");
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

            public static ZzzHeader Read(BinaryReader br)
            {
                ZzzHeader r = new ZzzHeader
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
            public static ZzzHeader Read(string path, out string[] files)
            {
                ZzzHeader r = new ZzzHeader();
                files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                r.Count = files.Length;
                r.Data = new FileData[r.Count];

                for (int i = 0; i < r.Count; i++)
                    r.Data[i] = FileData.Read(files[i]);

                int pos = r.TotalBytes;

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

        #endregion Structs
    }
}