using System.IO;
using System.Text;

namespace ZzzArchive
{
    /// <summary>
    /// Part of header that contains info on the files. </summary> <see cref="https://github.com/myst6re/qt-zzz/blob/master/zzztoc.h"/>
    public struct FileData
    {
        #region Fields

        public int FilenameLength;
        public long Offset;

        /// <summary>
        /// Needs to be uint to support 4gb files? Though probably over kill.
        /// </summary>
        public uint Size;

        private byte[] _fileNameBytes;

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
            get => Encoding.UTF8.GetString(_fileNameBytes); set
            {
                _fileNameBytes = Encoding.UTF8.GetBytes(value);
                FilenameLength = _fileNameBytes.Length;
            }
        }

        public int TotalBytes => sizeof(int) * 4 + FilenameLength;

        #endregion Properties

        #region Methods

        // readonly char[] invalid = Path.GetInvalidPathChars();
        public static FileData Read(BinaryReader br)
        {
            var r = new FileData
            {
                FilenameLength = br.ReadInt32()
            };
            r._fileNameBytes = br.ReadBytes(r.FilenameLength);
            //var tmp = r.Filename.Where(x => invalid.Contains(x));
            //if (tmp.Count() > 0)
            //    throw new InvalidDataException($"String ({r.Filename}) contains invalid characters! ({tmp})");
            r.Offset = br.ReadInt64(); // if we are reading more than this we are on future tech.
            r.Size = br.ReadUInt32();
            return r;
        }

        public static FileData Read(string oldPath, string newPath)
        {
            var safe = oldPath;
            safe = safe.Replace(newPath, "");
            safe = safe.Replace('/', '\\');
            safe = safe.Trim('\\');
            var fi = new FileInfo(oldPath);
            var r = new FileData
            {
                Filename = safe,
                Size = checked((uint)fi.Length) // zzz file only supports individual files of size uint.
            };
            return r;
        }

        public override string ToString() => $"({Filename}, {Offset}, {Size})";

        public void Write(BinaryWriter bw)
        {
            bw.Write(FilenameLength);
            bw.Write(_fileNameBytes);
            bw.Write(Offset);
            bw.Write(Size);
        }

        #endregion Methods
    }
}