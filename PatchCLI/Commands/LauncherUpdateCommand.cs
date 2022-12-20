using System;
using CommandLine;
using MHLab.Patch.Core.Admin;

namespace MHLab.Patch.Admin.Commands
{
    [Verb("launcherUpdate", HelpText = "Process a new update for the Launcher.")]
    public class LauncherUpdateCommand : BaseCommand
    {
        [Option("archiveName", Default = "MyLauncher", Required = false, HelpText = "The resulting archive name.")]
        public string ArchiveName { get; set; }
        
        [Option('c', "compression", Default = 6, Required = false, HelpText = "The compression level, from 1 to 9. The bigger the number, the stronger the compression.")]
        public int CompressionLevel { get; set; }
        
        private readonly AdminPatcherUpdateContext _context;
        
        public LauncherUpdateCommand() : base()
        {
            _context = new AdminPatcherUpdateContext(Settings, Progress)
            {
                Serializer = Serializer,
                Logger     = Logger,
            };
        }
        
        public override void Handle()
        {
            _context.CompressionLevel    = Math.Clamp(CompressionLevel, 1, 9);
            _context.LauncherArchiveName = ArchiveName;
            _context.LocalizedMessages   = Localization;

            _context.Initialize();

            var builder = new UpdaterBuilder(_context);
            builder.Build();
        }
    }
}