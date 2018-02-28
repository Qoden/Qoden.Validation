using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Qoden.Validation.AspNetCore
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
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
                }
                    break;
                default:
                {
                    var apiError = new ApiError("unexpected", e.Message);
                    var err = new ErrorResponse(new List<ApiError> {apiError});
                    context.Result = new ObjectResult(err)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                    break;
            }
        }
    }
}