using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;
using TodoList.Data;
using TodoList.Services.ToDoListsServices;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {


        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private ITodoListServices _ListServices;
        public ToDoListsController(ApplicationDbContext db, IMapper mapper, ITodoListServices ListServices)
        {
            _db = db;
            _mapper = mapper;
            _ListServices = ListServices;

        }

         

        //Create a list

        [HttpPost( "{Id:int}" , Name = "CreateList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> CreateList( int Id , [FromBody] CreateToDoListDTO Data)
        {
            if (Data == null)
            {
                return BadRequest("No data to creat a List");
            }

            var CheckListId = await _ListServices.checkList(Id);

            if (CheckListId == null)
            {
                return NotFound("List doesn't exist");
            }

            await _ListServices.insertToDoList(Data, Id);

            return Ok();
        }

         

    }
}
 
