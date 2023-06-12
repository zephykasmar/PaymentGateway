using CKOBank.Services;
using Domain.Models.DTOs;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CKOBank.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionProcessingController : ControllerBase
    {
        private readonly ITransactionProcessingService _transactionProcessingService;
        public TransactionProcessingController(ITransactionProcessingService tps) 
        {
            _transactionProcessingService = tps;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessTransaction([FromBody]PaymentDTO payment)
        {
            return Ok(await _transactionProcessingService.ProcessTransaction(payment));
        }
    }
}