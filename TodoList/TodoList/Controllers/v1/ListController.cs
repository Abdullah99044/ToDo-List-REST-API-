using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using System.Linq;
using System.Net;
using TodoList.Data;
using TodoList.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Lists")]
    [ApiController]
    [ApiVersion("1.0")]
    [EnableRateLimiting("FixedWindow")]
    [Authorize]

    public class ListController : ControllerBase
    {


        private readonly ApplicationDbContext _db;
        private readonly ILogger<ListController> _logger;
     

        private IListsServices _ListServices;
        public ListController(ApplicationDbContext db, ILogger<ListController> logger ,   IListsServices ListServices)
        {
            _db = db;
            _logger = logger;
            _ListServices = ListServices;

        }


        //Get all lists

        [HttpGet("{email}" )]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        


        public async Task<ActionResult<pageResponse>> GetLists( string email, [FromQuery] string? listName , 
            [FromQuery] bool? sortingByLetters ,  [FromQuery] string  inputPage)
        {


            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("In ListController in GetLists : Error 400 No data to get your Lists ");
                return BadRequest("No data to get your Lists");
            }

            if (inputPage == null  )
            {
                _logger.LogError("In ListController in GetLists : Error 400 Invalid page ");
                return BadRequest("Invalid page");
            }

            var page = int.Parse(inputPage);

            var getUserId = await _ListServices.GetUserId(email);

            if (string.IsNullOrEmpty(getUserId))
            {
                _logger.LogError("In ListController in GetLists : Error 404 User not found ");
                return NotFound("User not found");
            }

            var userLists = _ListServices.GetUserLists(  getUserId, listName ,
                                                        sortingByLetters ,  page  );

            var pageResult = 5;
            var totalCount = userLists.Count;
            var pageCount = (int)Math.Ceiling(totalCount / (double)pageResult);
            userLists = userLists.Skip((page - 1) * pageResult).Take(pageResult).ToList();
            

            var response = new pageResponse()
            {
                lists = userLists,
                currentPage = page,
                pages = pageCount
            };


            _logger.LogInformation("In ListController in GetLists : 200 Ok respons ");
            return Ok(response);

        }
       


        //Create a list

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
     
        public async Task<IActionResult> CreateList([FromBody] ListsDTO Data)
        {

            if (Data == null)
            {
                _logger.LogError("In ListController in CreateList  : Error 400  No data to creat a List  ");
                return BadRequest("No data to creat a List");
            }

            // Encode the input to prevent XSS

            Data.description = WebUtility.HtmlEncode(Data.description);
            Data.name = WebUtility.HtmlEncode(Data.name);


            string GetUserID = await _ListServices.GetUserId(Data.email);

            if (GetUserID == "")
            {
                _logger.LogError("In ListController in CreateList  : Error 404  User Not found  ");
                return NotFound("User Not found");
            }


            await _ListServices.InsertlIST(GetUserID, Data);

            _logger.LogInformation("In ListController in CreateList : 200 Ok respons ");

            return Ok();
        }


        //Delete a list

        [HttpDelete("{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteList(int Id)
        {

            var CheckId = await _ListServices.GetAlist(Id);



            if (CheckId == null)
            {
                _logger.LogError("In ListController in DeleteList  : This list does not exist ");
                return NotFound("This list does not exist ");
            }


            await _ListServices.DeleteList(CheckId);


            _logger.LogInformation("In ListController in DeleteList : 200 Ok respons ");

            return Ok();
        }

        //Update a list

        [HttpPut()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        
        public async Task<IActionResult> UpdateList([FromBody] UpdateListDTO List)
        {

            if(List == null)
            {
                _logger.LogError("In ListController in UpdateList : Error 400  No data to update a List  ");
                return BadRequest("No data to update a List");
            }

            // Encode the input to prevent XSS

            List.name = WebUtility.HtmlEncode(List.name);


            //Get list data to update it

            var getListData = await _ListServices.GetAlist(List.id);


            if (getListData == null)
            {
                _logger.LogError("In ListController in UpdateList  : This list does not exist ");
                return NotFound("This list doenot exist ");
            }

            getListData.name = List.name;
            getListData.UpdatedAt = DateTime.Now;


            await _ListServices.UpdateList(getListData);

            _logger.LogInformation("In ListController in UpdateList : 200 Ok respons ");

            return Ok();
        }

    }
}
