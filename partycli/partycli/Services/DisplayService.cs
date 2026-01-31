using partycli.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace partycli.Services
{
    internal static class DisplayService
    {
        internal static void DisplayList(List<ServerDto> serverList)
        {
            if (serverList is null)
            {
                throw new ArgumentNullException(nameof(serverList));
            }

            Console.WriteLine("Server list: ");

            for (int index = 0; index < serverList.Count(); index++)
            {
                Console.WriteLine("Name: " + serverList[index].Name);
            }

            Console.WriteLine("Total servers: " + serverList.Count());
        }
    }
}
