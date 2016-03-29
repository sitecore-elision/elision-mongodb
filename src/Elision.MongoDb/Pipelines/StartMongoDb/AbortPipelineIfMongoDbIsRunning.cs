using System.Configuration;
using System.Text.RegularExpressions;
using Sitecore.Analytics.Data.DataAccess.MongoDb;
using Sitecore.Diagnostics;

namespace Elision.MongoDb.Pipelines.StartMongoDb
{
    public class AbortPipelineIfMongoDbIsRunning
    {
        private readonly string _connectionStringName;

        public AbortPipelineIfMongoDbIsRunning(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void Process(StartMongoDbArgs args)
        {
            Log.Audit($"{this} Testing mongodb connection", this);

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[_connectionStringName];
            if (connectionStringSettings == null)
            {
                Log.SingleError($"{this} Unable to determine MongoDB status because connection string name '{_connectionStringName}' was not found in the web.config.", this);
                args.AbortPipeline();
                return;
            }

            var driver = new MongoDbDriver(connectionStringSettings.ConnectionString);
            try
            {
                if (driver.DatabaseAvailable)
                {
                    Log.Audit($"{this} Mongo Already running", this);
                    args.AbortPipeline();
                    return;
                }

                var match = Regex.Match(connectionStringSettings.ConnectionString, @"mongodb://.+:(?<port>\d+)/");
                if (match.Success)
                {
                    var intVal = 0;
                    if (int.TryParse(match.Groups["port"].Value, out intVal))
                        args.Port = intVal;
                }
            }
            catch{}

            Log.Audit($"{this} Mongo Not Running", this);
        }
    }
}
