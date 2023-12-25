using AutoMapper;
using Azure.Core;
using DataAccess.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using Model.Models.DTO.TodoTasksDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using TodoList.Data;
using TodoList.MiddleWare;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Services.ToDoListsServices
{


    //Cache the rquestd list data nad todolist data
    public class ToDoListServices : ITodoListServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private readonly MyMemoryCache _memoryCache;


        public ToDoListServices(ApplicationDbContext db, IMapper mapper , MyMemoryCache memoryCache)
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


        //Get all todo lists

        public async Task<IEnumerable<TodoListDTO>> GetAllTodoLists(int Id )
        {
            //How does the caching works?

            //The GetAllTodoLists() checks the requested data inside inmemorey cache

            //If cache hit(data exist) then it returns the data from the cache memorey and filter it if filtring is not empty.

            //If cache miss(data doesn't exist ) then it request the data from the DataBase and filter it if filtring is not empty.

            //the Save() method delete the data in the memory cache if it was changed, removed or added to in the dataBase




            //Cache key for storing the cache

            string cacheKey = $"Key_ListId_{Id}";


            //Check if the data inside the inmemeorey cache 

            if (_memoryCache.Cache.TryGetValue(cacheKey, out IEnumerable<TodoListDTO> cache))
            {

                return cache;
                       
            }


            // Execute the query and retrieve all data

            var queryAll =  _db.TodoList.AsNoTracking().Where(u => u.ListId == Id).Include(tl => tl.TodoTasks);


            //Mapping the data with a mapper to Json serialized error doesn't occur
            var TodoList = await queryAll.ToListAsync();

            IEnumerable<TodoListDTO> userTodoLits = TodoList.Select(TodoList => new TodoListDTO()
            {

                Id = TodoList.Id ,
                Name = TodoList.Name ,
                tottalTodoLists = TodoList.tottalTodoLists ,
                finishedTodoLists = TodoList.finishedTodoLists ,
                color = TodoList.color ,
                TodoTasks = TodoList.TodoTasks.Select(todoTask => new TodoTasks 
                {

                    Id = todoTask.Id,
                    Name = todoTask.Name,
                    status = todoTask.status  ,
                    Updated = todoTask.Updated  ,
                    Created = todoTask.Created ,
                    todoListId= 0 

                }).ToList()
            });

            // Cache the data

            cacheData(cacheKey, userTodoLits);

            return userTodoLits;


        }



        //Get a List
        public async Task<Lists> GetListById(int ListId)
        {

            return await _db.Lists.FindAsync(ListId);

        }


        //Get a Todo list
        public async Task<TodoList1> GetTodoListById(int Id)
        {

            return await _db.TodoList.FindAsync(Id);

        }

        //Save changes after every action in the database
        public async Task<bool> Save(int ListId)
        {

            var saved = await _db.SaveChangesAsync();

            if(saved > 0)
            {
                string cacheKey = $"Key_ListId_{ListId}";

                _memoryCache.Cache.Remove(cacheKey);
                return true;
            }

            return false;
        }


        //Post a todo list to the data base 
        public async Task<bool> InsertTodoList(int Id, CreateToDoListDTO Data)
        {

            var ToDoListData =  new TodoList1();

            ToDoListData.Name = Data.Name;
            ToDoListData.color = Data.color;
            ToDoListData.ListId = Id;
            ToDoListData.finishedTodoLists = 0;
            ToDoListData.tottalTodoLists = 0;
            ToDoListData.CreatedAt = DateTime.Now;
            ToDoListData.UpdatedAt = DateTime.Now;

            await _db.TodoList.AddAsync(ToDoListData);

            return await Save(ToDoListData.ListId);
        }



        //Update a todo list to the data base 
        public async Task<bool> UpdateTodoList(TodoList1 Data)
        {
            _db.Update(Data);
            
            return await Save(Data.ListId);
        }

        //Delete a todo list from the data base 
        public async Task<bool> DeleteTodoList(TodoList1 Data)
        {
 

            _db.Remove(Data);
            return await Save(Data.ListId);
        }
    }
}
