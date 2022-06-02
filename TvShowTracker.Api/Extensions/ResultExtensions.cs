using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace TvShowTracker.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.Match(result => new OkObjectResult(result), exception =>
            {
                if (exception is ValidationException)
                {
                    return new BadRequestObjectResult(exception);
                }

                return (IActionResult)new StatusCodeResult(500);
            });
        }

        public static IActionResult ToActionResult<T>(this Result<T> result,Func<T,IActionResult> successHandler, Func<Exception,IActionResult>? errorHandler = null)
        {
            return result.Match(successHandler, exception =>
            {
                if (errorHandler is not null)
                {
                    return errorHandler(exception);
                }

                if (exception is ValidationException)
                {
                    return new BadRequestObjectResult(exception);
                }

                return (IActionResult)new StatusCodeResult(500);
            });
        }
    }
}
