


using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using TodoList.Data;

namespace TodoList.Services
{
    public class ListServices  : IListsServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;
        public ListServices(ApplicationDbContext db, IMapper mapper) 
        {
            _db = db;
            _mapper = mapper;
        }


        //Post a List to the data base 


        public async Task InsertlIST( string Id  , ListsDTO Data)
        {

            var List = _mapper.Map<Lists>(Data);

            List.CreatedAt = DateTime.Now;
            List.UpdatedAt = DateTime.Now;
            List.UserId = Id;

            await _db.Lists.AddAsync(List);
            await _db.SaveChangesAsync();

        }


        //Get the user id for to create a list or get his lists

        public async Task<string> GetUserId(string Email)
        {
            var data = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email);

            if (data == null)
            {
                return "";
            }

            return data.Id;
        }



        //Get  all user lists

        public async Task<IEnumerable<Lists>> GetUserlists(string Id)
        {

            return await _db.Lists.AsNoTracking().Where(u => u.UserId == Id).ToListAsync();

        }

        //Get list data through the id


        public async Task<Lists> GetAlist(int Id)
        {

            return await _db.Lists.FindAsync(Id);

        }

        //Delete a list 


        public async Task DeleteList(Lists entity)
        {
            _db.Set<Lists>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        //Update a list

        public async Task UpdateList(Lists entity)
        {
            _db.Lists.Update(entity);
            await _db.SaveChangesAsync();
        }

    }
}
