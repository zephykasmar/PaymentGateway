using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;

namespace Analytics.Services
{
    public interface IPaymentsAnalyticsService    
    {
        Task<ServiceResult<PaymentDTO>> TryGetPayment(string id); 
    }
}