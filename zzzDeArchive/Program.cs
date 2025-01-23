using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;

namespace ZzzArchive
{
    public class Program
    {
        #region Fields

        private static readonly Zzz ZZZ = new Zzz();

        #endregion Fields

        #region Properties

        public static List<string> Args { get; private set; }

        #endregion Properties

        #region Methods

        private static string ExtractMenu()
        {
        StartExtractMenu:
            string path;
            bool good;
            const string title = "\n     Extract zzz Screen\n";
            do
            {
                good = false;
                Logger.Write(
                    title +
                    "Enter the path to zzz file: ");
                path = Console.ReadLine();
                if (path != null)
                {
                    path = path.Trim('"');
                    path = path.Trim();
                    Logger.WriteLine();
                    path = GetFullPath(path);
                    good = File.Exists(path);
                }

                if (!good)
                    Logger.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            ZZZ.In = new List<string> { path };
            do
            {
                good = false;
                Logger.Write(
                    title +
                    "Enter the path to extract contents: ");
                path = Console.ReadLine();
                if (path != null)
                {
                    path = path.Trim('"');
                    path = path.Trim();
                    path = GetFullPath(path);
                    Logger.WriteLine();
                    Directory.CreateDirectory(path);
                    good = Directory.Exists(path);
                }

                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            ZZZ.Path = path;
            try
            {
                return ZZZ.Extract();
            }
            catch (PathTooLongException)
            {
                goto StartExtractMenu;
            }
            catch (InvalidDataException)
            {
                goto StartExtractMenu;
            }
        }

        private static string GetFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path);
            }
            catch (Exception e)
            {
                Logger.WriteLine($"{e.Message}");
                Logger.WriteLine($"path: {path}");
                return null;
            }
        }

        [SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
        private static void Main(string[] args)
        {
            Args = args.Select(x => x.Trim('"').Trim()).ToList();

            var ind = Args.FindIndex(x => x.Equals("-skipWarning", StringComparison.OrdinalIgnoreCase));
            if (ind >= 0)
            {
                ZZZ.SkipWarning = true;
                Args.RemoveAt(ind);
            }
            var zzzArgs = Args.Where(arg => arg.EndsWith(".ZZZ", StringComparison.OrdinalIgnoreCase)).ToList();
            if ((Args.Count == 1 || Args.Count == 3) &&
                (Args[0].Equals("-folderMerge", StringComparison.OrdinalIgnoreCase) ||
                Args[0].Equals("-mergeFolder", StringComparison.OrdinalIgnoreCase)) &&
                (Directory.EnumerateFiles(ZZZ.ID1).Count() > 1 ||
                Directory.EnumerateFiles(ZZZ.ID2).Count() > 1 ||
                Directory.EnumerateDirectories(ZZZ.ID1).Count() > 1 ||
                Directory.EnumerateDirectories(ZZZ.ID2).Count() > 1))
            {
                if (Args.Count == 3)
                {
                    var path = Args[1].Trim();
                    path = GetFullPath(path);
                    ZZZ.Main = path;
                    path = Args[2].Trim();
                    path = GetFullPath(path);
                    ZZZ.Other = path;
                }
                try
                {
                    ZZZ.FolderMerge();
                }
                catch (PathTooLongException)
                {
                }
                catch (InvalidDataException)
                {
                }
                catch (ArgumentException)
                {
                }
            }
            else if (zzzArgs.Count >= 2 && File.Exists(zzzArgs[0] = GetFullPath(zzzArgs[0])))
            {
                //merge
                ZZZ.Path = Args[0];
                ZZZ.In = new List<string>();
                for (var i = 1; i < Args.Count; i++)
                {
                    Args[i] = GetFullPath(Args[i]);
                    if (File.Exists(zzzArgs[i]) && !ZZZ.In.Contains(zzzArgs[i]))
                        ZZZ.In.Add(zzzArgs[i]);
                    else
                        Logger.WriteLine($"({Args[i]}) doesn't exist or is already added.\n");
                }
                try
                {
                    if (ZZZ.In.Count > 0)
                        ZZZ.Merge();
                }
                catch (PathTooLongException)
                {
                }
                catch (InvalidDataException)
                {
                }
                catch (ArgumentException)
                {
                }
            }
            else if (Args.Count == 2 && File.Exists(Args[0] = GetFullPath(Args[0])) 
                && Args[0].EndsWith(".ZZZ", StringComparison.OrdinalIgnoreCase) 
                && !Args[1].EndsWith(".ZZZ", StringComparison.OrdinalIgnoreCase))
            { //extract
                Args[1] = GetFullPath(Args[1]);
                Directory.CreateDirectory(Args[1]);
                ZZZ.In = new List<string>();
                if (Directory.Exists(Args[1]))
                {
                    ZZZ.In.Add(Args[0]);
                    ZZZ.Path = Args[1];
                    try
                    {
                        ZZZ.Extract();
                    }
                    catch (PathTooLongException)
                    {
                    }
                    catch (InvalidDataException)
                    {
                    }
                }
                else
                    Logger.WriteLine("Invalid Directory");
            }
            else if (Args.Count == 1 && Directory.Exists(Args[0] = GetFullPath(Args[0])))
            {
                ZZZ.Path = Args[0];
                try
                {
                    ZZZ.Write();
                }
                catch (PathTooLongException)
                {
                }
                catch (InvalidDataException)
                {
                }
            }
            else
            {
                do
                {
                    var k = MainMenu();
                    switch (k.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            OpenFolder(ExtractMenu());
                            break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            OpenFolder(WriteMenu());
                            break;

                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            OpenFolder(MergeMenu());
                            break;

                        default:
                            {
                                if ((k.Key == ConsoleKey.D4 || k.Key == ConsoleKey.NumPad4) &&
                                    (Directory.EnumerateFiles(ZZZ.ID1).Count() > 1 ||
                                     Directory.EnumerateFiles(ZZZ.ID2).Count() > 1))
                                {
                                    try
                                    {
                                        OpenFolder(ZZZ.FolderMerge());
                                    }
                                    catch (PathTooLongException)
                                    {
                                    }
                                    catch (InvalidDataException)
                                    {
                                    }
                                    catch (ArgumentException)
                                    {
                                    }
                                }
                                else if (k.Key == ConsoleKey.T)
                                {
                                    TestMenu();
                                }

                                break;
                            }
                    }
                } while (true);
            }
            void OpenFolder(string folder)
            {
                try
                {
                    folder = GetFullPath(folder);
                    if (Directory.Exists(folder))
                        Process.Start(folder);
                }
                catch
                {
                    // ignored
                }
            }
            Logger.DisposeChildren();
        }

        private static ConsoleKeyInfo MainMenu()
        {
            ConsoleKeyInfo k;
            do
            {
                Logger.Write(
                    "            --- Welcome to the zzzDeArchive 0.1.7.6 ---\n" +
                    "     Code C# written by Sebanisu, Reversing and Python by Maki\n\n" +
                    "1) Extract - Extract zzz file\n" +
                    "2) Write - Write folder contents to a zzz file\n" +
                    "3) Merge - Write unique data from two or more zzz files into one zzz file.\n");
                if (Directory.EnumerateFiles(ZZZ.ID1).Count() > 1 || Directory.EnumerateFiles(ZZZ.ID2).Count() > 1)
                    Logger.Write("4) FolderMerge - Automatically merge files in the IN subfolder. To the OUT folder\n");
                Logger.Write(
                    "\n" +
                    "Escape) Exit\n\n" +

                    "  Select: ");
                k = Console.ReadKey();
                Logger.WriteLine();
                if (k.Key == ConsoleKey.Escape)
                {
                    Logger.DisposeChildren();
                    Environment.Exit(0);
                }
            }
            while (k.Key != ConsoleKey.T && k.Key != ConsoleKey.D1 && k.Key != ConsoleKey.D2 && k.Key != ConsoleKey.D4 && k.Key != ConsoleKey.NumPad4 && k.Key != ConsoleKey.NumPad1 && k.Key != ConsoleKey.NumPad2 && k.Key != ConsoleKey.D3 && k.Key != ConsoleKey.NumPad3);
            return k;
        }

        private static string MergeMenu()
        {
        StartMergeMenu:
            string path;
            bool good;
            const string title = "\n     Merge zzz Screen\n";
            do
            {
                good = false;
                Logger.Write(
                    title +
                    "  Only unchanged data will be kept, rest will be replaced...\n" +
                    "Enter the path to zzz file with ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.Write("Original/OLD data");
                Console.ForegroundColor = ConsoleColor.White;
                Logger.Write(": ");
                path = Console.ReadLine();
                if (path != null)
                {
                    path = path.Trim('"');
                    path = path.Trim();
                    Logger.WriteLine();
                    path = GetFullPath(path);
                    good = File.Exists(path);
                }

                if (!good)
                    Logger.WriteLine("File doesn't exist\n");
                else break;
            }
            while (true);

            ZZZ.Path = path;
            ZZZ.In = new List<string>();
            do
            {
                if (ZZZ.In.Count == 0)
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
                    Logger.Write("Path to an additional zzz file or press enter to continue: ");
                }
                path = Console.ReadLine();
                if (path == null) continue;
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                if (string.IsNullOrWhiteSpace(path))
                {
                    if (ZZZ.In.Count > 0)
                        break;
                    Logger.WriteLine("Need at least 1 file you entered an empty value.");
                }
                else
                {
                    path = GetFullPath(path);
                    ZZZ.In = new List<string>();
                    good = File.Exists(path) && !ZZZ.In.Contains(path);
                    if (good)
                    {
                        ZZZ.In.Add(path);
                        Logger.WriteLine($"File added, {ZZZ.In.Count} total.");
                    }
                    else
                        Logger.WriteLine("File doesn't exist or is already added.\n");
                }
            }
            while (true);
            try
            {
                return ZZZ.Merge();
            }
            catch (PathTooLongException)
            {
                goto StartMergeMenu;
            }
            catch (InvalidDataException)
            {
                goto StartMergeMenu;
            }
            catch (ArgumentException)
            {
                goto StartMergeMenu;
            }
        }

        private static void TestMenu()
        {
            string path;
            do
            {
                Logger.Write(
                    "\n  Test Writes zzz Debug Screen\n" +
                    "Warning! this is a test screen\n" +
                    "This will keep making zzz files till it's done or errors\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                if (path == null) continue;
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                var good = Directory.Exists(path);
                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);
            LoadSubDirs(path);
            void LoadSubDirs(string dir)
            {
                Logger.WriteLine($"Testing: {dir}\n");
                var subDirectoryEntries = Directory.GetDirectories(dir);
                ZZZ.Path = dir;
                try
                {
                    ZZZ.Write();
                }
                catch (PathTooLongException)
                {
                }
                catch (InvalidDataException)
                {
                }
                foreach (var subDirectory in subDirectoryEntries)
                    LoadSubDirs(subDirectory);
            }
        }

        //private const string zzz.Path = @"D:\ext";
        private static string WriteMenu()
        {
        BeginWriteMenu:
            string path;
            do
            {
                Logger.Write(
                    "\n     Write zzz Screen\n" +
                    "Enter the path of files to go into out.zzz: ");
                path = Console.ReadLine();
                if (path == null) continue;
                path = path.Trim('"');
                path = path.Trim();
                Logger.WriteLine();
                path = GetFullPath(path);
                var good = Directory.Exists(path);
                if (!good)
                    Logger.WriteLine("Directory doesn't exist\n");
                else break;
            }
            while (true);

            ZZZ.Path = path;
            try
            {
                return ZZZ.Write();
            }
            catch (PathTooLongException)
            {
                goto BeginWriteMenu;
            }
            catch (InvalidDataException)
            {
                goto BeginWriteMenu;
            }
        }

        #endregion Methods
    }
}