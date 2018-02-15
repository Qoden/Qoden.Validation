using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Qoden.Validation.AspNetCore
{
    public class MultipleErrorsExceptionConverter : ExceptionConverter<MultipleErrorsException>
    {
        private Func<Error, int> _errorStatusCode;

        public MultipleErrorsExceptionConverter(Func<Error, int> errorStatusCode)
        {
            Assert.Argument(errorStatusCode, nameof(errorStatusCode)).NotNull();
            _errorStatusCode = errorStatusCode;
        }

        public MultipleErrorsExceptionConverter() : this(ErrorExceptionConverter.ErrorStatusCode)
        {
        }

        protected override async Task Convert(MultipleErrorsException e, HttpContext context)
        {
            var response = context.Response;
            var topError = e.Errors.OrderBy(ErrorScore).FirstOrDefault();
            response.StatusCode = _errorStatusCode(topError);
            var errors = e.Errors.Select(x => x.ToApiError()).ToList();
            var error = new ErrorResponse(errors);
            await WriteBody(response, error);
        }

        protected virtual int ErrorScore(Error arg)
        {
            var code = _errorStatusCode(arg);
            if (code == StatusCodes.Status400BadRequest)
            {
                //validation errors comes last
                return 0;
            }

            if (code == StatusCodes.Status403Forbidden || code == StatusCodes.Status401Unauthorized)
            {
                return 3;
            }

            if (code == StatusCodes.Status404NotFound)
            {
                return 2;
            }
            
            return 1;
        }
    }
}