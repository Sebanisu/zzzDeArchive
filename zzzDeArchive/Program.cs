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
        #region Fields

        //private const string _in = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\main.zzz.old";
        //private const string _in = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\other.zzz";

        private const string _out = @"out.zzz";
        private static string _path;
        private static string _in;

        public static List<string> Args { get; private set; }

        //private const string _path = @"D:\ext";

        #endregion Fields

        #region Methods

        private static void Write()
        {
            ZzzHeader head = ZzzHeader.Read(_path, out string[] f);
            string path = Path.Combine(Directory.GetCurrentDirectory(), _out);
            Console.WriteLine(head);
            using (FileStream fs = File.Create(path))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    head.Write(bw);
                    foreach (string file in f)
                    {
                        bw.Write(File.ReadAllBytes(file));
                    }
                }
            }
            Console.WriteLine($"Saved to: {path}");
            try
            {
                Process.Start(Path.GetDirectoryName(path));
            }
            catch
            {
            }
        }

        private static void Extract()
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
            try
            {
                Process.Start(_path);
            }
            catch
            {
            }
        }

        private static void Main(string[] args)
        {
            Args = new List<string>(args);
            Args.ForEach(x => x.Trim('"'));
            if (Args.Count == 2 && File.Exists(Args[0]))
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
                    ExtractMenu();
                }
                else if (k.Key == ConsoleKey.D2 || k.Key == ConsoleKey.NumPad2)
                {
                    WriteMenu();
                }
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        private static ConsoleKeyInfo MainMenu()
        {
            ConsoleKeyInfo k;
            do
            {
                Console.Write(
                    "            --- Welcome to the zzzDeArchive 0.11 ---\n" +
                    "     Code C# written by Sebanisu, Reversing and Python by Maki\n\n" +
                    "1) Extract - Extract zzz file\n" +
                    "2) Write - Write folder contents to a zzz file\n" +
                    "  Select: ");
                k = Console.ReadKey();
                Console.WriteLine();
            }
            while (k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2);
            return k;
        }

        private static void ExtractMenu()
        {
            string path;
            bool good = false;
            do
            {
                Console.Write(
                    "     Extract zzz Screen\n" +
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
                    "     Extract zzz Screen\n" +
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
            Extract();
        }

        private static void WriteMenu()
        {
            string path;
            bool good = false;
            do
            {
                Console.Write(
                    "     Write zzz Screen\n" +
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
            Write();
        }

        #endregion Methods

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
                    Console.WriteLine($"Writing {r}");
                    r.Write(bw);
                }
            }

            #endregion Methods
        }

        #endregion Structs
    }
}