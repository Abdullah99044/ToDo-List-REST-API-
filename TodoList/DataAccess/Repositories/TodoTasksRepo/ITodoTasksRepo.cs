using Model.Models;
 

namespace TodoList.DataAccess.Repositories.TodoTasksRepo
{
    public interface ITodoTasksRepo
    {
        //Get a todolist from the data base 
        public Task<TodoList1> getTodoList (int id);

        //Get a todoTask from the data base 
        Task<TodoTasks> getTodoTasks(int Id);
        
        //Save changes in the database
        public Task<bool> save();

        //Post a todo task to the data base 
        public Task<bool> insertTodoTask(int todoListId , TodoTasks entity );

        //Update a todo task to the data base 
        public Task<bool>  updateTodoTask(TodoTasks TodoTask );

        //Delete a todo task from the data base 
        public Task<bool>  deleteTodoTask(TodoTasks TodoTask );
    }
}
