using FileLoader.Services.Model;

namespace FileLoader.Services.Processors
{
    public class MassCardCreationProcessor : IFileProcessor
    {
        public string ProcessType => "masscardcreation";

        public Task ProcessAsync(FileContext context)
        {
            throw new NotImplementedException();
        }
    }
}
