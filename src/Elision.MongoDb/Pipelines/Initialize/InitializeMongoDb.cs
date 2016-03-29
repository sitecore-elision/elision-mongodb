using Elision.MongoDb.Pipelines.StartMongoDb;
using Sitecore.Pipelines;

namespace Elision.MongoDb.Pipelines.Initialize
{
    public class InitializeMongoDb
    {
        public void Process(PipelineArgs args)
        {
            CorePipeline.Run("elision.startMongoDb", new StartMongoDbArgs());
        }
    }
}
