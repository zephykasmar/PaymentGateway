using Data.Repositories.Interfaces;
using Domain.Models;
using Domain.Models.Entities;
using MongoDB.Driver;

namespace Data.Repositories.Implementations
{
    public class MongoDbMerchantService : IMerchantService
    {
        private readonly IMongoCollection<Merchant> _merchants;

        public MongoDbMerchantService(MongoClient mongoClient, MongoSettings settings)
        {
            var database = mongoClient.GetDatabase(settings.Database);
            _merchants = database.GetCollection<Merchant>(settings.CollectionName);
        }

        public async Task<ServiceResult<Merchant>> TryGetMerchantAsync(string merchantId)
        {
            var value = await _merchants.Find(x => x.MerchantId == merchantId).FirstOrDefaultAsync();
            if (value is null) return ServiceResult<Merchant>.Fail(new ServiceError(ErrorTypes.MerchantNotFound));

            return ServiceResult<Merchant>.Success(value);
        }
        public async Task<ServiceResult<Merchant>> TryInsertMerchantAsync(Merchant merchant)
        {
            try
            {
                await _merchants.InsertOneAsync(merchant);
            }
            catch
            {
                return ServiceResult<Merchant>.Fail(new ServiceError(ErrorTypes.InternalServerError));
            }

            return ServiceResult<Merchant>.Success(merchant);
        }
    }
}
