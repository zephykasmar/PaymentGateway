using Analytics.Services;
using Domain.Models.DTOs;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentsAnalyticsService : ControllerBase
    {
        private readonly IPaymentsAnalyticsService _paymentAnalyticsService;

        public PaymentsAnalyticsService(IPaymentsAnalyticsService pas)
        {
            _paymentAnalyticsService = pas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(Guid id)
        {
            var payment = await _paymentAnalyticsService.TryGetPayment(id.ToString());
            if (!payment.IsSuccess) return Problem(statusCode: payment.Error.ErrorCode, detail: payment.Error.ErrorMessage);

            return Ok(payment.Result);
        }
    }
}