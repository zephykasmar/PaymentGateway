using Domain.Models;
using Domain.Models.Requests;

namespace Fulfilment.Services
{
    public interface IBankTransactionService
    {
        Task<ServiceResult<bool>> Process(AcquiringBankPaymentRequest abpr);
    }
}
