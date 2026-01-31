using partycli.Enums;
using partycli.Logger;
using partycli.Storage;
using System;

namespace partycli.Commands.Config
{
    internal class ConfigCommand : BaseCommand
    {
        internal override CLICommand CLICommand => CLICommand.Config;

        internal override void Execute(string[] args)
        {
            string name = args[0];
            string value = args[1];

            ConfigurationStorage.StoreValue(name, value);
            AppLogger.Log("Changed " + name + " to " + value);
        }

        internal override bool ValidateArgs(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Error: No arguments provided for config command.");
                return false;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Error: Invalid number of arguments for config command.");
                return false;
            }

            return true;
        }
    }
}
