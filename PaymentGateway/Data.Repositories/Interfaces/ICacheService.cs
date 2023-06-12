using Domain.Models;

namespace Data.Repositories.Interfaces
{
    public interface ICacheService
    {
        Task<ServiceResult<string>> TryGet(string key);
        Task<ServiceResult<bool>> TrySet(string key, string value);
    }
}
