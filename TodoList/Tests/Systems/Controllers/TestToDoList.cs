using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Model.Models.DTO.ToDoListsDTO;
using Moq;
using Tests.Fixtures;
using TodoList.Controllers;
using TodoList.Services.ToDoListsServices;

namespace Tests.Systems.Controllers
{
    public class TestToDoList
    {


        //This test tests the status code 

        [Fact]
        public async Task GetOnSuccess_ReturnsStaticCode200()
        {

            //Arrange

            var mocUserService = new Mock<ITodoListServices>();

            mocUserService.Setup(services => services.GetAllLists())
               .ReturnsAsync(ListsFixtures.GetTestLists());

            var controller = new ToDoListsController(mocUserService.Object);

            //Act
            var result = (ObjectResult)await controller.GetLists();

            //Assert 
            result.StatusCode.Should().Be(200);

        }


        //This test tests the  invoke of a service   


        [Fact]

        public async Task GetOnSuccess_InvokeService()
        {
            //Arrange

            var mocUserService = new Mock<ITodoListServices>();

            mocUserService.Setup(services => services.GetAllLists())
                .ReturnsAsync(ListsFixtures.GetTestLists());
                

            var controller = new ToDoListsController(mocUserService.Object);

            //Act
            var result =  await controller.GetLists();


            //Assert 

            mocUserService.Verify(service => service.GetAllLists(), Times.Once());

        }

        //This test tests the  invoke of a service   


        [Fact]

        public async Task GetSuccess_AllLists()
        {

            //Arrange

            var mocUserService = new Mock<ITodoListServices>();

            mocUserService.Setup(service => service.GetAllLists())
                .ReturnsAsync(ListsFixtures.GetTestLists());

            var controller = new ToDoListsController(mocUserService.Object);


            //Act

            var result = await controller.GetLists();


            //Assert

            result.Should().BeOfType<OkObjectResult>();

            var objectResult = (ObjectResult)result;

            objectResult.Value.Should().BeOfType<List<CreateToDoListDTO>>();

        }


        [Fact]

        public async Task GetNotFound_AllLists404()
        {

            //Arrange

            var mocUserService = new Mock<ITodoListServices>();

            mocUserService.Setup(service => service.GetAllLists())
                .ReturnsAsync(new List<CreateToDoListDTO>());

            var controller = new ToDoListsController(mocUserService.Object);


            //Act

            var result = await controller.GetLists();


            //Assert

             result.Should().BeOfType<NotFoundResult>();

            var objectResult = (NotFoundResult)result;

            objectResult.StatusCode.Should().Be(404);


        }

        var Data = new CreateToDoListDTO()
        {
            Name = "Car",
            Priority = "Important"

        };

        var Id = 1;

        [Theory]
        [InlineData(new CreateToDoListDTO()
        {
            Name = "Car",
            Priority = "Important"

        } , 2 )]

        public async Task InsertSuccess_ToDOLists()
        {

            //Arrange
            var mocUserService = new Mock<ITodoListServices>();
            var controler = new ToDoListsController(mocUserService.Object);

            //Act

            var result = (ObjectResult)await controler.CreateList();


            //Assert 

            result.StatusCode.Should().Be(200);
        }



    }

}