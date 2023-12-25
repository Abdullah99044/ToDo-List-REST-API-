﻿ 
using Model.Models;
using TodoList.Model.DTO.ListsDTO;
 

namespace TodoList.Model.Services
{
    public interface IListsServices
    {

        //Get  all user lists
        public Task<List<GetListDTO>> GetUserLists(string UserId, string? listName,
                   bool? sortingByDateTime, int page);



        //Get the user id for to create a list or get his lists
        public Task<string> GetUserId(string Email);

        
 
        //Get  list data through the id
        public Task<Lists> GetAlist(int Id);

        //Post a List to the data base 
        public Task InsertlIST(string Id, ListsDTO Data);
        //Delete a list 

        public Task DeleteList(Lists entity);

        //Update a list
        public Task UpdateList(Lists entity);

        



    }
}