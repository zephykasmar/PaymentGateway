using Data.Repositories.Interfaces;
using Domain.Models;
using StackExchange.Redis;

namespace Repository.Data.Implementations
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _merchantCache;

        public RedisCacheService(ConnectionMultiplexer connectionMultiplexer)
        {
            _merchantCache = connectionMultiplexer.GetDatabase();
        }

        public async Task<ServiceResult<bool>> TrySet(string key, string value)
        {
            var res = await _merchantCache.StringSetAsync(key, value);
            if (!res) return ServiceResult<bool>.Fail(new ServiceError(ErrorTypes.InternalServerError));

            return ServiceResult<bool>.Success(res);
        }

        public async Task<ServiceResult<string>> TryGet(string key)
        {
            var value = await _merchantCache.StringGetAsync(key);
            if (value.IsNullOrEmpty) return ServiceResult<string>.Fail(new ServiceError(ErrorTypes.MerchantNotFound));

            return ServiceResult<string>.Success(value);
        }
    }
}
