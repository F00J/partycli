using System;
using System.IO;

namespace partycli.Logger
{
    internal static class AppLogger
    {
        internal static void Log(string action)
        {
            string logPath 
                = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "partycli",
                    "log.txt");

            Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action}{Environment.NewLine}";
            File.AppendAllText(logPath, logEntry);
        }
    }
}