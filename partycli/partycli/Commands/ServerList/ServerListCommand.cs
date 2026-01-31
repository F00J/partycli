using Newtonsoft.Json;
using partycli.Clients.NordVpnClient;
using partycli.Clients.NordVpnClient.Enums;
using partycli.Enums;
using partycli.Logger;
using partycli.Models;
using partycli.Services;
using partycli.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace partycli.Commands.ServerList
{
    internal class ServerListCommand : BaseCommand
    {
        internal override CLICommand CLICommand => CLICommand.ServerList;

        public ServerListCommand(NordVpnClient nordVpnClient)
            : base(nordVpnClient)
        { 
        }

        internal override async Task ExecuteAsync(string[] args)
        {
            IEnumerable<ServerDto> servers;

            if (args == null || args.Length == 0)
            {
                servers = await FetchAllServersAsync();
            }
            else
            {
                servers = await FetchServersByArgumentAsync(args[0]);

                if (servers == null)
                {
                    Console.WriteLine($"Unknown option: {args[0]}");
                    return;
                }
            }

            if (servers == null || !servers.Any())
            {
                AppLogger.Log($"No servers retrieved for {args[0]}.");
                Console.WriteLine("No servers found.");
                return;
            }

            SaveAndDisplayServers(servers, args[0]);

            await Task.CompletedTask;
        }

        private async Task<IEnumerable<ServerDto>> FetchAllServersAsync()
        {
            // TODO: Check if local cache is available and valid if it is return from storage
            return await NordVpnClient.GetAllServersAsync();
        }

        private async Task<IEnumerable<ServerDto>> FetchServersByArgumentAsync(string arg)
        {
            switch (arg.ToLower())
            {
                case "--local":
                    return GetLocalServers();
                case "--france":
                    return await NordVpnClient.GetServersByCountryAsync((int)Country.France);
                case "--tcp":
                    return await NordVpnClient.GetServersByProtocolAsync(VpnProtocol.TCP);

                default:
                    return null;
            }
        }

        private IEnumerable<ServerDto> GetLocalServers()
        {
            string cachedData = Properties.Settings.Default.serverlist;

            if (string.IsNullOrEmpty(cachedData))
            {
                Console.WriteLine("Error: No local server data available.");
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<List<ServerDto>>(cachedData);
            }
            catch (JsonException ex)
            {
                AppLogger.Log($"Error deserializing cached servers: {ex.Message}");
                return null;
            }
        }

        private void SaveAndDisplayServers(IEnumerable<ServerDto> servers, string description)
        {
            string serverList = JsonConvert.SerializeObject(servers.ToList());

            ConfigurationStorage.StoreValue("serverlist", serverList, false);
            AppLogger.Log($"Saved {description}: {servers.Count()} servers");

            DisplayService.DisplayList(servers.ToList());
        }
    }
}