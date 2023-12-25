using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using Model.Models.DTO.TodoTasksDTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.MiddleWare;
using TodoList.Services.TodoTasksService;

namespace Test.Services
{
    public class TestTodoTasksService
    {

        public readonly IMapper _mapper;
        public readonly MyMemoryCache _cache;


        public TestTodoTasksService()
        {
            _mapper = A.Fake<IMapper>();
            _cache = A.Fake<MyMemoryCache>();


        }
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);

            databaseContext.Database.EnsureCreated();

            if (await databaseContext.TodoTasks.CountAsync() <= 0)
            {
                var counter = 1;
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.TodoTasks.Add(
                        new TodoTasks()
                        {
                           Id = i,
                            Name = "Test1",
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                            todoListId = 1,
                            status = "NotFinished"
                        });
                }

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }


        [Fact]

        public async Task TodoTasksService_InsertTodoTasks_ReturnOk()
        {

            //Arrange

            int todoListId = 1;

            var createTodoTasks = A.Fake<CreateTodoTasksDTO>();

            var TodoTasks = new TodoTasks()
            {

                Name = "Test1" ,
                Created = DateTime.Now,
                Updated = DateTime.Now, 
                todoListId = todoListId ,
                status = "NotFinished"

            };

         
            var databaseContext = await GetInMemoryDbContext();

            var service = new TodoTasksService(databaseContext, _mapper , _cache);

            A.CallTo(() => _mapper.Map<TodoTasks>(createTodoTasks)).Returns(TodoTasks);

            //Act


            var result = await service.insertTodoTask(todoListId , createTodoTasks, 1);


            //Assert

            result.Should().Be(true);

        }

        [Fact]

        public async Task TodoTasksService_updateTodoTasks_ReturnOk()
        {

            //Arrange

            int todoListId = 1;

 
            var TodoTasks = new TodoTasks()
            {

                Name = "Test1",
                Created = DateTime.Now,
                Updated = DateTime.Now,
                todoListId = todoListId,
                status = "NotFinished"

            };


            var databaseContext = await GetInMemoryDbContext();

            var service = new TodoTasksService(databaseContext, _mapper , _cache);

 
            //Act


            var result = await service.updateTodoTask(TodoTasks , 1);


            //Assert

            result.Should().Be(true);

        }


        [Fact]

        public async Task TodoTasksService_deleteTodoTasks_ReturnOk()
        {

            //Arrange


            var databaseContext = await GetInMemoryDbContext();


            var todoTasks = databaseContext.TodoTasks.Find(1);

            var service = new TodoTasksService(databaseContext, _mapper, _cache);


            //Act


            var result = await service.deleteTodoTask(todoTasks , 1);


            //Assert

            result.Should().Be(true);

        }
    }
}
