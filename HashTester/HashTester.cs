using System.IO;
using System.Security.Cryptography;

namespace ZzzArchive
{
    public class HashTester
    {
        #region Fields

        private static readonly HashAlgorithm Hash = new SHA1CryptoServiceProvider();

        #endregion Fields

        /*
                public static byte[] GetHashClose(Stream stream, ulong size)
                {
                    try
                    {
                        return GetHash(stream, size);
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
        */

        #region Methods

        public static byte[] GetHash(Stream stream, ulong size)
        {
            var br = new BinaryReader(stream);
            using (var ms = new MemoryStream())
            using (var tmp = new BinaryWriter(ms))
            {
                Read(tmp, br, size);
                return GetHash(ms);
            }
        }

        public static byte[] GetHash(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return Hash.ComputeHash(stream);
        }

        public static byte[] GetHashClose(Stream stream)
        {
            try
            {
                return GetHash(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        private static void Read(BinaryWriter bw, BinaryReader br, ulong size)
        {
            while (size.CompareTo(0) > 0)
            {
                var s = (size.CompareTo(int.MaxValue) > 0) ? int.MaxValue : (int)size;
                bw.Write(br.ReadBytes(s));
                size -= (ulong)s;
            }
        }

        #endregion Methods
    }
}