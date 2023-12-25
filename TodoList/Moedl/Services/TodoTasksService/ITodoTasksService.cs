using Model.Models;
using Todolist.Model.DTO.TodoTasksDTO;

namespace TodoList.Model.Services.TodoTasksService
{
    public interface ITodoTasksService
    {
        //Get a todolist from the data base 
        public Task<TodoList1> getTodoList (int id);

        //Get a todoTask from the data base 
        Task<TodoTasks> getTodoTasks(int Id);
        
        //Save changes in the database
        public Task<bool> removeCachedData(int ListId , bool isDataSaved);

        //Post a todo task to the data base 
        public Task<bool> insertTodoTask(int todoListId , CreateTodoTasksDTO entity , int ListId);

        //Update a todo task to the data base 
        public Task<bool>  updateTodoTask(TodoTasks TodoTask, int ListId);

        //Delete a todo task from the data base 
        public Task<bool>  deleteTodoTask(TodoTasks TodoTask, int ListId);
    }
}
