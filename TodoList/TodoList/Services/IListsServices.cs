using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Model.Models.DTO.ListsDTO;

namespace TodoList.Services
{
    public interface IListsServices
    {

        //Post a List to the data base 
        public Task InsertlIST(string Id , ListsDTO Data);


        //Get the user id for to create a list or get his lists

        public Task<string> GetUserId(string Email);

        //Get  all user lists

        public List<GetListDTO> GetUserLists(string UserId, string? listName,
                   bool? sortingByDateTime , int page  );

 

        //Get  list data through the id

        public Task<Lists> GetAlist(int Id);

        //Delete a list 

        public Task DeleteList(Lists entity);

        //Update a list
        public Task UpdateList(Lists entity);

        



    }
}
