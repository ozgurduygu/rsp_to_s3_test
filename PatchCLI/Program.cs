using System;
using MHLab.Patch.Admin.Commands;
using CommandLine;

namespace MHLab.Patch.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<
                      InitializeCommand,
                      BuildCommand,
                      GetVersionsCommand,
                      PatchCommand,
                      LauncherUpdateCommand>(args)
                  .WithParsed<InitializeCommand>(c => c.Handle())
                  .WithParsed<BuildCommand>(c => c.Handle())
                  .WithParsed<GetVersionsCommand>(c => c.Handle())
                  .WithParsed<PatchCommand>(c => c.Handle())
                  .WithParsed<LauncherUpdateCommand>(c => c.Handle())
                  .WithNotParsed(errors =>
                  {
                      foreach (var error in errors)
                      {
                          Console.WriteLine(error);
                      }
                  });
        }
    }
}