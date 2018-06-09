using TVim.Client.Models.Errors;

namespace TVim.Client.Models
{
    public class OperationResult
    {
        public bool IsError => Error != null;

        public ErrorBase Error { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(ErrorBase error)
        {
            Error = error;
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public OperationResult() { }

        public OperationResult(ErrorBase error) : base(error) { }


        public OperationResult(T result)
        {
            Result = result;
        }

    }
}