using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;
using System.Net;
using TodoList.Data;
using TodoList.Services.ToDoListsServices;

namespace TodoList.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Lists/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [EnableRateLimiting("FixedWindow")]
  


    public class ToDoListsController : ControllerBase
    {

 

        private readonly ITodoListServices _ListServices;

        private readonly ILogger<ToDoListsController> _Logger;

        private readonly string[] pirority = [ "Important"  , "Normal" ];
        public ToDoListsController(ITodoListServices ListServices  , ILogger<ToDoListsController>logger )
        {

            _ListServices = ListServices;
            _Logger = logger;


        }


        [HttpGet("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllToDoLists(int Id , [FromQuery] string? filtering )
        {


            if (Id == 0)
            {
                _Logger.LogError("In TodoListscontroller in GetAllToDoLists : Error 400 The List Id is invalid");
                return BadRequest("The List Id is invalid");
            }

            var List = await _ListServices.GetListById(Id);

            if (List == null)
            {
                _Logger.LogError("In TodoListscontroller in GetAllToDoLists : Error 404 This List doesn't exist");
                return NotFound("This List doesn't exist");
            }

            var toDoLists = await _ListServices.GetAllTodoLists(Id , filtering);

            return Ok(toDoLists);


        }

        //Create a list

        [HttpPost("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> CreateList(int Id, [FromBody] CreateToDoListDTO Data)
        {

            
            if (Id == 0)
            {

                _Logger.LogError("In TodoListscontroller in CreateList : Error 400 The List Id is invalid");
                return BadRequest("The List Id is invalid");
            }

            if (Data == null)
            {
                _Logger.LogError("In TodoListscontroller in CreateList : Error 404 This List doesn't exist");
                return NotFound("This List doesn't exist");
            }

            // Encode the input to prevent XSS

            Data.Name = WebUtility.HtmlEncode(Data.Name);
            Data.Priority = WebUtility.HtmlEncode(Data.Priority);


            var List = await _ListServices.GetListById(Id);

            if (List == null)
            {
                return NotFound();
            }

            await _ListServices.InsertTodoList(Id, Data);


            return Ok("Good");



        }






        [HttpPut()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> UpdateTodoList([FromBody] UpdateToDoListDTO Data)
        {

 
            if (Data == null)
            {
                _Logger.LogError("In TodoListscontroller in UpdateTodoList : Error 400  invalid Todo list ");
                return BadRequest("Invalid Todo list ");
            }

            var GetTodoList = await _ListServices.GetTodoListById(Data.Id);

            if (GetTodoList == null)
            {
                _Logger.LogError("In TodoListscontroller in UpdateTodoList : Error 404 This Todo list doesn't exist");
                return NotFound("This Todo list doesn't exist");
            }

            // Encode the input to prevent XSS

            Data.Name = WebUtility.HtmlEncode(Data.Name);
            Data.Priority = WebUtility.HtmlEncode(Data.Priority);


            GetTodoList.Name = Data.Name;
            GetTodoList.Priority = Data.Priority;
            

            await _ListServices.UpdateTodoList(GetTodoList);


            return Ok();


        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteTodoList(int Id)
        {


            if (Id == null)
            {
                _Logger.LogError("In TodoListscontroller in DeleteTodoList : Error 400  invalid Todo list Id ");
                return BadRequest("Invalid Todo list Id");
            }

            var GetTodoList = await _ListServices.GetTodoListById(Id);

            if (GetTodoList == null)
            {
                _Logger.LogError("In TodoListscontroller in DeleteTodoList : Error 404 This Todo list doesn't exist");
                return NotFound("This Todo list doesn't exist");
            }


            await _ListServices.DeleteTodoList(GetTodoList);


            return Ok();


        }





    }
}

