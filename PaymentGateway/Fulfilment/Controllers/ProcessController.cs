using Domain.Models.DTOs;
using Domain.Models.Requests;
using Fulfilment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fulfilment.Controllers
{
    [ApiController]
    [Route("process")]
    public class ProcessController : ControllerBase
    {
        private readonly IPaymentManagementService _paymentManagementService;

        public ProcessController(IPaymentManagementService context)
        {
            _paymentManagementService = context;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> ProcessPayment([FromBody]PaymentDTO payment)
        {
            payment.TransactionId = Guid.NewGuid().ToString();
            var res = await _paymentManagementService.ProcessPaymentAsync(payment);

            if (!res.IsSuccess) 
            {
                return Problem(statusCode: res.Error.ErrorCode, detail: res.Error.ErrorMessage);   
            }

            return Ok(res.Result);
        }
    }
}