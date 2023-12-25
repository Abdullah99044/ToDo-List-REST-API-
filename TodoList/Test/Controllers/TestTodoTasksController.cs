using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Models;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Model.DTO.TodoTasksDTO;
using TodoList.Controllers.v1;
using TodoList.Model.Services.TodoTasksService;

namespace Test.Controllers
{
    public class TestTodoTasksController
    {


        private readonly ITodoTasksService _TodoTasksServices;
        private readonly ILogger<ToDoTasksController> _logger;
        public TestTodoTasksController()
        {
            _TodoTasksServices = A.Fake<ITodoTasksService>();
            _logger = A.Fake<ILogger<ToDoTasksController>>();

        }

        [Fact]

        public async Task TodoTasksController_InsertTodoTasks_ReturnOk()
        {

            //Arrang

            var createTodoTasks = A.Fake<CreateTodoTasksDTO>();

            int TodoListId = 1;

            var TodoListData = A.Fake<TodoList1>();


            A.CallTo(() => _TodoTasksServices.getTodoList(TodoListId)).Returns(TodoListData);

            A.CallTo(() => _TodoTasksServices.insertTodoTask(TodoListId , createTodoTasks , 1)).Returns(true);


            var controller = new ToDoTasksController(_TodoTasksServices, _logger);

            //Act

            var result = await controller.insertTodoTask(TodoListId, createTodoTasks);

            //Assert

            result.Should().BeOfType<OkObjectResult>();

             

        }

        [Fact]

        public async Task TodoTasksController_updatetTodoTasks_ReturnOk()
        {

            //Arrang

            var updateTodoTasks = A.Fake<updateTodoTasksDTO>();


            var TodoTaskData = A.Fake<TodoTasks>();


            A.CallTo(() => _TodoTasksServices.getTodoTasks(1)).Returns(TodoTaskData);

            A.CallTo(() => _TodoTasksServices.updateTodoTask(TodoTaskData, 1)).Returns(true);


            var controller = new ToDoTasksController(_TodoTasksServices, _logger);

            //Act

            var result = await controller.udpateTodoTask(1 , updateTodoTasks  );

            //Assert

            result.Should().BeOfType<OkObjectResult>();



        }

        [Fact]

        public async Task TodoTasksController_deleteTodoTasks_ReturnOk()
        {

            //Arrang

            var deleteTodoTaskId = 1 ;


            var TodoTasksData = A.Fake<TodoTasks>();


            A.CallTo(() => _TodoTasksServices.getTodoTasks(1)).Returns(TodoTasksData);

            A.CallTo(() => _TodoTasksServices.deleteTodoTask(TodoTasksData, 1)).Returns(true);


            var controller = new ToDoTasksController(_TodoTasksServices, _logger);

            //Act

            var result = await controller.daleteTodoTask(deleteTodoTaskId);

            //Assert

            result.Should().BeOfType<OkObjectResult>();



        }
    }
}
