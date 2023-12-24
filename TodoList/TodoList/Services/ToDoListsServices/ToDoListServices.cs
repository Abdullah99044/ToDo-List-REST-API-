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
using TodoList.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList.Services.ToDoListsServices
{


    //Cache the rquestd list data nad todolist data
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


        public async Task<IEnumerable<TodoListDTO>> GetAllTodoLists(int Id )
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

  
                //return cache;
                
                
            }

            var queryAll =    _db.TodoList.AsNoTracking().Where(u => u.ListId == Id).Include(tl => tl.TodoTasks);
 
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

            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _memoryCache.Set(cacheKey, userTodoLits, cacheOptions);


           

            return userTodoLits;


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

      
        public async Task<TodoList1> GetTodoListById(int Id)
        {

            //Cache the todolist data 


            var cacheKey = $"TodoListID_{Id}";

            if (_memoryCache.TryGetValue(cacheKey, out TodoList1 cachedTodoList))
            {
                return cachedTodoList;
            }

            var todoList = await _db.TodoList.FindAsync(Id);

            if (todoList == null)
            {
                return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _memoryCache.Set(cacheKey, todoList, cacheOptions);

            return todoList; 
           
        }

        public async Task<bool> UpdateTodoList(TodoList1 Data)
        {
            _db.Update(Data);
            
            return await Save(Data.ListId);
        }

        public async Task<bool> DeleteTodoList(TodoList1 Data)
        {


            //The app stores the the todolist data in sevral function , when it's removed it will also be removed from the cache data
            _memoryCache.Remove($"TodoListID_{Data.Id}");


            _db.Remove(Data);
            return await Save(Data.ListId);
        }
    }
}
