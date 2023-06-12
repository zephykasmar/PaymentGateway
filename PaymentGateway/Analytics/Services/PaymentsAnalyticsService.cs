using Data.Repositories.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;

namespace Analytics.Services
{
    public class PaymentsAnalyticsService : IPaymentsAnalyticsService
    {
        private readonly IPaymentService _paymentService;

        public PaymentsAnalyticsService(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ServiceResult<PaymentDTO>> TryGetPayment(string id)
        {
            var payment = await _paymentService.TryGetPaymentAsync(id);
            if (!payment.IsSuccess) return ServiceResult<PaymentDTO>.Fail(payment.Error);

            return payment;
        }
    }
}