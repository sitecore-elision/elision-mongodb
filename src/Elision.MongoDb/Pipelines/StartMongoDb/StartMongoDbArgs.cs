using Sitecore.Pipelines;

namespace Elision.MongoDb.Pipelines.StartMongoDb
{
    public class StartMongoDbArgs : PipelineArgs
    {
        public string ExePath { get; set; }
        public string DbFolderPath { get; set; }
        public int? Port { get; set; }
        public string AdditionalArgs { get; set; }
    }
}