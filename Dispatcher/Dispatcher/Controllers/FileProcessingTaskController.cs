using Dispatcher.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers
{
    [Route("api/subtask")]
    [ApiController]
    public class FileProcessingTaskController : ControllerBase
    {
        private readonly IFileProcessingTaskService _fileProcessingTaskService;

        public FileProcessingTaskController(IFileProcessingTaskService fileProcessingTaskService)
        {
            _fileProcessingTaskService = fileProcessingTaskService ?? throw new ArgumentNullException(nameof(fileProcessingTaskService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubtaskById(Guid id)
        {
            var subtask = await _fileProcessingTaskService.GetFileProcessingTaskByIdAsync(id);

            return Ok(subtask);
        }
    }
}
