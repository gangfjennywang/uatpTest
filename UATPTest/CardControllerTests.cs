using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using uatp.Controllers; // Replace with your actual namespace
using uatp.Models; // Replace with your actual namespace

namespace UATPTest.Tests
{
   
    public class CardsControllerTests1
    {
        [Fact]
        public void CreateCard_ReturnsOkResult_WithCard()
        {
            // Arrange
            var mockRepository = new Mock<CardRepository>();
            var expectedCard = new Card(); // Replace with the actual type or mock of your Card class
            mockRepository.Setup(repo => repo.CreateCard()).Returns(expectedCard);

            var controller = new CardsController(mockRepository.Object);

            // Act
            var result = controller.CreateCard();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCard = Assert.IsType<Card>(okResult.Value);
            Assert.Equal(expectedCard, returnedCard);
        }
    }
}
