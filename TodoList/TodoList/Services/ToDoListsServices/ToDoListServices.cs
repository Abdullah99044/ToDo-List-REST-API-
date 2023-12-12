using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using TodoList.Data;

namespace TodoList.Services.ToDoListsServices
{
    public class ToDoListServices : ITodoListServices
    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

     
        public ToDoListServices(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
           

        }

        public async Task<Lists> checkList(int Id)
        {

           return await _db.Lists.FindAsync(Id);

        }

        public async Task insertToDoList(CreateToDoListDTO Data , int Id)
        {

            var ToDoList = _mapper.Map<TodoLists>(Data);

            ToDoList.CreatedAt = DateTime.Now;
            ToDoList.UpdatedAt = DateTime.Now;
            ToDoList.ListId = Id;

            await _db.TodoLists.AddAsync(ToDoList);
            await _db.SaveChangesAsync();
        }
    }
}
