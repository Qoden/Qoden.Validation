using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Qoden.Validation.AspNetCore
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private ILogger<ApiExceptionFilterAttribute> _logger;
        private IHostingEnvironment _environment;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public override void OnException(ExceptionContext context)
        {
            var e = context.Exception;
            switch (e)
            {
                case MultipleErrorsException multiEx:
                {
                    var apiErrors = multiEx.Errors.Select(x => x.ToApiError());
                    var err = new ErrorResponse(apiErrors.ToList());
                    context.Result = new ObjectResult(err)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    _logger.LogWarning(e, "Error: {error}", apiErrors);
                }

                    break;
                case ErrorException errEx:
                {
                    var apiError = errEx.Error.ToApiError();
                    var err = new ErrorResponse(new List<ApiError> {apiError});
                    context.Result = new ObjectResult(err)
                    {
                        StatusCode = apiError.StatusCode
                    };
                    _logger.LogWarning(e, "Error: {error}", apiError);
                }
                    break;
                default:
                {
                    var apiError = new ApiError("unexpected", e.Message);
                    if (!_environment.IsProduction())
                    {
                        apiError.Data = new Dictionary<string, object>();
                        apiError.Data["exception"] = e.ToString();
                    }
                    var err = new ErrorResponse(new List<ApiError> {apiError});
                    context.Result = new ObjectResult(err)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                    _logger.LogError(e, "Unexpected error");
                }
                    break;
            }
        }
    }
}