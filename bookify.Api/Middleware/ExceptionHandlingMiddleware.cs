using Microsoft.AspNetCore.Mvc;

namespace bookify.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Exception Occured with Message{exception.Message}");

                var exceptionDetails = GetExceptionDetails(exception);

                var problemDetails = new ProblemDetails
                {
                    Status = exceptionDetails.status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail,

                };

                if (exceptionDetails.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = exceptionDetails.Errors;
                }

                context.Response.StatusCode = exceptionDetails.status;
                await context.Response.WriteAsJsonAsync(problemDetails);


            }

        }

        private static ExceptionDetails GetExceptionDetails(Exception exception)
        {
            return exception switch
            {

                Bookify.Application.Exceptions.ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "ValidationError",
                    "One or more Validaion Error has occured",
                    validationException.Errors),

                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "ServerErrors",
                    "Internal Server Error",
                    "An unexpected error has occured",
                    null)
            };

        }

    }

    internal record ExceptionDetails(
        int status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);

}

