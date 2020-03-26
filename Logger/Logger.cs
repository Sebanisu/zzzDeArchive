using System;
using System.IO;
using System.Threading;

namespace ZzzArchive
{
    public class Logger : IDisposable
    {
        #region Fields

        private static readonly Logger Self = new Logger();
        private static ReaderWriterLock _locker;
        private FileStream _fs;
        private StreamWriter _sw;

        #endregion Fields

        #region Constructors

        public Logger()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
            if (_locker == null) _locker = new ReaderWriterLock();
            try
            {
                _locker.AcquireWriterLock(int.MaxValue);
                try
                {
                    _fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    _sw = new StreamWriter(_fs);
                }
                finally
                {
                    _locker.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
            }
        }

        #endregion Constructors

        #region Destructors

        ~Logger()
        {
            Dispose();
        }

        #endregion Destructors

        #region Methods

        public static void DisposeChildren() => Self?.Dispose();

        public static string Write(string @in, bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.Write(@in);
            if (!skipLog)
            {
                try
                {
                    _locker.AcquireWriterLock(int.MaxValue);
                    try
                    {
                        Self?._sw.Write(@in);
                    }
                    finally
                    {
                        _locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException)
                {
                    // The writer lock request timed out.
                }
            }
            return @in;
        }

        public static string WriteLine(string @in = "", bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.WriteLine(@in);
            if (skipLog) return @in;
            try
            {
                _locker.AcquireWriterLock(int.MaxValue);
                try
                {
                    Self?._sw.WriteLine(@in);
                }
                finally
                {
                    _locker.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
            }
            return @in;
        }

        public static string WriteLineThrow(string @in = "", bool skipConsole = false, bool skipLog = false) => throw new Exception(WriteLine(@in, skipConsole, skipLog));

        public void Dispose()
        {
            if (_sw == null || _fs == null) return;
            try
            {
                _locker.AcquireWriterLock(int.MaxValue);
                try
                {
                    _sw.Close();
                }
                finally
                {
                    _locker.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
            }
            finally
            {
                _sw = null;
                _fs = null;
            }
        }

        #endregion Methods
    }
}