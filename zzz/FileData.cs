using System.IO;
using System.Text;

namespace ZzzArchive
{
    /// <summary>
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
}