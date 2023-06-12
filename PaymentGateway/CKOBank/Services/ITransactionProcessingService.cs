using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;

namespace CKOBank.Services
{
    public interface ITransactionProcessingService
    {
        Task<ServiceResult<bool>> ProcessTransaction(PaymentDTO payment);
    }
}