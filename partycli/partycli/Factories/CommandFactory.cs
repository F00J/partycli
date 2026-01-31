using partycli.Clients.NordVpnClient;
using partycli.Commands;
using partycli.Commands.Config;
using partycli.Commands.ServerList;
using partycli.Enums;
using System;
using System.Collections.Generic;

namespace partycli.Factories
{
    internal class CommandFactory
    {
        private readonly Dictionary<string, Func<BaseCommand>> _commandMap;
        private readonly Dictionary<string, CLICommand> _stringToEnum;

        public CommandFactory()
        {
            NordVpnClient nordVpnClient = new NordVpnClient();

            _commandMap = new Dictionary<string, Func<BaseCommand>>(StringComparer.OrdinalIgnoreCase)
            {
                { "server_list", () => new ServerListCommand(nordVpnClient) },
                { "config", () => new ConfigCommand(nordVpnClient) }
            };

            _stringToEnum = new Dictionary<string, CLICommand>(StringComparer.OrdinalIgnoreCase)
            {
                { "server_list", CLICommand.ServerList },
                { "config", CLICommand.Config }
            };
        }

        public BaseCommand GetCommand(string commandName)
        {
            return _commandMap.TryGetValue(commandName, out Func<BaseCommand> factory)
                ? factory()
                : null;
        }

        public CLICommand? ParseCommandType(string commandString)
        {
            return _stringToEnum.TryGetValue(commandString, out CLICommand commandType)
                ? commandType
                : (CLICommand?)null;
        }

        public IEnumerable<string> GetAvailableCommands()
        {
            return _commandMap.Keys;
        }
    }
}
