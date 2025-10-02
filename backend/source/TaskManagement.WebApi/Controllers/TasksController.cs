using AutoMapper;
using TaskManagement.Domain.Tasks;
using TaskManagement.Domain.Tasks.AssignTasks;
using TaskManagementWebApi.Models.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementWebApi.Controllers
{
    /// <summary>
    /// Task controller.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class TasksController : ApiControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Tasks controller 
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="mapper"></param>
        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all task by status and assignee
        /// </summary>
        /// <param name="taskStatusId"></param>
        /// <param name="assigneeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? taskStatusId, [FromQuery(Name = "assignee")] Guid? assigneeId)
        {
            try
            {
                var tasks = await _taskService.GetAllAsync(taskStatusId, assigneeId);
                var taskViewModels = _mapper.Map<List<TaskListViewModel>>(tasks);
                return Ok(taskViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }
        /// <summary>
        /// Get tasks by assignees.
        /// </summary>
        /// <param name="assigneeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getTasksByAssignees")]
        public async Task<IActionResult> GetTasksByAssignees([FromBody]List<Guid> assigneeId)
        {
            try
            {
                var tasks = await _taskService.GetAllAssignnesAsync(assigneeId);
                var taskViewModels = _mapper.Map<List<TaskListViewModel>>(tasks);
                return Ok(taskViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Add new task.
        /// </summary>
        /// <param name="manageTaskViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ManageTaskViewModel manageTaskViewModel)
        {
            try
            {
                if (manageTaskViewModel == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Task can not be null" });

                var task = _mapper.Map<Tasks>(manageTaskViewModel);
                task.CreatorId = UserId;
                var createdTask = await _taskService.AddAsync(task);
                await AssignTaskTousers(createdTask.Id, manageTaskViewModel);   
                return Ok(new { message = "Task created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Update existing task.
        /// </summary>
        /// <param name="manageTaskViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ManageTaskViewModel manageTaskViewModel)
        {
            try
            {
                if (manageTaskViewModel == null || !manageTaskViewModel.Id.HasValue)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Task or Task Id can not be null" });

                var existingTask = await _taskService.GetByIdAsync(manageTaskViewModel.Id.Value);
                if (existingTask == null)
                    return NotFound();

                _mapper.Map(manageTaskViewModel, existingTask);

                var updatedTask = await _taskService.UpdateAsync(existingTask);
                await AssignTaskTousers(updatedTask.Id, manageTaskViewModel);
                return Ok(new { message = "Task updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var isDeleted = await _taskService.DeleteAsync(id);
                if (!isDeleted)
                    return NotFound();
                return Ok(new { message = "Task deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        private async Task AssignTaskTousers(Guid taskId, ManageTaskViewModel manageTaskViewModel)
        {
            if (manageTaskViewModel.AssigneeId == null || !manageTaskViewModel.AssigneeId.Any())
                return;

            var assignUsersToTaskRequest = new AssignUsersToTaskRequest
            {
                TaskId = taskId,
                UserIds = manageTaskViewModel.AssigneeId
            };
            await _taskService.AssignTaskAsync(assignUsersToTaskRequest);
        }
    }
}







