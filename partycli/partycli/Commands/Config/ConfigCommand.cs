using partycli.Clients.NordVpnClient;
using partycli.Enums;
using partycli.Logger;
using partycli.Storage;
using System;
using System.Threading.Tasks;

namespace partycli.Commands.Config
{
    internal class ConfigCommand : BaseCommand
    {
        internal override CLICommand CLICommand => CLICommand.Config;

        public ConfigCommand(NordVpnClient nordVpnClient) 
            : base(nordVpnClient)
        {    
        }

        internal override async Task ExecuteAsync(string[] args)
        {
            string name = args[0];
            string value = args[1];

            ConfigurationStorage.StoreValue(name, value);
            AppLogger.Log("Changed " + name + " to " + value);

            await Task.CompletedTask;
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
