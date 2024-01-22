namespace BusinessLogic
{
    public class Result<TValue, TError>
    {
        public TValue Value { get; }
        public TError Error { get; }

        public bool IsSuccess { get; set; }
        public bool IsError => !IsSuccess;

        private Result(TValue value)
        {
            IsSuccess = true;
            Value = value;
            Error = default;
        }

        private Result(TError error)
        {
            IsSuccess = false;
            Value = default;
            Error = error;
        }

        // Happy path
        public static implicit operator Result<TValue, TError>(TValue value) => new (value);

        // Error path
        public static implicit operator Result<TValue, TError>(TError error) => new (error);
    }
}
