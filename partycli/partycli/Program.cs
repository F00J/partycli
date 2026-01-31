using partycli.Commands;
using partycli.Factories;
using partycli.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: partycli <command> [options] [<command> [options]]...");
                Console.WriteLine("Available commands:");

                CommandFactory factory = new CommandFactory();
                foreach (string cmd in factory.GetAvailableCommands())
                {
                    Console.WriteLine($"  - {cmd}");
                }

                Console.Read();
                return;
            }

            CommandFactory commandFactory = new CommandFactory();

            int i = 0;
            while (i < args.Length)
            {
                string commandName = args[i];
                BaseCommand command = commandFactory.GetCommand(commandName);

                if (command == null)
                {
                    Console.WriteLine($"Unknown command: {commandName}");
                    i++;
                    continue;
                }

                List<string> commandArgs = new List<string>();
                int j = i + 1;

                while (j < args.Length && commandFactory.GetCommand(args[j]) == null)
                {
                    commandArgs.Add(args[j]);
                    j++;
                }

                if (command.ValidateArgs(commandArgs.ToArray()))
                {
                    try
                    {
                        await command.ExecuteAsync(commandArgs.ToArray());
                        AppLogger.Log($"Successfully executed: {command.CLICommand}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing {commandName}: {ex.Message}");
                        AppLogger.Log($"Error in {command.CLICommand}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid arguments for command: {commandName}");
                }

                i = j;
            }

            Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
            Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
            Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
            Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");

            Console.Read();
        }
    }
}
