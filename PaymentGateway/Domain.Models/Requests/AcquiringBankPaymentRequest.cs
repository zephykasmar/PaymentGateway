namespace Domain.Models.Requests
{
    public readonly record struct AcquiringBankPaymentRequest(string pan, string expiry, string cvv,
                                                              decimal amount, string currencyCode);
}