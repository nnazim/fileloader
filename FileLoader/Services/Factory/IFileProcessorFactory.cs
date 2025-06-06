using FileLoader.Services.Processors;

namespace FileLoader.Services.Factory
{
    public interface IFileProcessorFactory
    {
        IFileProcessor GetProcessor(string processType);
    }
}
