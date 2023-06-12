namespace Domain.Models 
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public ServiceError Error { get; set; }

        public static ServiceResult<T> Success(T result)
        {
            return new ServiceResult<T> { IsSuccess = true, Result = result };
        }

        public static ServiceResult<T> Fail(ServiceError error)
        {
            return new ServiceResult<T> { IsSuccess = false, Error = error };
        }
    }
}
