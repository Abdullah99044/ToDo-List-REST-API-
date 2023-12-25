using AutoMapper;
using Model.Models;
using Todolist.Model.DTO.TodoTasksDTO;
using TodoList.Data;
using TodoList.DataAccess.Repositories.TodoTasksRepo;


namespace TodoList.Model.Services.TodoTasksService
{
    public class TodoTasksService : ITodoTasksService
    {

        public readonly ApplicationDbContext _db;

        public readonly MyMemoryCache _memoryCache;

        public readonly IMapper _mapper;

        public readonly ITodoTasksRepo _repository ;
        public TodoTasksService(ApplicationDbContext  db, IMapper mapper   , MyMemoryCache memoreyCache , ITodoTasksRepo repository)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoreyCache;
            _repository = repository;
        }


        //Get a todolist from the data base 

        public async Task<TodoList1> getTodoList(int id)
        {

            return await _repository.getTodoList(id); 
        }


        //Get a todoTask from the data base 

        public async Task<TodoTasks> getTodoTasks(int Id)
        {

            return await _repository.getTodoTasks(Id);

        }


        //Save changes in the database

        public async Task<bool> removeCachedData(int ListId , bool isDataSaved)
        { 

            if (isDataSaved == true)
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

            var saveData = await  _repository.insertTodoTask(todoListId, todoTask);

            return await removeCachedData(ListId , saveData );
        }




        //Update a todo task to the data base 

        public async Task<bool> updateTodoTask(TodoTasks entity , int ListId)
        {

            var saveData  = await _repository.updateTodoTask(entity);

            return await removeCachedData(ListId  , saveData);
        }


        //Delete a todo task from the data base 

        public async Task<bool> deleteTodoTask(TodoTasks TodoTask, int ListId)
        {


            var saveData = await _repository.deleteTodoTask(TodoTask);

            return  await removeCachedData(ListId , saveData);
        }
    }
}
