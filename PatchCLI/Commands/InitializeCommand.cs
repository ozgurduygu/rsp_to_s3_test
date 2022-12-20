using CommandLine;
using MHLab.Patch.Core.IO;

namespace MHLab.Patch.Admin.Commands
{
    [Verb("init", HelpText = "Initialize the workspace.")]
    public class InitializeCommand : BaseCommand
    {
        public InitializeCommand() : base()
        {
        }
        
        public override void Handle()
        {
            FileSystem.CreateDirectory((FilePath)Settings.GetApplicationFolderPath());
            FileSystem.CreateDirectory((FilePath)Settings.GetUpdaterFolderPath());
        }
    }
}