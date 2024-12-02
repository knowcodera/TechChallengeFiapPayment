using Microsoft.EntityFrameworkCore;
using PaymentApi.Data;
using PaymentApi.Models;
using PaymentApi.Repositories;

namespace PaymentTests.Repositories
{
    public class PaymentRepositoryTests
    {
        private PaymentContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PaymentContext>()
                .UseInMemoryDatabase(databaseName: dbName) // Nome único para cada teste
                .Options;
            return new PaymentContext(options);
        }

        [Fact]
        public async Task CreatePaymentAsync_ShouldAddPaymentToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new PaymentRepository(context);

            var payment = new Payment
            {
                OrderId = 1,
                PaymentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            // Act
            await repository.CreatePaymentAsync(payment);
            var payments = await repository.GetPaymentsAsync();

            // Assert
            Assert.Single(payments);
            Assert.Equal("Pending", payments.First().Status);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ShouldReturnCorrectPayment()
        {
            // Arrange
            var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new PaymentRepository(context);

            var payment = new Payment
            {
                OrderId = 1,
                PaymentDate = DateTime.UtcNow,
                Status = "Completed"
            };

            await repository.CreatePaymentAsync(payment);

            // Act
            var retrievedPayment = await repository.GetPaymentByIdAsync(payment.Id);

            // Assert
            Assert.NotNull(retrievedPayment);
            Assert.Equal("Completed", retrievedPayment.Status);
        }

        [Fact]
        public async Task UpdatePaymentAsync_ShouldModifyExistingPayment()
        {
            // Arrange
            var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new PaymentRepository(context);

            var payment = new Payment
            {
                OrderId = 1,
                PaymentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await repository.CreatePaymentAsync(payment);

            // Act
            payment.Status = "Completed";
            await repository.UpdatePaymentAsync(payment);

            var updatedPayment = await repository.GetPaymentByIdAsync(payment.Id);

            // Assert
            Assert.NotNull(updatedPayment);
            Assert.Equal("Completed", updatedPayment.Status);
        }
    }
}
