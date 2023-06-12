using Domain.Models;
using Domain.Models.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IMerchantService
    {
        Task<ServiceResult<Merchant>> TryInsertMerchantAsync(Merchant merchant);
        Task<ServiceResult<Merchant>> TryGetMerchantAsync(string merchantId);
    }
}
