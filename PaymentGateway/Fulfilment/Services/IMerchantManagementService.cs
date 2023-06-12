using Domain.Models;

namespace Fulfilment.Services
{
    public interface IMerchantManagementService
    {
        Task<ServiceResult<bool>> ValidateMerchant(string merchantId);
    }
}
