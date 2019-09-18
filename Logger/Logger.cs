using System;
using System.IO;

namespace _Logger
{
    public class Logger : IDisposable
    {
        #region Fields

        private static Logger self = new Logger();
        private FileStream fs;
        private StreamWriter sw;

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
            fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sw = new StreamWriter(fs);
        }

        #endregion Constructors

        #region Methods

        public static string Write(string @in, bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.Write(@in);
            if (!skipLog)
                self.sw.Write(@in);
            return @in;
        }

        public static string WriteLine(string @in = "", bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.WriteLine(@in);
            if (!skipLog)
                self.sw.WriteLine(@in);
            return @in;
        }

        public void Dispose()
        {
            if (sw != null && fs != null)
            {
                try
                {
                    sw.Close();
                }
                catch
                {
                }
                sw = null;
                fs = null;
            }
        }

        #endregion Methods
    }
}