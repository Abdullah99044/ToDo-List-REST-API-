using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Model.Models.DTO.TodoTasksDTO;
using System.Net;
using TodoList.Services.TodoTasksService;



//Rate limiting ok
//Versioning ok
//Authrization ok
//Validation ok
//Logger
//Service ok
// Anti XSS ok 
//End points ok

namespace TodoList.Controllers.v1
{
    [Route("api/{version:apiVersion}/Lists/TodoLists/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [EnableRateLimiting("FixedWindow")]
    [Authorize]
    public class ToDoTasksController : ControllerBase
    {

        private readonly ITodoTasksService _TodoTasksServices;
        private readonly ILogger<ToDoTasksController> _logger;
        public ToDoTasksController(ITodoTasksService TodoTasksServices, ILogger<ToDoTasksController> logger)
        {
            _TodoTasksServices = TodoTasksServices;
            _logger = logger;

        }

        [HttpPost("{TodoListId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> insertTodoTask(int TodoListId, CreateTodoTasksDTO TodoTask)
        {

            if (TodoTask == null)
            {
                return BadRequest();
            }

            //check the todoListId

            var GetTodoListId = await _TodoTasksServices.checkTodoListId(TodoListId);

            if (GetTodoListId == null)
            {
                return NotFound();
            };


            TodoTask.Name = WebUtility.HtmlEncode(TodoTask.Name);

            //Insert the todo task in the database

            await _TodoTasksServices.insertTodoTask(TodoListId, TodoTask , GetTodoListId.ListId);


            return Ok("Todo task inserted");
        }


        [HttpPut("{TodoListId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> udpateTodoTask(int TodoListId, updateTodoTasksDTO TodoTask )
        {

            if (TodoTask == null)
            {
                return BadRequest();
            }

            //Get the List Id to update the cache data

            var GetTodoListId = await _TodoTasksServices.checkTodoListId(TodoListId);


            var getTodoTasks = await _TodoTasksServices.checkTodoTasksId(TodoTask.Id);

            if (getTodoTasks == null || GetTodoListId == null )
            {
                return NotFound();
            }

            getTodoTasks.Name = WebUtility.HtmlEncode(TodoTask.Name);
            getTodoTasks.status = WebUtility.HtmlEncode(TodoTask.status);
            getTodoTasks.Updated = DateTime.Now;

            //update the todo task in the database

            await _TodoTasksServices.updateTodoTask(getTodoTasks , GetTodoListId.ListId );

            return Ok("Todo task updated");
        }




        [HttpDelete("{TodoTaskId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> daleteTodoTask(int TodoTaskId )
        {

            if (TodoTaskId == null  )
            {
                return BadRequest();
            }

            var getTodoTasks = await _TodoTasksServices.checkTodoTasksId(TodoTaskId);


            //Get the List Id to update the cache data

            var GetTodoListId = await _TodoTasksServices.checkTodoListId(getTodoTasks.todoListId);


            if (getTodoTasks == null || GetTodoListId  == null )
            {
                return NotFound();
            }

            //update the todo task in the database

            await _TodoTasksServices.deleteTodoTask(getTodoTasks , GetTodoListId.ListId );

            return Ok("Todo task Deleted");
        }



    }
}
