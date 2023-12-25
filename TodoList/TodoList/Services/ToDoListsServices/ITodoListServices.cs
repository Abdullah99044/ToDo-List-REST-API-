using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;

namespace TodoList.Services.ToDoListsServices
{
    public interface ITodoListServices
    {


        //Get all todo lists
        public Task<IEnumerable<TodoListDTO>> GetAllTodoLists(int Id);

        //Get a List
        public Task<Lists> GetListById(int Id);

        //Get a Todo list
        public Task<TodoList1> GetTodoListById(int Id);

        //Save changes in the database
        public Task<bool> Save(int ListId);

        //Post a todo list to the data base 
        public Task<bool> InsertTodoList(int Id , CreateToDoListDTO Data);
      
        //Update a todo list to the data base 
        public Task<bool> UpdateTodoList(TodoList1 Data);

        //Delete a todo list from the data base 
        public Task<bool> DeleteTodoList(TodoList1 Data);


    }
}
