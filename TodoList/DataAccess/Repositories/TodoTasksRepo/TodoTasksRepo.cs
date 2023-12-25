 
using Model.Models;
 
using TodoList.Data;
 

namespace TodoList.DataAccess.Repositories.TodoTasksRepo
{
    public class TodoTasksRepo : ITodoTasksRepo
    {

        public readonly ApplicationDbContext _db;
 
        public TodoTasksRepo(ApplicationDbContext  db )
        {
            _db = db;
            
        }


        //Get a todolist from the data base 

        public async Task<TodoList1> getTodoList(int id)
        {

            return await _db.TodoList.FindAsync(id); 
        }


        //Get a todoTask from the data base 

        public async Task<TodoTasks> getTodoTasks(int Id)
        {

            return await _db.TodoTasks.FindAsync(Id);
        }


        //Save changes in the database

        public async Task<bool> save()
        {

            var save = await _db.SaveChangesAsync() ;

            if (save > 0 )
            {
                return true;
            }

            return false;
        }




        //Post a todo task into the data base 

        public async Task<bool> insertTodoTask(int todoListId, TodoTasks entity )
        {


            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.status = "Not finished";
            entity.todoListId = todoListId;
            

            await _db.TodoTasks.AddAsync(entity);

            return await save();
        }




        //Update a todo task to the data base 

        public async Task<bool> updateTodoTask(TodoTasks entity )
        {

           _db.TodoTasks.Update(entity);

            return await save();
        }


        //Delete a todo task from the data base 

        public async Task<bool> deleteTodoTask(TodoTasks TodoTask )
        {
            _db.TodoTasks.Remove(TodoTask);

            return  await save();
        }
    }
}
