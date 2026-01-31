using System;
using System.Diagnostics;
using System.IO;

namespace partycli.Logger
{
    internal static class AppLogger
    {
        internal static void Log(string action)
        {
            try
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
            catch (Exception exception)
            {
                EventLog.WriteEntry("Application", "Failed to write to log file: " + exception.Message, EventLogEntryType.Error);
            }
        }
    }
}