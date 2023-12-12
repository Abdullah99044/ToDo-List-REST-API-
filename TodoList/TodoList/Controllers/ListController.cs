using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using TodoList.Data;
using TodoList.Services;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {


        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private IListsServices _ListServices;
        public ListController(ApplicationDbContext db, IMapper mapper, IListsServices ListServices)
        {
            _db = db;
            _mapper = mapper;
            _ListServices = ListServices;

        }


        //Get all lists

        [HttpGet("GetLists/{Email}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetLists(string Email)
        {

            if (Email == null)
            {
                return BadRequest("No data to get your Lists");
            }


            var GetUserID = await _ListServices.GetUserId(Email);

            if (GetUserID == "")
            {
                return NotFound("User Not found");
            }


            var data = await _ListServices.GetUserlists(GetUserID);

            if (data == null)
            {

                return NotFound("No lists found");


            }

            var lists = data.Select(data => _mapper.Map<GetListDTO>(data));

            return Ok(lists);
        }


        //Create a list

        [HttpPost("CreateList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> CreateList([FromBody] ListsDTO Data)
        {

            if (Data == null)
            {
                return BadRequest("No data to creat a List");
            }

            string GetUserID = await _ListServices.GetUserId(Data.email);

            if (GetUserID == "")
            {
                return NotFound("User Not found");
            }


            await _ListServices.InsertlIST(GetUserID, Data);

            return Ok();
        }


        //Delete a list

        [HttpDelete("{Id:int}",  Name = "DeleteList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteList( int Id )
        {

            var CheckId = await _ListServices.GetAlist(Id);



            if (CheckId == null)
            {
                return NotFound("This list doenot exist ");
            }


            await _ListServices.DeleteList(CheckId);

            

            return Ok();
        }

        //Update a list

        [HttpPut("UpdateList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> UpdateList([FromBody] UpdateListDTO List )
        {

            var getListData = await _ListServices.GetAlist(List.id);


            if (getListData == null)
            {
                return NotFound("This list doenot exist ");
            }

            getListData.name = List.name;


            await _ListServices.UpdateList(getListData);

            return Ok();
        }

    }
}
