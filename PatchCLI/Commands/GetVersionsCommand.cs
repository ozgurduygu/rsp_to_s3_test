using System;
using System.Text;
using CommandLine;
using MHLab.Patch.Core.Admin;
using MHLab.Patch.Core.Serializing;

namespace MHLab.Patch.Admin.Commands
{
    [Verb("getVersions", HelpText = "Print a list of the existing builds in the workspace.")]
    public class GetVersionsCommand : BaseCommand
    {
        private AdminPatchContext _context;
        
        public GetVersionsCommand() : base()
        {
            _context = new AdminPatchContext(Settings, Progress)
            {
                Serializer = Serializer,
                Logger     = Logger
            };            
        }
        
        public override void Handle()
        {
            var versions = _context.GetVersions();

            var node = new JsonArray();
            for (var i = 0; i < versions.Count; i++)
            {
                var version = versions[i];
                node.Add(new JsonString(version.ToString()));
            }

            var stringBuilder = new StringBuilder();
            node.WriteToStringBuilder(stringBuilder, 2, 2, JsonTextMode.Indent);

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}