using Newtonsoft.Json;
using partycli.Enums;
using partycli.Logger;
using partycli.Models;
using partycli.Services;
using partycli.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            States currentState = States.none;
            string name = null;
            int argIndex = 1;

            foreach (string arg in args)
            {
                if (currentState == States.none)
                {
                    if (arg == "server_list")
                    {
                        currentState = States.server_list;
                        if (argIndex >= args.Count())
                        {
                            string serverList = NordVpnClient.GetAllServersListAsync();
                            ConfigurationStorage.StoreValue("serverlist", serverList, false);
                            AppLogger.Log("Saved new server list: " + serverList);
                            DisplayList(serverList);
                        }
                    }
                    if (arg == "config")
                    {
                        currentState = States.config;
                    }
                }
                else if (currentState == States.config)
                {
                    if (name == null)
                    {
                        name = arg;
                    }
                    else
                    {
                        ConfigurationStorage.StoreValue(ProccessName(name), arg);
                        AppLogger.Log("Changed " + ProccessName(name) + " to " + arg);
                        name = null;
                    }
                }
                else if (currentState == States.server_list)
                {
                    if (arg == "--local")
                    {
                        if (!String.IsNullOrEmpty(Properties.Settings.Default.serverlist))
                        {
                            DisplayList(Properties.Settings.Default.serverlist);
                        }
                        else
                        {
                            Console.WriteLine("Error: There are no server data in local storage");
                        }
                    }
                    else if (arg == "--france")
                    {
                        //france == 74
                        //albania == 2
                        //Argentina == 10
                        VpnServerQuery query = new VpnServerQuery(null, 74, null, null, null, null);
                        string serverList = NordVpnClient.GetAllServerByCountryListAsync(query.CountryId.Value); //France id == 74
                        ConfigurationStorage.StoreValue("serverlist", serverList, false);
                        AppLogger.Log("Saved new server list: " + serverList);
                        DisplayList(serverList);
                    }
                    else if (arg == "--TCP")
                    {
                        //UDP = 3
                        //Tcp = 5
                        //Nordlynx = 35
                        VpnServerQuery query = new VpnServerQuery(5, null, null, null, null, null);
                        string serverList = NordVpnClient.GetAllServerByProtocolListAsync((int)query.Protocol.Value);
                        ConfigurationStorage.StoreValue("serverlist", serverList, false);
                        AppLogger.Log("Saved new server list: " + serverList);
                        DisplayList(serverList);
                    }
                }

                argIndex++;
            }

            if (currentState == States.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }
            Console.Read();
        }

        static string ProccessName(string name) => name.Replace("-", string.Empty);

        internal static void DisplayList(string serverListString)
        {
            List<ServerModel> serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);
            
            Console.WriteLine("Server list: ");
            
            for (int index = 0; index < serverlist.Count(); index++)
            {
                Console.WriteLine("Name: " + serverlist[index].Name);
            }
            
            Console.WriteLine("Total servers: " + serverlist.Count());
        }
    }
}
