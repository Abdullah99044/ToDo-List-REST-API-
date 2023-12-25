 
using Model.Models;
 

namespace TodoList.DataAccess.Repositories
{
    public interface IListsRepo
    {

        //Get  all user lists
        public IQueryable<Lists> GetUserLists(string UserId);

        //Get the user id for to create a list or get his lists
        public Task<string> GetUserId(string Email);

        public Task<bool> Save(); 

        //Get  list data through the id
        public Task<Lists> GetAlist(int Id);

        //Post a List to the data base 
        public Task<bool> InsertlIST(string Id, Lists Data);
        //Delete a list 

        public Task<bool> DeleteList(Lists entity);

        //Update a list
        public Task<bool> UpdateList(Lists entity);

        



    }
}
