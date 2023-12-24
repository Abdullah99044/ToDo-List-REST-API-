using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TodoList.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Services.ToDoListsServices
{
    public class ToDoListServices : ITodoListServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private readonly IMemoryCache _memoryCache;


        public ToDoListServices(ApplicationDbContext db, IMapper mapper , IMemoryCache memoryCache)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoryCache;

        }


        public async Task<IEnumerable<TodoListDTO>> GetAllTodoLists(int Id, string? filetrting)
        {
            //How does the caching works?

            //The GetAllTodoLists() checks the requested data inside inmemorey cache

            //If cache hit(data exist) then it returns the data from the cache memorey and filter it if filtring is not empty.

            //If cache miss(data doesn't exist ) then it request the data from the DataBase and filter it if filtring is not empty.

            //the Save() method delete the data in the memory cache if it was changed, removed or added to in the dataBase



            //Check if the data inside the inmemeorey cache 

           string cacheKey = $"Key_ListId_{Id}";


            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<TodoListDTO> cache))
            {


                //Filter the data if filtring is not null


                if (!string.IsNullOrEmpty(filetrting))
                {
                    return cache.Where(u => u.Priority == filetrting);

                }
                else
                {
                    return cache;
                }
                
            }

            var queryAll = _db.TodoLists.AsNoTracking().Where(u => u.ListId == Id);

            //Filter the data if filtring is not null

            if (!string.IsNullOrEmpty(filetrting))
            {
                queryAll.Where(u => u.Priority == filetrting);

            }


            var userTodoLists = await queryAll.Select(TodoList => _mapper.Map<TodoListDTO>(TodoList)).ToListAsync();

            //Cache the data

            var cacheOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _memoryCache.Set(cacheKey, userTodoLists, cacheOptions);

            return userTodoLists;
          

        }


        //Save changes after every action in the database
        public async Task<bool> Save(int ListId)
        {

            var saved = await _db.SaveChangesAsync();

            if(saved > 0)
            {

                //Remove the cached data when it's updated
                _memoryCache.Remove($"Key_ListId_{ListId}");
                return true;
            }

            return false;
        }

        public async Task<Lists> GetListById(int Id)
        {


            return await _db.Lists.FindAsync(Id);

               
        }

        public async Task<bool> InsertTodoList(int Id, CreateToDoListDTO Data)
        {

            var ToDoListData =  new TodoListsDTO();

            ToDoListData.Name = Data.Name;
            ToDoListData.Priority = Data.Priority;
            ToDoListData.ListId = Id;
            ToDoListData.CreatedAt = DateTime.Now;
            ToDoListData.UpdatedAt = DateTime.Now;

            await _db.TodoLists.AddAsync(ToDoListData);

            return await Save(ToDoListData.ListId);
        }

      
        public async Task<TodoListsDTO> GetTodoListById(int Id)
        {
            return await _db.TodoLists.FindAsync(Id);
        }

        public async Task<bool> UpdateTodoList(TodoListsDTO Data)
        {
            _db.Update(Data);
            
            return await Save(Data.ListId);
        }

        public async Task<bool> DeleteTodoList(TodoListsDTO Data)
        {
            _db.Remove(Data);
            return await Save(Data.ListId);
        }
    }
}
