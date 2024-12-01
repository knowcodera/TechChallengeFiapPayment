using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentApi.Controllers;
using PaymentApi.Models;
using PaymentApi.Repositories;

namespace PaymentTests.Controllers
{
    public class PaymentControllerTests
    {

        [Fact]
        public async Task GetPayments_ShouldReturnAllPayments()
        {
            // Arrange
            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(repo => repo.GetPaymentsAsync())
                    .ReturnsAsync(new List<Payment>
                    {
                        new Payment { OrderId = 1, Status = "Pending" },
                        new Payment { OrderId = 2, Status = "Completed" }
                    });

            var controller = new PaymentController(mockRepo.Object);

            // Act
            var result = await controller.GetPayments();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var payments = Assert.IsAssignableFrom<IEnumerable<Payment>>(okResult.Value);

            // Assert
            Assert.Equal(2, payments.Count());
        }

        [Fact]
        public async Task CreatePayment_ShouldReturnCreatedPayment()
        {
            // Arrange
            var mockRepo = new Mock<IPaymentRepository>();
            var payment = new Payment { OrderId = 1, Status = "Pending" };

            var controller = new PaymentController(mockRepo.Object);

            // Act
            var result = await controller.CreatePayment(payment);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            // Assert
            Assert.Equal("GetPaymentById", createdAtActionResult.ActionName);
            Assert.Equal(payment, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdatePayment_ShouldReturnNoContent()
        {
            // Arrange
            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(repo => repo.GetPaymentByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Payment { Id = 1, Status = "Pending" });

            var controller = new PaymentController(mockRepo.Object);

            // Act
            var result = await controller.UpdatePayment(1, "Completed");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
