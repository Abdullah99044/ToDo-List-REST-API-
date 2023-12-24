using AutoMapper;
using DataAccess.Migrations;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.TodoTasksDTO;
using TodoList.Data;

namespace TodoList.Services.TodoTasksService
{
    public class TodoTasksService : ITodoTasksService
    {

        public readonly ApplicationDbContext _db;

        public readonly IMemoryCache _memoryCache;

        public readonly IMapper _mapper;
        public TodoTasksService(ApplicationDbContext  db, IMapper mapper   , IMemoryCache memoreyCache)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoreyCache;
        }


      

       
        public async Task<TodoList1> checkTodoListId(int id)
        {


            //Cache the todolist data 


            var cacheKey = $"TodoListID_{id}";

            if (_memoryCache.TryGetValue(cacheKey, out TodoList1 cachedTodoList ))
            {
                return cachedTodoList;
            }

            var todoList = await _db.TodoList.FindAsync(id);

            if(todoList == null)
            {
                return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _memoryCache.Set(cacheKey, todoList, cacheOptions);

            return todoList ;
        }



        public async Task<TodoTasks> checkTodoTasksId(int Id)
        {


            //Cache the todoTasks data 

            var cacheKey = $"TodoTasksID_{Id}";

            if (_memoryCache.TryGetValue(cacheKey, out TodoTasks cachedTodoTasks))
            {
                return cachedTodoTasks;
            }

            var todoTask = await _db.TodoTasks.FindAsync(Id);

            if (todoTask == null)
            {
                return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _memoryCache.Set(cacheKey, todoTask, cacheOptions);

            return todoTask;
            
        }


        //How does the cache works in todolistServices ?

        //When a user request a TodoTask or TodoList data , the app stored in in memorey cache

        //If the user updated the todotasks the cached data of All todod lists will removed so the user can see the updated data

        //If the user remove a todoList the cached data of  todod lists will removed so he can request new data for other function in this class


        public async Task<bool> save(int ListId , int TodoTasksID  )
        {

            var save = await _db.SaveChangesAsync() ;

            if (save > 0 )
            {

                //Update cached data 

                _memoryCache.Remove($"Key_ListId_{ListId}") ;
                _memoryCache.Remove($"TodoTasksID_{TodoTasksID}");
               

                return true;
            }

            return false;
        }

        public async Task<bool> insertTodoTask(int todoListId, CreateTodoTasksDTO entity, int ListId)
        {

            var todoTask = _mapper.Map<TodoTasks>(entity);

            todoTask.Created = DateTime.Now;
            todoTask.Updated = DateTime.Now;
            todoTask.status = "Not finished";
            todoTask.todoListId = todoListId;
            

            await _db.TodoTasks.AddAsync(todoTask  );

            return await save(ListId , todoTask.Id );
        }

        public async Task<bool> updateTodoTask(TodoTasks entity , int ListId)
        {

           _db.TodoTasks.Update(entity);

            return await save(ListId , entity.Id  );
        }

        public async Task<bool> deleteTodoTask(TodoTasks TodoTask, int ListId)
        {
            

            _db.TodoTasks.Remove(TodoTask);

            return  await save(ListId , TodoTask.Id  );
        }
    }
}
