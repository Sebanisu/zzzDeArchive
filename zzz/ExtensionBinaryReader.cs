using System.IO;

namespace ZzzArchive
{
    public static class ExtensionBinaryReader
    {
        #region Fields

        private const int bufferSize = 4096;

        #endregion Fields

        #region Methods

        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static byte[] ReadAllBytes(this Stream reader)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static long WriteAllBytes(this BinaryWriter writer, BinaryReader reader)
        {
            byte[] buffer = new byte[bufferSize];
            int count;
            long total = 0;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
                total += count;
                writer.Write(buffer, 0, count);
            }
            return total;
        }

        public static long WriteAllBytes(this Stream writer, Stream reader)
        {
            byte[] buffer = new byte[bufferSize];
            int count;
            long total = 0;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
                total += count;
                writer.Write(buffer, 0, count);
            }
            return total;
        }

        #endregion Methods
    }
}