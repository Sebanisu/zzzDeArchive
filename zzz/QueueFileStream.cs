using System;
using System.IO;

namespace ZzzArchive
{
    public partial class Zzz
    {
        #region Classes

        /// <summary>
        /// Sepperate Read and Write locations. This is probably pointless. I was thinking of trying
        /// to make verification a thread or task. But Seems less like it'll be useful.
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/10352574/stream-that-has-separate-write-and-read-positions"/>
        private class QueueFileStream : FileStream
        {
            #region Fields

            private object locker = new object();
            private long ReadPosition;
            private long WritePosition;

            #endregion Fields

            #region Constructors

            public QueueFileStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
            {
            }

            #endregion Constructors

            #region Methods

            public override int Read(byte[] buffer, int offset, int count)
            {
                lock (locker)
                {
                    try
                    {
                        base.Seek(ReadPosition, SeekOrigin.Begin);
                        return base.Read(buffer, offset, count);
                    }
                    finally
                    {
                        ReadPosition = Position;
                    }
                }
            }

            [Obsolete("This is not supported in this class.", true)]
            public override long Seek(long offset, SeekOrigin origin) =>
                throw new Exception("Do not use Generic Seek. Use SeekRead or SeekWrite");

            public long SeekRead(long offset, SeekOrigin loc)
            {
                lock (locker)
                {
                    try
                    {
                        base.Seek(ReadPosition, SeekOrigin.Begin);
                        return base.Seek(offset, loc);
                    }
                    finally
                    {
                        ReadPosition = Position;
                    }
                }
            }

            public long SeekWrite(long offset, SeekOrigin loc)
            {
                lock (locker)
                {
                    try
                    {
                        base.Seek(WritePosition, SeekOrigin.Begin);
                        return base.Seek(offset, loc);
                    }
                    finally
                    {
                        WritePosition = Position;
                    }
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                lock (locker)
                {
                    base.Seek(WritePosition, SeekOrigin.Begin);

                    base.Write(buffer, offset, count);

                    WritePosition = Position;
                }
            }

            #endregion Methods
        }

        #endregion Classes
    }
}