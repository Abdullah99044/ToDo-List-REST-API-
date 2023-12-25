using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Todolist.Model.DTO.TodoTasksDTO;
using System.Net;
using TodoList.Model.Services.TodoTasksService;


 

namespace TodoList.Controllers.v1
{
    [Route("api/{version:apiVersion}/Lists/TodoLists/")]
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


        //Create todo task

        [HttpPost("{TodoListId}/[controller]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> insertTodoTask(int TodoListId, CreateTodoTasksDTO TodoTask)
        {

            //Inputs validation

            if (TodoTask == null)
            {
                _logger.LogError("TodoTasksController  insertTodoTask : 400 invalid TodoTask data");
                return BadRequest(" Invalid TodoTask data");
            }

            //check if the todo task exist in the database 

            var GetTodoListId = await _TodoTasksServices.getTodoList(TodoListId);

            if (GetTodoListId == null)
            {
                _logger.LogError("TodoTasksController  insertTodoTask : 404 Todolist  not found");
                return NotFound(" Todolist  not found ");
            };

            // Encode the input to prevent XSS

            TodoTask.Name = WebUtility.HtmlEncode(TodoTask.Name);

            //Insert the todo task in the database

            await _TodoTasksServices.insertTodoTask(TodoListId, TodoTask , GetTodoListId.ListId);


            return Ok("Todo task inserted successfully");
        }


        //Update a todo task


        [HttpPut("{TodoListId}/[controller]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> udpateTodoTask(int TodoListId, updateTodoTasksDTO TodoTask )
        {

            //Inputs validation

            if (TodoTask == null)
            {
                _logger.LogError("TodoTasksController  udpateTodoTask : 400 invalid TodoTask data");
                return BadRequest("Invalid TodoTask data");
            }


            //check if the todo task exist in the database 

            var GetTodoListId = await _TodoTasksServices.getTodoList(TodoListId);


            //Get the List Id to update the cache data

            var getTodoTasks = await _TodoTasksServices.getTodoTasks(TodoTask.Id);

            if (getTodoTasks == null || GetTodoListId == null )
            {
                _logger.LogError("TodoTasksController  udpateTodoTask : 404 TodoTask not found ");
                return NotFound("TodoTask not found ");
            }

            //Update the todo task data

            getTodoTasks.Name = WebUtility.HtmlEncode(TodoTask.Name);
            getTodoTasks.status = WebUtility.HtmlEncode(TodoTask.status);
            getTodoTasks.Updated = DateTime.Now;

            //Update the todo task in the database

            await _TodoTasksServices.updateTodoTask(getTodoTasks , GetTodoListId.ListId );

            return Ok("Todo task updated successfully");
        }



        //Delete a todo task

        [HttpDelete("[controller]/{TodoTaskId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(402)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> daleteTodoTask(int TodoTaskId )
        {

            //Inputs validation

            if (TodoTaskId == null  )
            {
                _logger.LogError("TodoTasksController  daleteTodoTask : 400 invalid TodoTask data");
                return BadRequest("Invalid TodoTask data");
            }

            //check if the todo task exist in the database 

            var getTodoTasks = await _TodoTasksServices.getTodoTasks(TodoTaskId);


            //Get the todo List Id to update the cache data

            var GetTodoListId = await _TodoTasksServices.getTodoList(getTodoTasks.todoListId);


            if (getTodoTasks == null || GetTodoListId  == null )
            {
                _logger.LogError("TodoTasksController  udpateTodoTask : 404 not found TodoTask or todoList ");
                return NotFound(" TodoTask or todoList not found ");
            }

            //Delete the todo task in the database

            await _TodoTasksServices.deleteTodoTask(getTodoTasks , GetTodoListId.ListId );

            return Ok("Todo task Deleted successfully");
        }
    }
}
