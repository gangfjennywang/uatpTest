using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using uatp.Controllers;
using uatp.Models;

namespace UATPTest
{
    public class PaymentsControllerTests
    {
        [Fact]
        public void CreatePayment_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new PaymentsController();
            var paymentRequest = new PaymentRequest();

            // Act
            var result = controller.CreatePayment(paymentRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid payment amount.", badRequestResult.Value);
        }

        [Fact]
        public void CreatePayment_NegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            var controller = new PaymentsController();
            var request = new PaymentRequest { Amount = -10m };

            // Act
            var result = controller.CreatePayment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid payment amount.", badRequestResult.Value);
        }

        [Fact]
        public void CreatePayment_ValidRequest_ReturnsOk()
        {
            // Arrange
            var mockUfeService = new Mock<UfeService>();
            //mockUfeService.Setup(s => s.GetFeeMultiplier()).Returns(0.1m);

            var controller = new PaymentsController(); 
            var request = new PaymentRequest { Amount = 100m };

            // Act
            var result = controller.CreatePayment(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var paymentResponse = Assert.IsType<PaymentResponse>(okResult.Value);

            Assert.Equal(100m, paymentResponse.Amount);
            Assert.Equal(10m, paymentResponse.Fee); // 100m * 0.1m
            Assert.Equal(110m, paymentResponse.Total); // 100m + 10m
        }
    }

}
