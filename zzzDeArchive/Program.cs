using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            using (FileStream fs = File.OpenRead(_in))
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
                                    fs.Seek((long)d.Offset, SeekOrigin.Begin);
                                    bw.Write(br.ReadBytes((int)d.Size));
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
            Args = new List<string>(args);
            Args.ForEach(x => x.Trim('"'));
            if (Args.Count == 2 && File.Exists(Args[0]) && File.Exists(Args[1]))
            {
                //merge
                _in = Args[0];
                _path = Args[1];
                Merge();
            }
            else if (Args.Count == 2 && File.Exists(Args[0]))
            {
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
            else if (Args.Count == 1 && Directory.Exists(Args[0]))
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
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            void openfolder(string folder)
            {
                try
                {
                    if (Directory.Exists(folder))
                        Process.Start(folder);
                }
                catch
                {
                }
            }
        }

        private static ConsoleKeyInfo MainMenu()
        {
            ConsoleKeyInfo k;
            do
            {
                Console.Write(
                    "            --- Welcome to the zzzDeArchive 0.1.5.1 ---\n" +
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
            while (k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2 && k.Key != ConsoleKey.D3 && k.Key != ConsoleKey.NumPad3);
            return k;
        }

        private static string Merge()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), _out);
            (BinaryReader _in, BinaryReader _out) br;
            using (br._in = new BinaryReader(File.OpenRead(_in)))
            using (br._out = new BinaryReader(File.OpenRead(_path)))
            {
                (ZzzHeader _in, ZzzHeader _out) head = (ZzzHeader.Read(br._in), ZzzHeader.Read(br._out));
                (ZzzHeader head, BinaryWriter bw) merged;

                Debug.Assert(head._in.Data.Last().Size + head._in.Data.Last().Offset == (ulong)br._in.BaseStream.Length);
                Debug.Assert(head._out.Data.Last().Size + head._out.Data.Last().Offset == (ulong)br._out.BaseStream.Length);
                merged.head = ZzzHeader.Merge(head._in, ref head._out);
                using (merged.bw = new BinaryWriter(File.Create(path)))
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
                            merged.bw.Write(_br.ReadBytes((int)i.Size));
                        }
                    }
                    Debug.Assert(merged.head.Data.Last().Size + merged.head.Data.Last().Offset == (ulong)merged.bw.BaseStream.Length);
                }
                Console.WriteLine($"Saved to: {path}");
            }
            return Path.GetDirectoryName(path);
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
                good = File.Exists(path);
                if (!good)
                    Console.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);
            _path = path;
            return Merge();
        }

        private static string Write()
        {
            ZzzHeader head = ZzzHeader.Read(_path, out string[] f);
            string path = Path.Combine(Directory.GetCurrentDirectory(), _out);
            Console.WriteLine(head);
            using (FileStream fs = File.Create(path))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    head.Write(bw);
                    Console.WriteLine($"Writing raw file data from {_path}");
                    foreach (string file in f)
                    {
                        FileInfo fi = new FileInfo(file);
                        Console.WriteLine($"Writing {file} {fi.Length} bytes");
                        bw.Write(File.ReadAllBytes(file));
                    }
                }
            }
            Console.WriteLine($"Saved to: {path}");
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
            public uint FilenameLength;
            public ulong Offset;
            public uint Size;

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
                    FilenameLength = (uint)filenamebytes.Length;
                }
            }

            public int TotalBytes => (int)(sizeof(uint) * 4 + FilenameLength);

            #endregion Properties

            #region Methods

            //static readonly char[] invalid = Path.GetInvalidPathChars();
            public static FileData Read(BinaryReader br)
            {
                FileData r = new FileData
                {
                    FilenameLength = br.ReadUInt32()
                };
                r.filenamebytes = br.ReadBytes((int)r.FilenameLength);
                //var tmp = r.Filename.Where(x => invalid.Contains(x));
                //if (tmp.Count() > 0)
                //    throw new InvalidDataException($"String ({r.Filename}) contains invalid characters! ({tmp})");
                r.Offset = br.ReadUInt64();
                r.Size = br.ReadUInt32();
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
                    Size = (uint)fi.Length
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

            public uint Count;
            public FileData[] Data;

            #endregion Fields

            #region Properties

            public int TotalBytes => sizeof(uint) + (from x in Data select x.TotalBytes).Sum();

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
                ZzzHeader r;
                List<FileData> data = new List<FileData>((int)@out.Count);
                // grab the files that are unique to @out. Replacing that bit of the header
                @out.Data = @out.Data.Where(x => @in.Data.Where(y => y.Filename.Equals(x.Filename, StringComparison.OrdinalIgnoreCase)).Count() == 0).ToArray();
                @out.Count = (uint)@out.Data.Length;
                foreach (FileData i in @out.Data)
                {
                    data.Add(i);
                }
                foreach (FileData i in @in.Data)
                {
                    data.Add(i);
                }
                r.Count = (uint)data.Count();
                r.Data = data.ToArray();
                ulong offset = (ulong)r.TotalBytes;
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
                    Count = br.ReadUInt32()
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
            public static ZzzHeader Read(string path, out string[] f)
            {
                ZzzHeader r = new ZzzHeader();
                f = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                r.Count = (uint)f.Length;
                r.Data = new FileData[r.Count];

                for (int i = 0; i < r.Count; i++)
                    r.Data[i] = FileData.Read(f[i]);

                uint pos = (uint)r.TotalBytes;

                //cannot know the size of the header till i had loaded the rest of the data.
                //so now we are updating the offset to be past the header. in the same order as the files.
                for (int i = 0; i < r.Count; i++)
                {
                    r.Data[i].Offset = pos;
                    pos += r.Data[i].Size;
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