using _Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                Logger.Write(
                    title +
                    "Enter the path to zzz file: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                good = File.Exists(path);
                if (!good)
                    Logger.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            zzz.In.Add(path);
            do
            {
                Logger.Write(
                    title +
                    "Enter the path to extract contents: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                path = GetFullPath(path);
                Logger.WriteLine();
                Directory.CreateDirectory(path);
                good = Directory.Exists(path);
                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            zzz.Path_ = path;
            return zzz.Extract();
        }
        static string GetFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path);
            }
            catch(Exception e)
            {
                Logger.WriteLine($"{e.Message}");
                Logger.WriteLine($"path: {path}");
                return null;
            }
        }

        private static void Main(string[] args)
        {
            Args = new List<string>(args);
            Args.ForEach(x => x.Trim('"'));
            int ind = Args.FindIndex(x => x.Equals("-skipwarning", StringComparison.OrdinalIgnoreCase));
            if (ind >= 0)
            {
                zzz.SkipWarning = true;
                Args.RemoveAt(ind);
            }
            if ((Args.Count == 1 || Args.Count == 3) &&
                (Args[0].Equals("-foldermerge", StringComparison.OrdinalIgnoreCase) ||
                Args[0].Equals("-mergefolder", StringComparison.OrdinalIgnoreCase)) &&
                (Directory.EnumerateFiles(zzz.id1).Count() > 1 ||
                Directory.EnumerateFiles(zzz.id2).Count() > 1 ||
                Directory.EnumerateDirectories(zzz.id1).Count() > 1 ||
                Directory.EnumerateDirectories(zzz.id2).Count() > 1))
            {
                if (Args.Count == 3)
                {
                    string path = Args[1].Trim();
                    path = GetFullPath(path);
                    zzz.Main = path;
                    path = Args[2].Trim();
                    path = GetFullPath(path);
                    zzz.Other = path;
                }
                zzz.FolderMerge();
            }
            else if (Args.Count >= 2 && File.Exists(Args[0] = GetFullPath(Args[0])))
            {
                //merge
                zzz.Path_ = Args[0];
                for (int i = 1; i < Args.Count; i++)
                {
                    Args[i] = GetFullPath(Args[i]);
                    if (File.Exists(Args[i]) && !zzz.In.Contains(Args[i]))
                        zzz.In.Add(Args[i]);
                    else
                        Logger.WriteLine($"({Args[i]}) doesn't exist or is already added.\n");
                }
                if (zzz.In.Count > 0)
                    zzz.Merge();
            }
            else if (Args.Count == 2 && File.Exists(Args[0] = GetFullPath(Args[0])))
            {
                Args[1] = GetFullPath(Args[1]);
                Directory.CreateDirectory(Args[1]);
                if (Directory.Exists(Args[1]))
                {
                    zzz.In.Add(Args[0]);
                    zzz.Path_ = Args[1];
                    zzz.Extract();
                }
                else
                    Logger.WriteLine("Invalid Directory");
            }
            else if (Args.Count == 1 && Directory.Exists(Args[0] = GetFullPath(Args[0])))
            {
                zzz.Path_ = Args[0];
                zzz.Write();
            }
            else
            {
            start:
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
                else if ((k.Key == ConsoleKey.D4 || k.Key == ConsoleKey.NumPad4) &&
                    (Directory.EnumerateFiles(zzz.id1).Count() > 1 || Directory.EnumerateFiles(zzz.id2).Count() > 1))
                {
                    openfolder(zzz.FolderMerge());
                }
                else if (k.Key == ConsoleKey.T)
                {
                    TestMenu();
                }
                goto start;
            }
            void openfolder(string folder)
            {
                try
                {
                    folder = GetFullPath(folder);
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
                Logger.Write(
                    "            --- Welcome to the zzzDeArchive 0.1.7.4 ---\n" +
                    "     Code C# written by Sebanisu, Reversing and Python by Maki\n\n" +
                    "1) Extract - Extract zzz file\n" +
                    "2) Write - Write folder contents to a zzz file\n" +
                    "3) Merge - Write unique data from two or more zzz files into one zzz file.\n");
                if (Directory.EnumerateFiles(zzz.id1).Count() > 1 || Directory.EnumerateFiles(zzz.id2).Count() > 1)
                    Logger.Write("4) FolderMerge - Automaticly merge files in the IN subfolder. To the OUT folder\n");
                Logger.Write(
                    "\n" +
                    "Escape) Exit\n\n" +

                    "  Select: ");
                k = Console.ReadKey();
                Logger.WriteLine();
                if (k.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
            }
            while (k.Key != ConsoleKey.T && k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.D4 && k.Key != ConsoleKey.NumPad4 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2 && k.Key != ConsoleKey.D3 && k.Key != ConsoleKey.NumPad3);
            return k;
        }

        private static string MergeMenu()
        {
            string path;
            bool good = false;
            const string title = "\n     Merge zzz Screen\n";
            do
            {
                Logger.Write(
                    title +
                    "  Only unchanged data will be kept, rest will be replaced...\n" +
                    "Enter the path to zzz file with ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.Write("Original/OLD data");
                Console.ForegroundColor = ConsoleColor.White;
                Logger.Write(": ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                good = File.Exists(path);
                if (!good)
                    Logger.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            zzz.Path_ = path;
            do
            {
                if (zzz.In.Count == 0)
                {
                    Logger.Write(
                        "Enter the path to a zzz file with ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Logger.Write("NEW data");
                    Console.ForegroundColor = ConsoleColor.White;
                    Logger.Write(": ");
                }
                else
                {
                    Logger.Write($"Path to an additional zzz file or press enter to continue: ");
                }
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                if (string.IsNullOrWhiteSpace(path))
                {
                    if (zzz.In.Count > 0)
                        break;
                    else
                        Logger.WriteLine("Need atleast 1 file you entered an empty value.");
                }
                else
                {
                    path = GetFullPath(path);
                    good = File.Exists(path) && !zzz.In.Contains(path);
                    if (good)
                    {
                        zzz.In.Add(path);
                        Logger.WriteLine($"File added, {zzz.In.Count} total.");
                    }
                    else
                        Logger.WriteLine("File doesn't exist or is already added.\n");
                }
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
                Logger.Write(
                    "\n  Test Writes zzz Debug Screen\n" +
                    "Warning! this is a test screen\n" +
                    "This will keep making zzz files till it's done or errors\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                good = Directory.Exists(path);
                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            LoadSubDirs(path);
            void LoadSubDirs(string dir)
            {
                Logger.WriteLine($"Testing: {dir}\n");
                string[] subdirectoryEntries = Directory.GetDirectories(dir);
                zzz.Path_ = dir;
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
                Logger.Write(
                    "\n     Write zzz Screen\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                good = Directory.Exists(path);
                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);

            zzz.Path_ = path;
            return zzz.Write();
        }

        #endregion Methods

        #region Properties

        public static List<string> Args { get; private set; }

        #endregion Properties
    }
}