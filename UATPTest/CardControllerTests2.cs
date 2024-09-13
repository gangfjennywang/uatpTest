using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using uatp.Controllers;
using uatp.Models;

namespace UATPTest
{
    public class CardsControllerTests
    {
        private readonly Mock<CardRepository> _mockRepository;
        private readonly CardsController _controller;

        public CardsControllerTests()
        {
            _mockRepository = new Mock<CardRepository>();
            _controller = new CardsController(_mockRepository.Object);
        }

        [Fact]
        public void CreateCard_ReturnsOkResult_WithCard()
        {
            // Arrange
            var expectedCard = new Card(); // Replace with the actual Card object
            expectedCard.CardNumber = null;
            _mockRepository.Setup(repo => repo.CreateCard()).Returns(expectedCard);

            // Act
            var result = _controller.CreateCard();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCard = Assert.IsType<Card>(okResult.Value);
            Assert.Equal(expectedCard, returnedCard);
        }

        [Fact]
        public void Pay_WithInvalidCardNumber_ThrowsArgumentException()
        {
            // Arrange
            var request = new CardsController.PaymentRequest
            {
                CardNumber = null,
                Amount = 100m
            };

            // Act & Assert: Card number cannot be null or empty (Parameter 'CardNumber')'
            var exception = Assert.Throws<ArgumentException>(() => _controller.Pay(request));
            Assert.Equal("Card number cannot be null or empty", exception.Message);
            Assert.Equal("request.CardNumber", exception.ParamName);
        }

        [Fact]
        public void Pay_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CardsController.PaymentRequest
            {
                CardNumber = "123456789",
                Amount = 100m
            };

            _mockRepository.Setup(repo => repo.Pay(request.CardNumber, request.Amount)).Returns(true);

            // Act
            var result = _controller.Pay(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Pay_WithFailedPayment_ReturnsBadRequest()
        {
            // Arrange
            var request = new CardsController.PaymentRequest
            {
                CardNumber = "123456789",
                Amount = 100m
            };

            _mockRepository.Setup(repo => repo.Pay(request.CardNumber, request.Amount)).Returns(false);

            // Act
            var result = _controller.Pay(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Payment failed.", badRequestResult.Value);
        }

        [Fact]
        public void GetCardBalance_WithValidCardNumber_ReturnsOkResult_WithBalance()
        {
            // Arrange
            var cardNumber = "123456789";
            var expectedBalance = 150m;
            _mockRepository.Setup(repo => repo.GetCardBalance(cardNumber)).Returns(expectedBalance);

            // Act
            var result = _controller.GetCardBalance(cardNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var balance = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(expectedBalance, balance);
        }

        [Fact]
        public void GetCardBalance_WithInvalidCardNumber_ReturnsNotFound()
        {
            // Arrange
            var cardNumber = "123456789";
            //_mockRepository.Setup(repo => repo.GetCardBalance(cardNumber)).Returns((decimal?)null);

            // Act
            var result = _controller.GetCardBalance(cardNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Card not found.", notFoundResult.Value);
        }

        [Fact]
        public void CreateCard_ShouldReturnNewCard()
        {
            // Arrange
            var expectedCard = new Card { CardNumber = "123456789012345", Balance = 0 };
            //_mockRepository.Setup(repo => repo.CreateCard()).Returns(expectedCard);

            // Act
            var result = _controller.CreateCard() as OkObjectResult;

            // Check if the result is not null and is of type OkObjectResult
            Assert.NotNull(result);
            var card = result?.Value as Card;

            // Assert
            Assert.NotNull(card);
            Assert.NotEqual(expectedCard.CardNumber, card?.CardNumber);
            Assert.Equal(expectedCard.Balance, card?.Balance);
        }

    }

}

