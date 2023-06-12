using Domain.Models.Requests;
using Domain.Models;
using Fulfilment.Services;
using System.Text;

namespace Fulfilment.Services
{
    public class BankTransactionService : IBankTransactionService
    {
        private readonly HttpClient _httpClient;

        public BankTransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<bool>> Process(AcquiringBankPaymentRequest abpr)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("", abpr);
                if (!response.IsSuccessStatusCode)
                {
                    ServiceResult<bool>.Fail(new ServiceError(ErrorTypes.AcquiringBankError));
                }

                return ServiceResult<bool>.Success(true);
            }
            catch
            {
                return ServiceResult<bool>.Fail(new ServiceError(ErrorTypes.InternalServerError));
            }
        }
    }
}