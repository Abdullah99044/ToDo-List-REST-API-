using Model.Models;
using Model.Models.DTO.TodoTasksDTO;

namespace TodoList.Services.TodoTasksService
{
    public interface ITodoTasksService
    {

        public Task<TodoList1> checkTodoListId (int id);

        Task<TodoTasks> checkTodoTasksId(int Id);

        public Task<bool> save(int ListId , int TodoTasksID  );

        public Task<bool> insertTodoTask(int todoListId , CreateTodoTasksDTO entity , int ListId);

        public Task<bool>  updateTodoTask(TodoTasks TodoTask, int ListId);

        public Task<bool>  deleteTodoTask(TodoTasks TodoTask, int ListId);
    }
}
