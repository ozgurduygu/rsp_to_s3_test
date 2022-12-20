using CommandLine;
using MHLab.Patch.Core.Admin;
using MHLab.Patch.Core.Versioning;

namespace MHLab.Patch.Admin.Commands
{
    [Verb("build", HelpText = "Process a new build from your application files.")]
    public class BuildCommand : BaseCommand
    {
        private const string DefaultVersion = "0.1.0";
        
        [Option('b', "build", Default = null, Required = false, HelpText = "The version string for the build.")]
        public string Version { get; set; }
        
        [Option('r', "release", Default = "fix", Required = false, HelpText = "Define the release type. Possible values: major, minor, fix.")]
        public string ReleaseType { get; set; }
        
        private readonly AdminBuildContext _context;
        
        public BuildCommand() : base()
        {
            _context = new AdminBuildContext(Settings, Progress)
            {
                Serializer = Serializer,
                Logger     = Logger,
            };
        }

        private IVersion GetVersionToBuild()
        {
            if (string.IsNullOrWhiteSpace(Version) == false)
                return _context.VersionFactory.Create(Version);
            
            var currentVersion = _context.GetLastVersion();

            if (currentVersion == null)
            {
                return _context.VersionFactory.Create(DefaultVersion);
            }

            switch (ReleaseType.ToLower())
            {
                case "major":
                    currentVersion.UpdateMajor();
                    break;
                case "minor":
                    currentVersion.UpdateMinor();
                    break;
                default:
                    currentVersion.UpdatePatch();
                    break;
            }

            return currentVersion;
        }
        
        public override void Handle()
        {
            var version = GetVersionToBuild();

            _context.BuildVersion      = version;
            _context.LocalizedMessages = Localization;

            _context.Initialize();

            var builder = new BuildBuilder(_context);
            builder.Build();
        }
    }
}