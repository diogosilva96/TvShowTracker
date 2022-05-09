using FluentValidation.Results;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Helpers
{
    public static class ResultHelper
    {
        public static Result<T> ToSuccessResult<T>(T data)
        {
            return new Result<T>()
            {
                Data = data,
                Success = true
            };
        }

        public static Result<T> ToErrorResult<T>(IEnumerable<string> errors)
        {
            if (errors is null || !errors.Any())
            {
                throw new ArgumentException("There are no validation errors.");
            }
            return new Result<T>()
            {
                Errors = errors,
                Success = false
            };
        }
        public static Result<T> ToErrorResult<T>(ValidationResult? validationResult)
        {
            if (validationResult is null || !validationResult.Errors.Any())
            {
                throw new ArgumentException("There are no validation errors.");
            }
            return new Result<T>()
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage),
                Success = false
            };

        }
    }
}
