using FileLoader.Services.Model;

namespace FileLoader.Services.Processors
{
    public class TollTransactionProcessor : IFileProcessor
    {
        public string ProcessType => "tolltransaction";

        public Task ProcessAsync(FileContext context)
        {
            throw new NotImplementedException();
        }
    }
}
