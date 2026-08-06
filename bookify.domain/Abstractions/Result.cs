using System.Diagnostics.CodeAnalysis;

namespace bookify.domain.Abstractions
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }
            this.IsSuccess = isSuccess;
            this.Error = error;

        }
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; private set; }
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, Error.None);
        public static Result<T> Failure<T>(Error error) => new(default!, false, error);
        public static Result<T> Create<T>(T value) =>
            value is null ? Failure<T>(Error.NullValue) : Success(value);
    }
    public class Result<T> : Result
    {
        protected internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            this.value = value;
        }



        private readonly T? value;

        [NotNull]
        public T Value => IsSuccess ? value! : throw new InvalidOperationException("Cannot access the value of a failed result.");

        public static implicit operator Result<T>(T value)
            => Create(value);
    }
}
