using System;
using System.IO;

namespace ZzzArchive
{
    /// <summary>
    /// </summary>
    /// <see cref="https://social.msdn.microsoft.com/Forums/vstudio/en-US/c409b63b-37df-40ca-9322-458ffe06ea48/how-to-access-part-of-a-filestream-or-memorystream?forum=netfxbcl"/>
    public class SubStream : Stream
    {
        #region Fields

        private Stream _baseStream;
        private long _length;
        private long _offset;

        #endregion Fields

        #region Methods

        private void CheckDisposed()
        {
            if (_baseStream == null) throw new ObjectDisposedException(GetType().Name);
        }

        private void ConnotReachPoint() => throw new Exception("Cannot reach offset");

        private long GetRemaining()
        {
            long remaining = _baseStream.Length - _baseStream.Position;
            long remaining2 = _length - Position;
            if (remaining2 < remaining) remaining = remaining2;
            return remaining;
        }

        private void ReadToPoint(ref long offset)
        {
            long remaining = GetRemaining();
            //read to point;
            const int BUFFER_SIZE = 512;
            byte[] buffer = new byte[BUFFER_SIZE];
            if (offset > remaining)
                offset = remaining;
            while (offset > 0)
            {
                int read = _baseStream.Read(buffer, 0, offset < BUFFER_SIZE ? checked((int)offset) : BUFFER_SIZE);
                offset -= read;
            }
        }

        #endregion Methods

        #region Constructors

        public SubStream(Stream baseStream, long offset, long length, SeekOrigin seekOrigin = SeekOrigin.Begin)
        {
            if (_baseStream == null) throw new ArgumentNullException("baseStream");
            if (!CanRead) throw new ArgumentException("baseStream cannot be read");
            if (baseStream.Position + offset < 0) throw new ArgumentOutOfRangeException("offset is < 0");

            _baseStream = baseStream;
            _length = length;
            if (CanSeek)
                _baseStream.Seek(_offset, seekOrigin);
            else if (seekOrigin == SeekOrigin.Current)
            {
                ReadToPoint();
            }
            else if (seekOrigin == SeekOrigin.Begin)
            {                //read to point;
                if (_baseStream.Position <= offset)
                    offset -= _baseStream.Position;
                else
                    ConnotReachPoint();
                ReadToPoint();
            }
            else
            {
                ConnotReachPoint();
            }

            void ReadToPoint()
            {
                //read to point;
                const int BUFFER_SIZE = 512;
                byte[] buffer = new byte[BUFFER_SIZE];
                while (offset > 0)
                {
                    int read = baseStream.Read(buffer, 0, offset < BUFFER_SIZE ? checked((int)offset) : BUFFER_SIZE);
                    offset -= read;
                }
            }
            _offset = _baseStream.Position;
        }

        #endregion Constructors

        #region Properties

        public override bool CanRead
        {
            get
            {
                CheckDisposed();
                return _baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                CheckDisposed();
                return _baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                CheckDisposed();
                return _baseStream.CanWrite;
            }
        }

        public override long Length => _length;
        public override long Position { get => _baseStream.Position - _offset; set => _baseStream.Position = _offset + value; }

        #endregion Properties

        public override void Flush()
        {
            CheckDisposed(); _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            long remaining = GetRemaining();
            if (remaining <= 0) return 0;
            if (remaining < count) count = (int)remaining;
            int read = _baseStream.Read(buffer, offset, count);
            return read;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:

                    if (CanSeek)
                        _baseStream.Seek(_offset + offset, origin);
                    else if (Position <= offset)
                    {
                        offset -= Position;
                        ReadToPoint(ref offset);
                    }
                    else ConnotReachPoint();
                    break;

                case SeekOrigin.Current:
                    if (CanSeek)
                        _baseStream.Seek(offset, origin);
                    else if (Position <= offset)
                    {
                        offset -= Position;
                        ReadToPoint(ref offset);
                    }
                    else ConnotReachPoint();
                    break;

                case SeekOrigin.End:
                default:
                    ConnotReachPoint();
                    break;
            }
            return Position;
        }
        public override void SetLength(long value) =>
        throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            long remaining = GetRemaining();
            if (remaining <= 0) return;
            if (remaining < count) count = (int)remaining;
            _baseStream.Write(buffer, offset, count);
        }
    }
}