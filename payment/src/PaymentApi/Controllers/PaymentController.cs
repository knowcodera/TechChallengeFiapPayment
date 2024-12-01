using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models;
using PaymentApi.Repositories;
using PaymentApi.Services;

namespace PaymentApi.Controllers
{

    [ApiController]
    [Route("v1/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly RabbitMQPublisher _publisher;

        public PaymentController(IPaymentRepository repository, RabbitMQPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await _repository.GetPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _repository.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = "Pending";

            await _repository.CreatePaymentAsync(payment);

            var message = $"Payment for OrderId {payment.OrderId} created with status {payment.Status}";
            _publisher.Publish("payments_queue", message);

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] string status)
        {
            var payment = await _repository.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            payment.Status = status;
            await _repository.UpdatePaymentAsync(payment);

            var message = $"Payment for OrderId {payment.OrderId} updated to status {status}";
            _publisher.Publish("payments_queue", message);

            return NoContent();
        }
    }
}
