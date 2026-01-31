using partycli.Enums;

namespace partycli.Commands
{
    internal abstract class BaseCommand
    {
        internal abstract CLICommand CLICommand { get; }
        internal abstract void Execute(string[] args);
        
        internal virtual bool ValidateArgs(string[] args)
        {
            return true;
        }
    }
}