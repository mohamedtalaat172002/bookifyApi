namespace Bookify.Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        private IEnumerable<ValidationError> _errors;

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            _errors = errors;
        }
    }
}
