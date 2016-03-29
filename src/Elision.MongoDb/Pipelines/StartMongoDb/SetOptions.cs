namespace Elision.MongoDb.Pipelines.StartMongoDb
{
    public class SetOptions
    {
        private readonly string _exePath;
        private readonly string _dbFolderPath;
        private readonly int? _port;
        private readonly string _additionalArgs;

        public SetOptions(string exePath, string dbFolderPath, string port, string additionalArgs)
        {
            _additionalArgs = additionalArgs;
            _exePath = exePath;
            _dbFolderPath = dbFolderPath;

            var intVal = 0;
            _port = int.TryParse(port, out intVal) ? intVal : (int?) null;
        }

        public void Process(StartMongoDbArgs args)
        {
            if (!string.IsNullOrWhiteSpace(_exePath))
                args.ExePath = _exePath;
            if (!string.IsNullOrWhiteSpace(_dbFolderPath))
                args.DbFolderPath = _dbFolderPath;
            if (!string.IsNullOrWhiteSpace(_additionalArgs))
                args.AdditionalArgs = _additionalArgs;
            if (_port.HasValue && _port.Value > 0)
                args.Port = _port.Value;
        }
    }
}
