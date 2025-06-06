using FileLoader.Services.Factory;
using FileLoader.Services.Model;

namespace FileLoader.Services
{
    public class FileProcessingService
    {
        private readonly IFileProcessorFactory _factory;

        public FileProcessingService(IFileProcessorFactory factory)
        {
            _factory = factory;
        }

        public async Task HandleFileAsync(string key)
        {
            var context = new FileContext(key);
            var processor = _factory.GetProcessor(context.ProcessType);
            await processor.ProcessAsync(context);
        }
    }
}
