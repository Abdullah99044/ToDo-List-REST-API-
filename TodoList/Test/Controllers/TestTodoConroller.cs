using AutoMapper;
using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Controllers.v1;
using TodoList.Services.ToDoListsServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test.Controllers
{
    public class TestTodoConroller
    {

        private ITodoListServices _ListServices;
        private readonly ILogger<ToDoListsController> _logger;
     


        public TestTodoConroller()
        {
            _ListServices =  A.Fake<ITodoListServices>();
            _logger = A.Fake<ILogger<ToDoListsController>>();



        }



        // Test Create list : 

        [Fact]

        public async Task ToDoList_CreateList_ReturnOk()
        {

            //Arrange 

            int Id = 1;

            var data = new CreateToDoListDTO()
            {
                Name = "name",
                color = "red"
            };

            var FakeList = A.Fake<Lists>();

            A.CallTo(() => _ListServices.GetListById(Id)).Returns(FakeList);

            A.CallTo(() => _ListServices.InsertTodoList(Id , data));

            var controller = new ToDoListsController(_ListServices  , _logger);

            //Act

            var result = await controller.CreateList(Id , data);

            //Assert

            result.Should().BeOfType<OkObjectResult>();



        }


        // Test GetAllTodoLists : 


        [Fact]

        public async Task ToDoList_GetAllTodoLists_ReturnOk()
        {

            //Arrange 

            int Id = 1;

            string filtriring = "Important";

            var FakeList = A.Fake<Lists>();

            var FakeListTodoList = A.Fake<TodoList1>();

            var FakeToDoLists = A.Fake<List<TodoListDTO>>();

            List<TodoListDTO> FakeToDoListDTO = new List<TodoListDTO>();

            for(int i = 0; i < 10; i++)
            {
                FakeToDoListDTO.Add(

                    new TodoListDTO()

                    {

                        Name = "name",
                        color = "red"
                        

                    }
                );
            };

            A.CallTo(() => _ListServices.GetListById(Id)).Returns(FakeList);

            A.CallTo(() =>  _ListServices.GetAllTodoLists(Id )).Returns(FakeToDoListDTO);

           

            var controller = new ToDoListsController(_ListServices , _logger);

            //Act

            var result = await controller.GetAllToDoLists(Id  );

            //Assert

            result.Should().BeOfType<OkObjectResult>();  

            var okResult = result as OkObjectResult;
          
            okResult.Value.Should().BeOfType<List<TodoListDTO>>();


        }


        // Test UpdateTodoList : 

        [Fact]


        public async Task TodoListController_UpdateTodoList_ReturnOK()
        {

            //Arrange

            var TodoListDTO = A.Fake<UpdateToDoListDTO>();

            var TodoList = A.Fake<TodoList1>();



            A.CallTo(() => _ListServices.GetTodoListById(1) ).Returns(TodoList);

            A.CallTo(() => _ListServices.UpdateTodoList(TodoList)).Returns(true);


            var controller = new ToDoListsController(_ListServices , _logger);


            //Act

            var result  = await controller.UpdateTodoList(TodoListDTO);

            //Assert

            result.Should().BeOfType<OkObjectResult>();


        }


        // Test DeleteTodoList : 

        [Fact]


        public async Task TodoListController_DeleteTodoList_ReturnOK()
        {

            //Arrange

 
            var TodoList = A.Fake<TodoList1>();



            A.CallTo(() => _ListServices.GetTodoListById(1)).Returns(TodoList);

            A.CallTo(() => _ListServices.DeleteTodoList(TodoList)).Returns(true);


            var controller = new ToDoListsController(_ListServices , _logger);


            //Act

            var result = await controller.DeleteTodoList(1);

            //Assert

            result.Should().BeOfType<OkObjectResult>();  
            
          
            

        }




    }
}
