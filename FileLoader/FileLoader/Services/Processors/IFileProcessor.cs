using FileLoader.Services.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileLoader.Services.Processors
{
    public interface IFileProcessor
    {
        string ProcessType { get; }  // e.g. "masscardcreation", "tolltransaction", "justification"
        Task ProcessAsync(FileContext context);
    }
}
