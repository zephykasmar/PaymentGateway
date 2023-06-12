using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;

namespace Fulfilment.Services
{
    public interface IPaymentManagementService
    {
        Task<ServiceResult<PaymentDTO>> ProcessPaymentAsync(PaymentDTO payment);
    }
}