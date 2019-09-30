using System;
using System.IO;
using System.Threading;

namespace ZzzArchive
{
    public class Logger : IDisposable
    {
        #region Fields

        private static Logger self = new Logger();
        private FileStream fs;
        private StreamWriter sw;
        private static ReaderWriterLock locker;

        #endregion Fields

        #region Destructors

        ~Logger()
        {
            Dispose();
        }

        #endregion Destructors

        #region Constructors

        public Logger()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
            if(locker == null) locker = new ReaderWriterLock();
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                try
                {
                    fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs);
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
            }
        }

        #endregion Constructors

        #region Methods

        public static string Write(string @in, bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.Write(@in);
            if (!skipLog)
            {
                try
                {
                    locker.AcquireWriterLock(int.MaxValue);
                    try
                    {
                        self?.sw.Write(@in);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
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
            if (!skipLog)
            {
                try
                {
                    locker.AcquireWriterLock(int.MaxValue);
                    try
                    {
                        self?.sw.WriteLine(@in);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException)
                {
                    // The writer lock request timed out.
                }
            }
            return @in;
        }

        public static void DisposeChildren()
        {
            self?.Dispose();
            self = null;
        }

        public void Dispose()
        {
            if (sw != null && fs != null)
            {
                try
                {
                    locker.AcquireWriterLock(int.MaxValue);
                    try
                    {
                        sw.Close();
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException)
                {
                    // The writer lock request timed out.
                }
                finally
                {
                    sw = null;
                    fs = null;
                }
            }
        }

        #endregion Methods
    }
}