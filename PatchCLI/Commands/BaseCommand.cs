using System;
using MHLab.Patch.Admin.Localization;
using MHLab.Patch.Admin.Serializing;
using MHLab.Patch.Core.Admin;
using MHLab.Patch.Core.Admin.Localization;
using MHLab.Patch.Core.Admin.Progresses;
using MHLab.Patch.Core.IO;
using MHLab.Patch.Core.Logging;

namespace MHLab.Patch.Admin.Commands
{
    public abstract class BaseCommand
    {
        protected JsonSerializer            Serializer;
        protected ILogger                   Logger;
        protected IAdminSettings            Settings;
        protected IAdminLocalizedMessages   Localization;
        protected Progress<BuilderProgress> Progress;
        protected IFileSystem               FileSystem;

        protected BaseCommand()
        {
            FileSystem = new FileSystem();

            Settings = new AdminSettings()
            {
                RootPath    = FileSystem.GetCurrentDirectory().FullPath,
                AppDataPath = FileSystem.GetApplicationDataPath("PATCH Admin Tool").FullPath
            };
            Localization = new EnglishAdminLocalizedMessages();

            Progress = new Progress<BuilderProgress>();
            Progress.ProgressChanged += ProgressChanged;

            Serializer = new JsonSerializer();
            Logger     = new SimpleLogger(FileSystem, Settings.GetLogsFilePath(), Settings.DebugMode);
        }
        
        private void ProgressChanged(object sender, BuilderProgress e)
        {
            Console.WriteLine($"{e.StepMessage} [{e.CurrentSteps}/{e.TotalSteps}]");
        }

        public abstract void Handle();
    }
}