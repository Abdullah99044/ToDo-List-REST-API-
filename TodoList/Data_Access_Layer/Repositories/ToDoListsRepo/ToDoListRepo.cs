 
 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
 
using TodoList.Data;
 
 
namespace TodoList.DataAccess.Repositories.TodoListRepo
{


    //Cache the rquestd list data nad todolist data
    public class ToDoListRepo : ITodoListRepo
    {

        private readonly ApplicationDbContext _db;

        public ToDoListRepo(ApplicationDbContext db )
        {
            _db = db;

        }
 

        //Get all todo lists

        public async Task<IEnumerable<TodoList1>> GetAllTodoLists(int Id )
        {

            return await _db.TodoList.AsNoTracking().Where(u => u.ListId == Id).Include(tl => tl.TodoTasks).ToListAsync();

        }



        //Get a List
        public async Task<Lists> GetListById(int ListId)
        {

            return await _db.Lists.FindAsync(ListId);

        }


        //Get a Todo list
        public async Task<TodoList1> GetTodoListById(int Id)
        {

            return await _db.TodoList.FindAsync(Id);

        }

        //Save changes after every action in the database
        public async Task<bool> Save()
        {

            var saved = await _db.SaveChangesAsync();

            if(saved > 0)
            {
                return true;
            }

            return false;
        }


        //Post a todo list to the data base 
        public async Task<bool> InsertTodoList(  TodoList1 entity)
        {
 
            await _db.TodoList.AddAsync(entity);

            return await Save();
        }



        //Update a todo list to the data base 
        public async Task<bool> UpdateTodoList(TodoList1 Data)
        {
            _db.Update(Data);
            
            return await Save();
        }

        //Delete a todo list from the data base 
        public async Task<bool> DeleteTodoList(TodoList1 Data)
        {
 

            _db.Remove(Data);
            return await Save();
        }
    }
}
