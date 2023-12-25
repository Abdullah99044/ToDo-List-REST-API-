 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
 
using TodoList.Data;

namespace TodoList.DataAccess.Repositories
{
    public class ListRepo  : IListsRepo
    {

        private readonly ApplicationDbContext _db;

        
        public ListRepo(ApplicationDbContext db) 
        {
            _db = db;
             
        }

        //Get  all user lists

        public  IQueryable<Lists> GetUserLists(string UserId )
        {

            return   _db.Lists.Where(u => u.UserId == UserId) ;
 
        }


        //Get the user id for to create a list or get his lists

        public async Task<string> GetUserId(string Email)
        {
            var user  =  await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }


        //Get list data with its id
        public async Task<Lists> GetAlist(int ListId)
        {
            return await _db.Lists.FindAsync(ListId);

        }

        //Save changes after every action in the database
        public async Task<bool> Save()
        {

            var saved = await _db.SaveChangesAsync();

            if (saved > 0)
            {
                return true;
            }

            return false;
        }

       

        //Post a List into the data base 

        public async Task<bool> InsertlIST(string Id, Lists entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.UserId = Id;

            await _db.Lists.AddAsync(entity);
            return await Save();

        }


        //Delete a list 
        public async Task<bool> DeleteList(Lists entity)
        {   
             _db.Set<Lists>().Remove(entity);
             return await Save();
        }


        //Update a list
        public async Task<bool> UpdateList(Lists entity)
        {
            _db.Lists.Update(entity);
            return await Save();
        }

       
    }
}
