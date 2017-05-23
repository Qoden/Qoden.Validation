using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Qoden.Validation.AspNetCore
{
    public class ErrorExceptionConverter : ExceptionConverter<ErrorException>
    {
        private Func<Error, int> _errorStatusCode;

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
            await WriteBody(response, e.Error);
        }

        public static int ErrorStatusCode(Error e)
        {
            if (e.ContainsKey("StatusCode"))
            {
                var ee = e["StatusCode"];
                if (ee is Int32)
                {
                    return (int)ee;
                }
            }
            return StatusCodes.Status400BadRequest;
        }
    }
}