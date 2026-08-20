namespace Bookify.Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public IEnumerable<ValidationError> Errors;

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}
