using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Model.Models;
 
using System.Net;
using TodoList.Data;
using TodoList.DataAccess.Repositories.TodoListRepo;
using TodoList.Models.DTO.ToDoListsDTO;
using TodoList.Services.ToDoListsServices;

namespace TodoList.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Lists/")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [EnableRateLimiting("FixedWindow")]
   



    public class ToDoListsController : ControllerBase
    {

 

        private readonly ITodoListServices _ListServices;

        private readonly ILogger<ToDoListsController> _Logger;

         public ToDoListsController(ITodoListServices ListServices  , ILogger<ToDoListsController>logger )
        {

            _ListServices = ListServices;
            _Logger = logger;


        }


        //Get all todolists from a list

        [HttpGet("{ListId}/[controller]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllToDoLists(int ListId)
        {
            //Inputs validation

            if (ListId == 0)
            {
                _Logger.LogError("In TodoListscontroller in GetAllToDoLists : Error 400 The List Id is invalid");
                return BadRequest("The List Id is invalid");
            }


            //Check if the list does exist in the database

            var List = await _ListServices.GetListById(ListId);

            if (List == null)
            {
                _Logger.LogError("In TodoListscontroller in GetAllToDoLists : Error 404 This List doesn't exist");
                return NotFound("This List doesn't exist");
            }


            //Request the todo lists from the database 

            var toDoLists = await _ListServices.GetAllTodoLists(ListId);

            return Ok(toDoLists);


        }

        //Create a todo list

        [HttpPost("{ListId}/[controller]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> CreateList(int ListId, [FromBody] CreateToDoListDTO Data)
        {

            //Inputs validation

            if (ListId == 0)
            {

                _Logger.LogError("In TodoListscontroller in CreateList : Error 400 The List Id is invalid");
                return BadRequest("The List Id is invalid");
            }

            if (Data == null)
            {
                _Logger.LogError("In TodoListscontroller in CreateList : Error 404 This List doesn't exist");
                return NotFound("This List doesn't exist");
            }


            //Check if the List does exist  in the database

            var List = await _ListServices.GetListById(ListId);

            if (List == null)
            {
                return NotFound();
            }

            // Encode the input to prevent XSS

            Data.Name = WebUtility.HtmlEncode(Data.Name);
            Data.color = WebUtility.HtmlEncode(Data.color);


            //Insert the todo list

            await _ListServices.InsertTodoList(ListId , Data);


            return Ok("Todo list inserted successfully");



        }




        //Update a todo list

        [HttpPut("/[controller]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> UpdateTodoList([FromBody] UpdateToDoListDTO Data)
        {


            //Inputs validation

            if (Data == null)
            {
                _Logger.LogError("In TodoListscontroller in UpdateTodoList : Error 400  invalid Todo list ");
                return BadRequest("Invalid Todo list ");
            }


            //Check if the todo list exist in the database

            var GetTodoList = await _ListServices.GetTodoListById(Data.Id);

            if (GetTodoList == null)
            {
                _Logger.LogError("In TodoListscontroller in UpdateTodoList : Error 404 This Todo list doesn't exist");
                return NotFound("This Todo list doesn't exist");
            }

            // Encode the input to prevent XSS

            Data.Name = WebUtility.HtmlEncode(Data.Name);
            Data.color = WebUtility.HtmlEncode(Data.color);

            //Update the todo list data

            GetTodoList.Name = Data.Name;
            GetTodoList.color = Data.color;
            GetTodoList.tottalTodoLists = Data.tottalTodoLists;
            GetTodoList.finishedTodoLists = Data.finishedTodoLists;
            
            

            await _ListServices.UpdateTodoList(GetTodoList);


            return Ok("Todo list Updated successfully");


        }


        //Delete a todo list

        [HttpDelete("[controller]/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteTodoList(int Id)
        {

            //Inputs validation

            if (Id == null)
            {
                _Logger.LogError("In TodoListscontroller in DeleteTodoList : Error 400  invalid Todo list Id ");
                return BadRequest("Invalid Todo list Id");
            }

            //Check if the todo list exist in the database

            var GetTodoList = await _ListServices.GetTodoListById(Id);

            if (GetTodoList == null)
            {
                _Logger.LogError("In TodoListscontroller in DeleteTodoList : Error 404 This Todo list doesn't exist");
                return NotFound("This Todo list doesn't exist");
            }

            //Delete the todo list

            await _ListServices.DeleteTodoList(GetTodoList);

            return Ok("Todo list deleted successfully");

        }


    }
}

