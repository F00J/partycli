using partycli.Clients.NordVpnClient;
using partycli.Enums;
using System.Threading.Tasks;

namespace partycli.Commands
{
    internal abstract class BaseCommand
    {
        internal abstract CLICommand CLICommand { get; }
        internal abstract Task ExecuteAsync(string[] args);

        protected NordVpnClient NordVpnClient { get; }

        protected BaseCommand(NordVpnClient nordVpnClient)
        {
            NordVpnClient = nordVpnClient;
        }

        internal virtual bool ValidateArgs(string[] args)
        {
            return true;
        }
    }
}