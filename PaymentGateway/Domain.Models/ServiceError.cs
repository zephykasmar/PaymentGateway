namespace Domain.Models
{
    public enum ErrorTypes
    {
        None,
        MerchantNotFound,
        MerchantInvalid,
        InternalServerError,
        AcquiringBankError,
        PaymentNotFound,
        PaymentsForMerchantNotFound
    }

    public class ServiceError
    {
        private ErrorTypes _error;

        private Dictionary<ErrorTypes, (string message, int errCode)> _errorMessages = new Dictionary<ErrorTypes, (string, int)>
        {
            { ErrorTypes.None, ("", -1) },
            { ErrorTypes.MerchantNotFound, ("Merchant provided does not exist", 404) },
            { ErrorTypes.MerchantInvalid, ("Merchant provided is invalid", 400) },
            { ErrorTypes.InternalServerError, ("Something went wrong", 500) },
            { ErrorTypes.AcquiringBankError, ("Acquiring bank denied request", 401) },
            { ErrorTypes.PaymentNotFound, ("Payment provided does not exist", 404) },
            { ErrorTypes.PaymentsForMerchantNotFound, ("Merchant provided has no payments", 404) }
        };

        public ServiceError(ErrorTypes err)
        {
            _error = err;
        }

        public int ErrorCode => _errorMessages[_error].errCode;
        public string ErrorMessage => _errorMessages[_error].message;
    }
}
