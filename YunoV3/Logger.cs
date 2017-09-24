using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3
{
    public static class Logger
    {

        private static StreamWriter _streamWriter;
        private static object _consoleLock;
        private static object _streamWriterLock;

        public static void Initialize()
        {

            if (File.Exists("log.txt"))
                File.Delete("log.txt");

            _streamWriter = new StreamWriter("log.txt", true, Encoding.UTF8);
            _consoleLock = new object();
            _streamWriterLock = new object();

        }

        public static void Log(string message)
            => Log("Info", "Logger", message);

        public static void Log(string warningLvl, string source, string message, Exception exception = null)
        {

            //if i could use consolecolor in a single writeline statement, i wouldnt need this lock :/
            //its hardly neccessary, but wynaut
            lock (_consoleLock)
            {

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(DateTime.Now);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{warningLvl}]");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{source}] ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message);

                if (exception != null)
                    Console.WriteLine(exception.Message + exception.StackTrace);

                Console.ResetColor();

            }

            WriteToFile(message);

        }

        private static void WriteToFile(string message)
        {

            lock (_streamWriterLock)
            {

                _streamWriter.Write(message + "\n");
                _streamWriter.Flush();

            }

        }

    }
}
