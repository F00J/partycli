using Newtonsoft.Json;
using partycli.Models;
using partycli.Storage;
using System;
using System.Collections.Generic;

namespace partycli.Logger
{
    internal static class AppLogger
    {
        internal static void Log(string action)
        {
            LogModel newLog = new LogModel
            {
                Action = action,
                Time = DateTime.Now
            };

            List<LogModel> currentLog;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.log))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(Properties.Settings.Default.log);
                currentLog.Add(newLog);
            }
            else
            {
                currentLog = new List<LogModel> { newLog };
            }

            ConfigurationStorage.StoreValue("log", JsonConvert.SerializeObject(currentLog), false);
        }
    }
}
