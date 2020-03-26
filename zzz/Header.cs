using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZzzArchive
{
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

        public long ExpectedFileSize => (Data?.Last().Size ?? 0) + (Data?.Last().Offset ?? 0);

        public int MaxPathLength => Data.Max(x => x.FilenameLength);

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

        #region Methods

        public static Header Merge(ref Header @out, ref Header[] @in, bool skipWarning = false)
        {
            Logger.WriteLine("Merging Headers");
            var r = new Header();
            EliminateDuplicates(ref @out, @in, skipWarning);

            for (var i = 0; i < @in.Length; i++)
                Merge(ref @out, ref @in[i], ref r);

            return CorrectOffsets(ref r);
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
            var data = r.Data != null ? new List<FileData>(r.Data) : new List<FileData>();
            if (r.Count == 0)
                data.AddRange(@out.Data);
            data.AddRange(@in.Data);
            r.Count = data.Count;
            r.Data = data.ToArray();
            return r;
        }

        public static Header Read(BinaryReader br)
        {
            var r = new Header
            {
                Count = br.ReadInt32()
            };
            r.Data = new FileData[r.Count];
            for (var i = 0; i < r.Count; i++)
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
            var r = new Header();
            files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            r.Count = files.Length;
            r.Data = new FileData[r.Count];

            for (var i = 0; i < r.Count; i++)
                r.Data[i] = FileData.Read(files[i], _path);

            long pos = r.TotalBytes;

            //cannot know the size of the header till i had loaded the rest of the data.
            //so now we are updating the offset to be past the header. in the same order as the files.
            for (var i = 0; i < r.Count; i++)
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
                foreach (var r in Data)
                {
                    Logger.WriteLine($"Writing FileData {r}");
                    r.Write(bw);
                }
            Logger.WriteLine($"Header data written {TotalBytes} bytes");
        }

        private static Header CorrectOffsets(ref Header r)
        {
            long offset = r.TotalBytes;
            for (var i = 0; i < r.Count; i++)
            {
                r.Data[i].Offset = offset;
                offset += r.Data[i].Size;
            }

            return r;
        }

        private static void EliminateDuplicates(ref Header @out, Header[] @in, bool skipWarning = false)
        {
            Logger.WriteLine("Eliminating Duplicates for input zzz files from other input zzz files...");
            for (var i = 0; i < @in.Length; i++)
                for (var j = i + 1; j < @in.Length; j++)
                    EliminateDuplicates(ref @in[i], @in[j], true);

            Logger.WriteLine("Eliminating Duplicates for input zzz files from the original zzz file...");
            foreach (var headerData in @in)
                EliminateDuplicates(ref @out, headerData, skipWarning);
        }

        private static void EliminateDuplicates(ref Header @out, Header @in, bool skipWarning)
        {
            var out2 = new List<FileData>(@out.Data);
            var in2 = new List<FileData>();
            // grab the files that are unique to @out. Replacing that bit of the header
            //Logger.WriteLine("Eliminating Duplicates...");
            //for (int i = 0; i < out2.Count; i++)
            //{
            //    if (@in.Data.Any(x => x.Filename.Equals(out2[i].Filename, StringComparison.OrdinalIgnoreCase)))
            //        out2.RemoveAt(i--);
            //}
            for (var i = 0; i < @in.Count; i++)
            {
                int ind;
                var fn = @in.Data[i].Filename;
                if ((ind = out2.FindIndex(x => x.Filename.Equals(fn, StringComparison.OrdinalIgnoreCase))) > -1)
                    out2.RemoveAt(ind);
                else
                    in2.Add(@in.Data[i]);
            }
            if (@out.Count - out2.Count > 0)
                Logger.WriteLine($"Eliminated {@out.Count - out2.Count}");
            if (@out.Count - out2.Count < @in.Count && !skipWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.WriteLine($"WARNING you are not replacing all {@in.Count} files. \n" +
                    $"There are going to be {Math.Abs(@out.Count - out2.Count - @in.Count)} files added!\n" +
                    "The game may ignore any new files it is not expecting...");
                Console.ForegroundColor = ConsoleColor.White;
                Logger.WriteLine("\nPress any key to continue...");
                try
                {
                    Console.ReadKey();
                }
                catch
                {
                    // ignored
                }

                Logger.WriteLine("-- List of new files --");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var i in in2)
                {
                    Logger.WriteLine(i.ToString());
                }
                Console.ForegroundColor = ConsoleColor.White;
                Logger.WriteLine("\nPress any key to continue...");
                try
                {
                    Console.ReadKey();
                }
                catch
                {
                    // ignored
                }
            }
            @out.Count = out2.Count;
            @out.Data = out2.ToArray();
        }

        #endregion Methods

        /*
                public static Header Merge(ref Header @out, ref Header @in)
                {
                    var r = new Header();
                    return Merge(ref @out, ref @in, ref r);
                }
        */
    }
}