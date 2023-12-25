using Model.Models;
 

namespace TodoList.DataAccess.Repositories.TodoListRepo
{
    public interface ITodoListRepo
    {


        //Get all todo lists
        public Task<IEnumerable<TodoList1>> GetAllTodoLists(int Id);

        //Get a List
        public Task<Lists> GetListById(int Id);

        //Get a Todo list
        public Task<TodoList1> GetTodoListById(int Id);

        //Save changes in the database
        public Task<bool> Save();

        //Post a todo list to the data base 
        public Task<bool> InsertTodoList( TodoList1 Data);
      
        //Update a todo list to the data base 
        public Task<bool> UpdateTodoList(TodoList1 Data);

        //Delete a todo list from the data base 
        public Task<bool> DeleteTodoList(TodoList1 Data);


    }
}
