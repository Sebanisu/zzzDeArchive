using System;
using System.IO;

namespace _Logger
{
    public class Logger : IDisposable
    {
        private static Logger self = new Logger();
        private FileStream fs;
        private StreamWriter sw;

        public Logger()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
            fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sw = new StreamWriter(fs);
        }

        public static string Write(string @in, bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.Write(@in);
            if (!skipLog)
                self.sw.Write(@in);
            return @in;
        }

        public static string WriteLine(string @in="", bool skipConsole = false, bool skipLog = false)
        {
            if (!skipConsole)
                Console.WriteLine(@in);
            if (!skipLog)
                self.sw.WriteLine(@in);
            return @in;
        }

        ~Logger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (sw != null && fs != null)
            {
                sw.Close();
                sw = null;
                fs = null;
            }
        }
    }
}