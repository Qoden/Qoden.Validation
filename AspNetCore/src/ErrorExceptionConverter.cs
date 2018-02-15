using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Qoden.Util;

namespace Qoden.Validation.AspNetCore
{
    public class ErrorExceptionConverter : ExceptionConverter<ErrorException>
    {
        private readonly Func<Error, int> _errorStatusCode;

        public ErrorExceptionConverter(Func<Error, int> errorStatusCode)
        {
            Assert.Argument(errorStatusCode, nameof(errorStatusCode)).NotNull();
            _errorStatusCode = errorStatusCode;
        }

        public ErrorExceptionConverter() : this(ErrorStatusCode)
        {
        }

        protected override async Task Convert(ErrorException e, HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = _errorStatusCode(e.Error);
            var error = ErrorToJson(e.Error);
            await WriteBody(response, error);
        }

        public static ErrorResponse ErrorToJson(Error error)
        {
            var apiError = error.ToApiError();
            return new ErrorResponse(new List<ApiError> {apiError});
        }

        public static int ErrorStatusCode(Error e)
        {
            if (e.ContainsKey(ApiError.StatusCodeKey))
            {
                var ee = e[ApiError.StatusCodeKey];
                if (ee is IConvertible)
                {
                    return System.Convert.ToInt32(ee);
                }
            }

            return StatusCodes.Status400BadRequest;
        }
    }
}