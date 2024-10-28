namespace Entities.Exeptions
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public T Value { get; private set; }

        private Result(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(value, true, null);
        public static Result<T> Fail(string error) => new Result<T>(default, false, error);
    }
}
