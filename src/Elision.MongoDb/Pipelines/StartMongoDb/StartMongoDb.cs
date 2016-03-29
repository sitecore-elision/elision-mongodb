using System;
using System.Diagnostics;
using System.IO;
using Sitecore.Diagnostics;

namespace Elision.MongoDb.Pipelines.StartMongoDb
{
    public class StartMongoDb
    {
        public void Process(StartMongoDbArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.ExePath))
            {
                Log.Error($"{this} Unable to start MongoDB because exe path was not specified.", this);
                return;
            }

            args.ExePath = Environment.ExpandEnvironmentVariables(args.ExePath);
            if (!File.Exists(args.ExePath))
            {
                Log.Error($"{this} Unable to start MongoDB because exe path {args.ExePath} was not found.", this);
                return;
            }

            if (string.IsNullOrWhiteSpace(args.DbFolderPath))
            {
                Log.Error($"{this} Unable to start MongoDB because no database folder path was configured.", this);
                return;
            }

            if (!Directory.Exists(args.DbFolderPath))
                Directory.CreateDirectory(args.DbFolderPath);

            if (!Directory.Exists(args.DbFolderPath))
            {
                Log.Error($"{this} Unable to create configured db folder path {args.DbFolderPath}", this);
                return;
            }

            var procArgs = $"--dbpath {args.DbFolderPath}";
            if (args.Port.HasValue && args.Port.Value > 0)
                procArgs += $" --port {args.Port.Value}";

            if (!string.IsNullOrWhiteSpace(args.AdditionalArgs))
                procArgs += $" {args.AdditionalArgs}";

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = args.ExePath, //@"C:\Program Files\MongoDB 2.6 Standard\bin\mongod.exe",
                Arguments = procArgs,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                Log.Audit($"{this} Trying to start mongo with command line {startInfo.FileName} {startInfo.Arguments}", this);
                var pid = 0;
                using (var exeProcess = System.Diagnostics.Process.Start(startInfo))
                {
                    pid = exeProcess.Id;
                    exeProcess.WaitForExit(50);
                }
                Log.Audit($"{this} MongoDb started. PID: {pid}", this);
            }
            catch (Exception exception)
            {
                Log.Error($"{this} Could not start mongo", exception, this);
            }
        }
    }
}
