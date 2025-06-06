using FileLoader.Dtos;
using FileLoader.Services;
using FileLoader.Services.Factory;
using FileLoader.Services.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace FileLoader.Controllers
{
    [Route("fileloader/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly FileProcessingService _processingService;

        public FileController(FileProcessingService processingService)
        {
            _processingService = processingService;
        }


        [HttpPost("process")]
        public async Task<ActionResult> ProcessFile(ProcessFileRequest file)
        {
            if (file == null || string.IsNullOrWhiteSpace(file.Key))
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                await _processingService.HandleFileAsync(file.Key);
                return Ok("Processing started");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
