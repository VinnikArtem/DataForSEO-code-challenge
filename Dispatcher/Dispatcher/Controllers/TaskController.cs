using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndQueueSuperTaskAsync([FromBody] TaskRequest taskRequest)
        {
            await _taskService.CreateAndQueueSuperTaskAsync(taskRequest);

            return Created();
        }
    }
}
