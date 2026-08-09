using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Bookify.Application.Abstraction.Behavior
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationErrors = _validators.Select(v => v.Validate(context))
                .Where(ValidationResult => ValidationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .Select(ValidationFailure => new ValidationError(ValidationFailure.PropertyName, ValidationFailure.ErrorMessage))
                .ToList();

            if (validationErrors.Any())
            {
                throw new Exceptions.ValidationException(validationErrors);
            }

            return await next();
        }
    }
}
