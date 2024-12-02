using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentApi.Controllers;
using PaymentApi.Models;
using PaymentApi.Repositories;
using PaymentApi.Services;

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

            var mockPublisher = new Mock<IRabbitMQPublisher>(); // Cria o mock do publisher.

            var controller = new PaymentController(mockRepo.Object, mockPublisher.Object); // Inclui o publisher.

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
            var mockPublisher = new Mock<IRabbitMQPublisher>();

            var payment = new Payment { OrderId = 1, Status = "Pending" };

            var controller = new PaymentController(mockRepo.Object, mockPublisher.Object); // Inclui o publisher.

            // Act
            var result = await controller.CreatePayment(payment);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            // Assert
            Assert.Equal("GetPaymentById", createdAtActionResult.ActionName);
            Assert.Equal(payment, createdAtActionResult.Value);
            mockPublisher.Verify(p => p.Publish(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePayment_ShouldReturnNoContent()
        {
            // Arrange
            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(repo => repo.GetPaymentByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Payment { Id = 1, Status = "Pending" });

            var mockPublisher = new Mock<IRabbitMQPublisher>(); // Cria o mock do publisher.

            var controller = new PaymentController(mockRepo.Object, mockPublisher.Object); // Inclui o publisher.

            // Act
            var result = await controller.UpdatePayment(1, "Completed");

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockPublisher.Verify(p => p.Publish(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}

