using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Services.ToDoListsServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test.Services
{
    public class TestTodoListServices
    {


        private IMapper _mapper;

        private IMemoryCache _memoreyCache;
        public TestTodoListServices()
        {
            _mapper = A.Fake<IMapper>();
            _memoreyCache = A.Fake<IMemoryCache>();
        }
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);

            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Lists.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Lists.Add(
                        new Lists()
                        {
                            name = i.ToString(),
                            description = "Nothing",
                            UserId = i.ToString(),
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            TodoLists = new List<TodoListsDTO>()
                            {
                        new TodoListsDTO()
                        {
                            Name = i.ToString(),
                            Priority = "Important",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            ListId = i
                        },
                                // Add other TodoLists as needed
                            }
                        });
                }

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }


        //Testing inserting Todolist in the DataBse

        [Fact]

        public async Task TodoListServices_InsertTodoList_ReturnTrue()
        {

            
                //Arrange

                int Id = 33;
                var CreateToDo = new CreateToDoListDTO()
                {
                    Name = "Test 1",
                    Priority = "Important"
                };

                

                

                var databaseContext = await GetInMemoryDbContext();

 
                var repo = new ToDoListServices(databaseContext , _mapper ,   _memoreyCache);


                //Act

                var result = await repo.InsertTodoList(Id, CreateToDo);

                //Assert

                result.Should().Be(true);

           
        }

        //Testing geating one List by Id from the DataBse


        [Fact]

        public async Task TodoListServices_GetListById_ReturnList()
        {


            //Arrange

            int Id = 1;
            var databaseContext = await GetInMemoryDbContext();

            var TodoList = A.Fake<TodoListsDTO>();

            var data = A.Fake<TodoListDTO>();


            A.CallTo(() => _mapper.Map<TodoListDTO>(TodoList)).Returns(data);



            var repo = new ToDoListServices(databaseContext, _mapper, _memoreyCache);


            //Act

            var result = await repo.GetListById(Id);

            //Assert

            result.Should().BeOfType<Lists>();


        }

        //Testing geating all Todo Lists by Id from the DataBse


        [Fact]

        public async Task TodoListServices_GetAllTodoLists_ReturnLists()
        {

            //Arrange 

            var databaseContext = await GetInMemoryDbContext();

            int Id = 1;

            var repo = new ToDoListServices(databaseContext, _mapper, _memoreyCache);

            //Act

            var result = await repo.GetAllTodoLists(Id , "Important" );

            //Assert

            result.Should().BeOfType<List<TodoListDTO>>();
             



        }

        //Testing geating   Todo List  by Id from the DataBase


        [Fact]

        public async Task GetTodoListById_GetAllTodoLists_TodoList()
        {

            //Arrange 

            var databaseContext = await GetInMemoryDbContext();

            int Id = 1;

            var repo = new ToDoListServices(databaseContext, _mapper, _memoreyCache);

            //Act

            var result = await repo.GetTodoListById(Id); 

            //Assert

            result.Should().BeOfType<TodoListsDTO>();



        }

        //Testing Updating a Todo List in the DataBase


        [Fact]

        public async Task GetTodoListById_UpdateTodoList_ReturnTrue()
        {

            //Arrange 

            var databaseContext = await GetInMemoryDbContext();

            var Data = new TodoListsDTO
            {
                Name = "1",
                Priority = "Test",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ListId = 14

            };

      

            var repo = new ToDoListServices(databaseContext, _mapper , _memoreyCache);

            //Act

            var result = await repo.UpdateTodoList(Data);

            //Assert

            result.Should().Be(true);



        }

        //Testing deleting a Todo List  from the DataBase

        public async Task GetTodoListById_DeleteTodoList_ReturnTrue()
        {

            //Arrange 

            var databaseContext = await GetInMemoryDbContext();

            var Data = new TodoListsDTO
            {
                Name = "1",
                Priority = "Test",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ListId = 14

            };



            var repo = new ToDoListServices(databaseContext, _mapper, _memoreyCache);

            //Act

            var result = await repo.DeleteTodoList(Data);

            //Assert

            result.Should().Be(true);



        }

    }
}
