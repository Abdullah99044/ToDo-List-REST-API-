using Model.Models;
using Model.Models.DTO.ToDoListsDTO;

namespace TodoList.Services.ToDoListsServices
{
    public interface ITodoListServices
    {

        public Task insertToDoList(CreateToDoListDTO Data, int Id);

        public Task<Lists> checkList(int Id);

    }
}
