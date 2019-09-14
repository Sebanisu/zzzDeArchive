using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zzz
{ /// <summary>
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
            r.Offset = br.ReadInt64();
            r.Size = br.ReadInt32();
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

        #endregion Fields

        #region Properties

        public long ExpectedFileSize => Data.Last().Size + Data.Last().Offset;

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
        public static  ZzzHeader Merge(ZzzHeader @in, ref ZzzHeader @out)
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
        public static ZzzHeader Read(string path, out string[] files, string _path)
        {
            ZzzHeader r = new ZzzHeader();
            files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            r.Count = files.Length;
            r.Data = new FileData[r.Count];

            for (int i = 0; i < r.Count; i++)
                r.Data[i] = FileData.Read(files[i], _path);

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
    public class Zzz
    {
        private string _path;

       
    }
}