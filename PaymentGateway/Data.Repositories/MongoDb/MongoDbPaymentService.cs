using Data.Repositories.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using Domain.Models.Extensions;
using Domain.Models.Requests;
using MongoDB.Driver;
using System.Text;

namespace Data.Repositories.Implementations
{
    public class MongoDbPaymentService : IPaymentService
    {
        private readonly IMongoCollection<Payment> _payments;

        public MongoDbPaymentService(MongoClient mongoClient, MongoSettings settings)
        {
            var database = mongoClient.GetDatabase(settings.Database);
            _payments = database.GetCollection<Payment>(settings.CollectionName);
        }

        public async Task<ServiceResult<PaymentDTO>> TryGetPaymentAsync(string paymentId)
        {
            var value = await _payments.Find(x => x.TransactionId == paymentId).FirstOrDefaultAsync();
            if (value is null) return ServiceResult<PaymentDTO>.Fail(new ServiceError(ErrorTypes.PaymentNotFound));

            return ServiceResult<PaymentDTO>.Success(value.ToPaymentDTO());
        }

        public async Task<ServiceResult<List<PaymentDTO>>> TryGetPaymentsForMerchantAsync(string merchantId)
        {
            var value = await _payments.FindAsync(x => x.MerchantId == merchantId);
            if (!value.Any()) return ServiceResult<List<PaymentDTO>>.Fail(new ServiceError(ErrorTypes.PaymentsForMerchantNotFound));

            return ServiceResult<List<PaymentDTO>>.Success(value.ToList().Select(x => x.ToPaymentDTO()).ToList());
        }

        public async Task<ServiceResult<PaymentDTO>> TryInsertPaymentAsync(Payment payment)
        {
            try
            {
                await _payments.InsertOneAsync(payment);
            }
            catch
            {
                return ServiceResult<PaymentDTO>.Fail(new ServiceError(ErrorTypes.InternalServerError));
            }

            return ServiceResult<PaymentDTO>.Success(payment.ToPaymentDTO());
        }
    }
}
