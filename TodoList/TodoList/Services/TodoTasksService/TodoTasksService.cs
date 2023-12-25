using AutoMapper;
using DataAccess.Migrations;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.TodoTasksDTO;
using TodoList.Data;
using TodoList.MiddleWare;

namespace TodoList.Services.TodoTasksService
{
    public class TodoTasksService : ITodoTasksService
    {

        public readonly ApplicationDbContext _db;

        public readonly MyMemoryCache _memoryCache;

        public readonly IMapper _mapper;
        public TodoTasksService(ApplicationDbContext  db, IMapper mapper   , MyMemoryCache memoreyCache)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoreyCache;
        }


        //Get a todolist from the data base 

        public async Task<TodoList1> getTodoList(int id)
        {

            return await _db.TodoList.FindAsync(id); 
        }


        //Get a todoTask from the data base 

        public async Task<TodoTasks> getTodoTasks(int Id)
        {

            return await _db.TodoTasks.FindAsync(Id);
        }


        //Save changes in the database

        public async Task<bool> save(int ListId , int TodoTasksID  )
        {

            var save = await _db.SaveChangesAsync() ;

            if (save > 0 )
            {

                string cacheKey = $"Key_ListId_{ListId}";

                _memoryCache.Cache.Remove(cacheKey);
                return true;
            }

            return false;
        }




        //Post a todo task into the data base 

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




        //Update a todo task to the data base 

        public async Task<bool> updateTodoTask(TodoTasks entity , int ListId)
        {

           _db.TodoTasks.Update(entity);

            return await save(ListId , entity.Id  );
        }


        //Delete a todo task from the data base 

        public async Task<bool> deleteTodoTask(TodoTasks TodoTask, int ListId)
        {
            

            _db.TodoTasks.Remove(TodoTask);

            return  await save(ListId , TodoTask.Id  );
        }
    }
}
