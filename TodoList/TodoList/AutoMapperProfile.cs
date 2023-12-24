using AutoMapper;
using Model.Models;
using Model.Models.DTO.ListsDTO;
using Model.Models.DTO.ToDoListsDTO;

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


            CreateMap<CreateToDoListDTO , TodoListDTO>();

            CreateMap<TodoListDTO , CreateToDoListDTO>();


            CreateMap<TodoListDTO , TodoListsDTO>();

            CreateMap<TodoListsDTO, TodoListDTO>();


        }
    }
}
