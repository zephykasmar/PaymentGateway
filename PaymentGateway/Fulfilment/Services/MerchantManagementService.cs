using Data.Repositories.Interfaces;
using Domain.Models;

namespace Fulfilment.Services
{
    public class MerchantManagementService : IMerchantManagementService
    {
        private readonly ICacheService _merchantsCache;
        private readonly IMerchantService _merchantService;

        public MerchantManagementService(ICacheService cache, IMerchantService merchantService)
        {
            _merchantsCache = cache;
            _merchantService = merchantService;
        }

        public async Task<ServiceResult<bool>> ValidateMerchant(string merchantId)
        {
            var id = await _merchantsCache.TryGet(merchantId.ToString());
            if (!id.IsSuccess)
            {
                var merchant = await _merchantService.TryGetMerchantAsync(merchantId);
                if (!merchant.IsSuccess) return ServiceResult<bool>.Fail(merchant.Error);

                var insert = await _merchantsCache.TrySet(merchantId.ToString(), "");
                if (!insert.IsSuccess) return ServiceResult<bool>.Fail(insert.Error);
            }

            return ServiceResult<bool>.Success(true);
        }
    }
}
