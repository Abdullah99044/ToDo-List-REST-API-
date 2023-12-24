


using AutoMapper;
using DataAccess.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TodoList.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Services
{
    public class ListServices  : IListsServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private readonly IMemoryCache _memoryCache;
        public ListServices(ApplicationDbContext db, IMapper mapper , IMemoryCache memoryCache) 
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoryCache;
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

        public List<GetListDTO> GetUserLists(string UserId, string? listName, bool? sortingByLetters, int page  )
        {


            //How does the caching works?

            //The GetUserLists() checks the requested  data ( pageResponse ) inside inmemorey cache

            //If cache hit(data exist) then it returns the data from the cache memorey and making copy to filter it if filtring is not empty.

            //And sorting it if sorting  is true 

            //If cache miss(data doesn't exist ) then it request the data from the DataBase and filter it if filtring is not empty.

            //And sorting it if sorting  is true 


            //the Save() method delete the data in the memory cache if it was changed, removed or added to in the dataBase


            var cacheKey = $"Key_{UserId}";

            // Check if data is in the in-memory cache
            if (_memoryCache.TryGetValue(cacheKey, out List<GetListDTO> cachedResponse))
            {
                // Make a deep copy of the cached response
                var filteredResponse = cachedResponse;


                filteredResponse = ApplyFiltering(  filteredResponse, listName);

                filteredResponse =  ApplySorting(  filteredResponse, sortingByLetters);

                return filteredResponse;
            }

            IQueryable<Lists> query = _db.Lists.Where(u => u.UserId == UserId);

            // Execute the query and retrieve all data
            var allData = query.Select(userList => _mapper.Map<GetListDTO>(userList)).ToList();

            // Cache the data
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _memoryCache.Set(cacheKey, allData, cacheOptions);


            var filteerdData = ApplyFiltering(allData, listName);
                filteerdData = ApplySorting(allData, sortingByLetters);

            return filteerdData;
        }

        // Applying filtering  

        private  List<GetListDTO> ApplyFiltering(   List<GetListDTO>  ListDTO , string? listName)
        {
            if (string.IsNullOrWhiteSpace(listName))
            {
                return ListDTO;
            }
            return  ListDTO.Where(u => u.name.Contains(listName)).ToList();
          
        }

        // Applying sorting   
        private List<GetListDTO>  ApplySorting(List<GetListDTO>  ListDTO, bool? sortingByLetters)
        {
            if ( sortingByLetters != true )
            {
                return ListDTO;
            }

            return ListDTO =  ListDTO.OrderBy(p => p.name).ToList();
            
        }

     

        //Save changes after every action in the database
        public async Task<bool> Save( string email )
        {

            var saved = await _db.SaveChangesAsync();

            if (saved > 0)
            {

                //Remove the cached data when it's updated
                _memoryCache.Remove($"Key_{email}");
                return true;
            }

            return false;
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
             await Save(entity.UserId);
        }

        //Update a list

        public async Task UpdateList(Lists entity)
        {
            _db.Lists.Update(entity);
            await Save(entity.UserId);
        }

       
    }
}
