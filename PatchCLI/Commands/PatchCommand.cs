using System;
using CommandLine;
using MHLab.Patch.Core.Admin;
using MHLab.Patch.Core.Versioning;

namespace MHLab.Patch.Admin.Commands
{
    [Verb("patch", HelpText = "Process a new patch starting from two builds.")]
    public class PatchCommand : BaseCommand
    {
        [Option("from", Default = null, Required = false, HelpText = "The version to start from.")]
        public string FromVersion { get; set; }
        
        [Option("to", Default = null, Required = false, HelpText = "The target version.")]
        public string ToVersion { get; set; }
        
        [Option('c', "compression", Default = 6, Required = false, HelpText = "The compression level, from 1 to 9. The bigger the number, the stronger the compression.")]
        public int CompressionLevel { get; set; }
        
        private readonly AdminPatchContext _context;
        
        public PatchCommand() : base()
        {
            _context = new AdminPatchContext(Settings, Progress)
            {
                Serializer = Serializer,
                Logger     = Logger,
            };
        }

        private (IVersion from, IVersion to) GetVersions()
        {
            if (string.IsNullOrWhiteSpace(FromVersion) == false &&
                string.IsNullOrWhiteSpace(ToVersion)   == false)
            {
                var from = _context.VersionFactory.Create(FromVersion); 
                var to = _context.VersionFactory.Create(ToVersion);
                return (from, to);
            }
            
            var versions = _context.GetVersions();

            if (versions.Count < 2) throw new Exception("Not enough versions to build a patch.");

            return (versions[versions.Count - 2], versions[versions.Count - 1]);
        }
        
        public override void Handle()
        {
            var (from, to) = GetVersions();
            
            _context.LocalizedMessages = Localization;
            _context.VersionFrom       = from;
            _context.VersionTo         = to;
            _context.CompressionLevel  = Math.Clamp(CompressionLevel, 1, 9);

            _context.Initialize();

            var builder = new PatchBuilder(_context);
            builder.Build();
        }
    }
}