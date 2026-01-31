using partycli.Enums;
using partycli.Logger;
using partycli.Models;
using partycli.Services;
using partycli.Storage;
using System;

namespace partycli.Commands.ServerList
{
    internal class ServerListCommand : BaseCommand
    {
        internal override CLICommand CLICommand => CLICommand.ServerList;

        internal override void Execute(string[] args)
        {
            string serverList = NordVpnClient.GetAllServersListAsync();
            ConfigurationStorage.StoreValue("serverlist", serverList, false);
            AppLogger.Log("Saved new server list: " + serverList);
            DisplayService.DisplayList(serverList);

            if (args == null || args.Length == 0)
            {
                return;
            }

            string arg = args[0];

            switch (arg)
            {
                case "--local":
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.serverlist))
                    {
                        DisplayService.DisplayList(Properties.Settings.Default.serverlist);
                    }
                    else
                    {
                        Console.WriteLine("Error: There are no server data in local storage");
                    }
                    break;
                case "--france":
                    {
                        //france == 74
                        //albania == 2
                        //Argentina == 10
                        VpnServerQuery query = new VpnServerQuery(null, 74, null, null, null, null);
                        serverList = NordVpnClient.GetAllServerByCountryListAsync(query.CountryId.Value); //France id == 74
                        ConfigurationStorage.StoreValue("serverlist", serverList, false);
                        AppLogger.Log("Saved new server list: " + serverList);
                        DisplayService.DisplayList(serverList);
                        break;
                    }

                case "--TCP":
                    {
                        //UDP = 3
                        //Tcp = 5
                        //Nordlynx = 35
                        VpnServerQuery query = new VpnServerQuery(5, null, null, null, null, null);
                        serverList = NordVpnClient.GetAllServerByProtocolListAsync(query.Protocol.Value);
                        ConfigurationStorage.StoreValue("serverlist", serverList, false);
                        AppLogger.Log("Saved new server list: " + serverList);
                        DisplayService.DisplayList(serverList);
                        break;
                    }
            }
        }
    }
}