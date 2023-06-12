using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using Domain.Models.Requests;

namespace Data.Repositories.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<PaymentDTO>> TryInsertPaymentAsync(Payment payment);
        Task<ServiceResult<PaymentDTO>> TryGetPaymentAsync(string paymentId);
        Task<ServiceResult<List<PaymentDTO>>> TryGetPaymentsForMerchantAsync(string merchantId);
    }
}
