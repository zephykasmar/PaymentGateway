using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;

namespace CKOBank.Services
{
    public class TransactionProcessingService : ITransactionProcessingService
    {
        public Task<ServiceResult<bool>> ProcessTransaction(PaymentDTO payment)
        {
            return Task.FromResult(ServiceResult<bool>.Success(true));    
        }
    }
}