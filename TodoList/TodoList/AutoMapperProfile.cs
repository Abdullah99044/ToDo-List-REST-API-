using AutoMapper;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;
using Model.Models.DTO.TodoTasksDTO;

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
