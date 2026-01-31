using System;

namespace partycli.Storage
{
    internal static class ConfigurationStorage
    {
        internal static void StoreValue(string name, string value, bool writeToConsole = true)
        {
            try
            {
                Properties.Settings settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole)
                {
                    Console.WriteLine("Changed " + name + " to " + value);
                }
            }
            catch
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }
        }
    }
}