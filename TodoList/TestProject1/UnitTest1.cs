using FakeItEasy;
using TodoList.Controllers;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void GetLists_Returns_OK_AllLists()
        {

            //Arrange

            var dataStor = A.Fake<I>();
            var controller = new ListController();
            //Act

            //Assert

        }
    }
}