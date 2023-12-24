using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;

namespace TodoList.Services.ToDoListsServices
{
    public interface ITodoListServices
    {


        public Task<bool> Save(int ListId);

        public Task<bool> InsertTodoList(int Id , CreateToDoListDTO Data);

        public Task<Lists> GetListById(int Id);

        public Task<IEnumerable<TodoListDTO>> GetAllTodoLists(int Id );

        public Task<TodoList1> GetTodoListById(int Id);

        public Task<bool> UpdateTodoList(TodoList1 Data);

        public Task<bool> DeleteTodoList(TodoList1 Data);


    }
}
