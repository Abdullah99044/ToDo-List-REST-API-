


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
using TodoList.MiddleWare;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Services
{
    public class ListServices  : IListsServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private readonly MyMemoryCache _memoryCache;
        public ListServices(ApplicationDbContext db, IMapper mapper , MyMemoryCache memoryCache) 
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        //Store data in the cache memory
        private void cacheData<T>(string cacheKey, T cachedData)
        {
            //Set the size and the Expired time
            var cacheOptions = new MemoryCacheEntryOptions()
                                    .SetSize(1)
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Cache.Set(cacheKey, cachedData, cacheOptions);

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


            //Cache key for storing the cache
            var cacheKey = $"Key_{UserId}";

            // Check if data is in the in-memory cache
            if (_memoryCache.Cache.TryGetValue(cacheKey, out List<GetListDTO> cachedResponse))
            {
                // Make a deep copy of the cached response to filter and sort it with out effecting the sotred cahced data
                var filteredResponse = cachedResponse;

                //Filter and sort the cahced data
                filteredResponse = ApplyFiltering(  filteredResponse, listName);

                filteredResponse =  ApplySorting(  filteredResponse, sortingByLetters);

                return filteredResponse;
            }

            IQueryable<Lists> query = _db.Lists.Where(u => u.UserId == UserId);

            // Execute the query and retrieve all data
            var allData = query.Select(userList => _mapper.Map<GetListDTO>(userList)).ToList();

            // Cache the data
            cacheData(cacheKey, allData);

            //Filter and sort the requested data

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


        //Get the user id for to create a list or get his lists

        public async Task<string> GetUserId(string Email)
        {
            var data = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email);

            if (data == null)
            {
                return null;
            }

            return data.Id;
        }


        //Get list data with its id
        public async Task<Lists> GetAlist(int ListId)
        {
            return await _db.Lists.FindAsync(ListId);

        }

        //Save changes after every action in the database
        public async Task<bool> Save( string userID )
        {

            var saved = await _db.SaveChangesAsync();

            if (saved > 0)
            {

                //Remove the cached data when it's updated
                _memoryCache.Cache.Remove($"Key_{userID}");
                return true;
            }

            return false;
        }

       

        //Post a List into the data base 

        public async Task InsertlIST(string Id, ListsDTO Data)
        {
            //Mapping  
            var List = _mapper.Map<Lists>(Data);

            List.CreatedAt = DateTime.Now;
            List.UpdatedAt = DateTime.Now;
            List.UserId = Id;

            await _db.Lists.AddAsync(List);
            await Save(List.UserId);

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
