using Data.Repositories.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using Domain.Models.Extensions;
using Domain.Models.Requests;
using Microsoft.EntityFrameworkCore.Internal;

namespace Fulfilment.Services
{
    public class PaymentManagementService : IPaymentManagementService
    {
        private readonly IPaymentService _paymentService;
        private readonly IMerchantManagementService _merchantsManagementService;
        private readonly IBankTransactionService _bankService;

        public PaymentManagementService(IPaymentService paymentService, IMerchantManagementService merchantsService, IBankTransactionService bankService)
        {
            _paymentService = paymentService;
            _merchantsManagementService = merchantsService;
            _bankService = bankService;
        }

        public async Task<ServiceResult<PaymentDTO>> ProcessPaymentAsync(PaymentDTO payment)
        {
            var merchant = await _merchantsManagementService.ValidateMerchant(payment.MerchantId);
            if (!merchant.IsSuccess) return ServiceResult<PaymentDTO>.Fail(merchant.Error);

            var req = await _bankService.Process(new AcquiringBankPaymentRequest(payment.PAN, payment.Expiry, payment.CVV, payment.Amount, payment.CurrencyCode));
            if (!req.IsSuccess) return ServiceResult<PaymentDTO>.Fail(req.Error);

            var ins = await _paymentService.TryInsertPaymentAsync(payment.ToPayment());
            if (!ins.IsSuccess) return ServiceResult<PaymentDTO>.Fail(ins.Error);

            return ServiceResult<PaymentDTO>.Success(ins.Result);

        }
    }
}
