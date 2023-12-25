using AutoMapper;
using Model.Models;
using Todolist.Model.DTO.TodoTasksDTO;
using TodoList.Model.DTO.ListsDTO;
using TodoList.Models.DTO.ToDoListsDTO;

namespace TodoList
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<Lists, ListsDTO >();

            CreateMap< ListsDTO , Lists>();


            CreateMap<GetListDTO, Lists>();

            CreateMap< Lists , GetListDTO  > ();


            CreateMap<CreateToDoListDTO , TodoList1>();

            CreateMap<TodoList1 , CreateToDoListDTO>();


            CreateMap<TodoListDTO , TodoList1>();

            CreateMap<TodoList1, TodoListDTO>();

            CreateMap<TodoTasks , CreateTodoTasksDTO>();

            CreateMap<CreateTodoTasksDTO, TodoTasks>();


        }
    }
}
