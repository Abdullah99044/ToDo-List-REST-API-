 
 
 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using TodoList.Models.DTO.ToDoListsDTO;
using TodoList.Data;
using TodoList.Services.ToDoListsServices;
using TodoList.DataAccess.Repositories.TodoListRepo;

namespace TodoList.Model.Services.ToDoListsServices
{


    //Cache the rquestd list data nad todolist data
    public class ToDoListServices : ITodoListServices
    {

        private readonly ITodoListRepo _repository;

      

        private readonly MyMemoryCache _memoryCache;


        public ToDoListServices(ITodoListRepo repository ,   MyMemoryCache memoryCache)
        {
            _repository = repository;
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

            var TodoList = await _repository.GetAllTodoLists(Id);

            //Mapping the data without a mapper so the Json serialized error doesn't occur


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

            return await _repository.GetListById(ListId);

        }


        //Get a Todo list
        public async Task<TodoList1> GetTodoListById(int Id)
        {

            return await _repository.GetTodoListById(Id);

        }

        //Save changes after every action in the database
        public async Task<bool> removeCachedData(int ListId , bool isDataSavedSuccissfully )
        {

            if(isDataSavedSuccissfully == true)
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

            var saveData = await _repository.InsertTodoList(ToDoListData);

            return await removeCachedData(ToDoListData.ListId , saveData);
        }



        //Update a todo list to the data base 
        public async Task<bool> UpdateTodoList(TodoList1 Data)
        {
            var saveData =  await _repository.UpdateTodoList(Data);
            
            return await removeCachedData(Data.ListId , saveData);
        }

        //Delete a todo list from the data base 
        public async Task<bool> DeleteTodoList(TodoList1 Data)
        {
            var saveData =  await _repository.DeleteTodoList(Data);
            return await removeCachedData(Data.ListId , saveData);
        }
    }
}
