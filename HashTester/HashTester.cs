using System.IO;
using System.Security.Cryptography;

namespace ZzzArchive
{
    public class HashTester
    {
        #region Fields

        private static HashAlgorithm sha = new SHA1CryptoServiceProvider();

        #endregion Fields

        #region Methods
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
        public static byte[] GetHash(Stream stream, ulong size)
        {
            BinaryReader br = new BinaryReader(stream);
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter tmp = new BinaryWriter(ms))
            {
                Read(tmp, br, size);
                return GetHash(ms);
            }
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
        public static byte[] GetHash(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return sha.ComputeHash(stream);
        }

        private static void Read(BinaryWriter bw, BinaryReader br, ulong size)
        {
            while (size.CompareTo(0) > 0)
            {
                int s = (size.CompareTo(int.MaxValue) > 0) ? int.MaxValue : (int)size;
                bw.Write(br.ReadBytes(s));
                size -= (ulong)s;
            }
        }

        #endregion Methods
    }
}