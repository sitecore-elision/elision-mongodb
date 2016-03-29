using Elision.MongoDb.Pipelines.StartMongoDb;
using FluentAssertions;
using NUnit.Framework;

namespace Elision.MongoDb.Tests
{
    [TestFixture]
    public class SetOptionsTests
    {
        [Test]
        public void SetsDbPath()
        {
            var processor = new SetOptions(null, "db path", null, null);
            var args = new StartMongoDbArgs();
            processor.Process(args);
            args.DbFolderPath.Should().Be("db path");
        }

        [Test]
        public void SetsExePath()
        {
            var processor = new SetOptions("exe path", null, null, null);
            var args = new StartMongoDbArgs();
            processor.Process(args);
            args.ExePath.Should().Be("exe path");
        }

        [Test]
        public void SetsAdditionalArgs()
        {
            var processor = new SetOptions(null, null, null, "additional args");
            var args = new StartMongoDbArgs();
            processor.Process(args);
            args.AdditionalArgs.Should().Be("additional args");
        }

        [Test]
        public void SetsPort()
        {
            var processor = new SetOptions(null, null, "12345", null);
            var args = new StartMongoDbArgs();
            processor.Process(args);
            args.Port.Value.Should().Be(12345);
        }

        [Test]
        public void LeavesPreviousPortValueWhenNotSupplied()
        {
            var processor = new SetOptions(null, null, "", null);
            var args = new StartMongoDbArgs()
            {
                Port = 12345
            };
            processor.Process(args);
            args.Port.Value.Should().Be(12345);
        }
    }
}
