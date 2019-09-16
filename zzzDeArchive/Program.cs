using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ZzzFile;

namespace ZzzConsole
{
    public class Program
    {
        //private const string zzz.In = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\main.zzz.old";
        //private const string zzz.In = @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered\other.zzz";

        #region Fields

        private static Zzz zzz = new Zzz();

        #endregion Fields

        #region Methods

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

            zzz.In.Add(path);
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
            zzz.Path = path;
            return zzz.Extract();
        }

        private static void Main(string[] args)
        {
            Args = new List<string>(args);
            Args.ForEach(x => x.Trim('"'));
            if (Args.Count >= 2 && File.Exists(Args[0] = Path.GetFullPath(Args[0])) && File.Exists(Args[1] = Path.GetFullPath(Args[1])))
            {
                //merge

                zzz.Path = Args[0];
                for(int i = 1; i < Args.Count; i++)
                    zzz.In.Add(Args[i]);
                zzz.Merge();
            }
            else if (Args.Count == 2 && File.Exists(Args[0] = Path.GetFullPath(Args[0])))
            {
                Args[1] = Path.GetFullPath(Args[1]);
                Directory.CreateDirectory(Args[1]);
                if (Directory.Exists(Args[1]))
                {
                    zzz.In.Add(Args[0]);
                    zzz.Path = Args[1];
                    zzz.Extract();
                }
                else
                    Console.WriteLine("Invalid Directory");
            }
            else if (Args.Count == 1 && Directory.Exists(Args[0] = Path.GetFullPath(Args[0])))
            {
                zzz.Path = Args[0];
                zzz.Write();
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
                else if (k.Key == ConsoleKey.T)
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

        private static ConsoleKeyInfo MainMenu()
        {
            ConsoleKeyInfo k;
            do
            {
                Console.Write(
                    "            --- Welcome to the zzzDeArchive 0.1.6.5 ---\n" +
                    "     Code C# written by Sebanisu, Reversing and Python by Maki\n\n" +
                    "1) Extract - Extract zzz file\n" +
                    "2) Write - Write folder contents to a zzz file\n" +
                    "3) Merge - Write unique data from two zzz files into one zzz file.\n" +
                    //"4) FolderMerge - Automaticly merge files in the IN subfolder.\n" +
                    "  Select: ");
                k = Console.ReadKey();
                Console.WriteLine();
                if (k.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
            }
            while (k.Key != ConsoleKey.T && k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2 && k.Key != ConsoleKey.D3 && k.Key != ConsoleKey.NumPad3);
            return k;
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
                    "Enter the path to zzz file with original/old data: ");
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

            zzz.Path = path;
            do
            {
                Console.Write(
                    title +
                    "Enter the path to zzz file with new data: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Console.WriteLine();
                path = Path.GetFullPath(path);
                good = File.Exists(path);
                if (good)
                    zzz.In.Add(path);
                else if (string.IsNullOrWhiteSpace(path) && zzz.In.Count > 0)
                    break;
                else
                    Console.WriteLine("File doesn't exist\n");
            }
            while (true);
            return zzz.Merge();
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
                zzz.Path = dir;
                zzz.Write();
                foreach (string subdir in subdirectoryEntries)
                    LoadSubDirs(subdir);
            }
        }

        //private const string zzz.Path = @"D:\ext";
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

            zzz.Path = path;
            return zzz.Write();
        }

        #endregion Methods

        #region Properties

        public static List<string> Args { get; private set; }

        #endregion Properties
    }
}