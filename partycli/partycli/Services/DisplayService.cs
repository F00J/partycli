using Newtonsoft.Json;
using partycli.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace partycli.Services
{
    internal static class DisplayService
    {
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
