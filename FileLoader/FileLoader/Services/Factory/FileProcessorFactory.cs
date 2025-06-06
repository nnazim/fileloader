using FileLoader.Services.Processors;

namespace FileLoader.Services.Factory
{
    public class FileProcessorFactory : IFileProcessorFactory
    {
        private readonly IDictionary<string, IFileProcessor> _processors;

        public FileProcessorFactory(IEnumerable<IFileProcessor> processors)
        {
            _processors = processors.ToDictionary(p => p.ProcessType.ToLower());
        }

        public IFileProcessor GetProcessor(string processType)
        {
            if (!_processors.TryGetValue(processType.ToLower(), out var processor))
                throw new ArgumentException($"Unknown process type: {processType}");
            return processor;
        }
    }
}
