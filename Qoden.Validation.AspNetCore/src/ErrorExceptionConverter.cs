using System;
using System.Collections.Generic;
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
            var error = ErrorToJson(e.Error);
            await WriteBody(response, error);
        }

        public static Dictionary<string, object> ErrorToJson(Error error)
        {
            var json = new Dictionary<string, object>();
            foreach (var kv in error)
            {
                if (!string.IsNullOrEmpty(kv.Key) && kv.Value != null)
                {
                    json[kv.Key] = kv.Value;
                }
            }
            return json;
        }

        public static int ErrorStatusCode(Error e)
        {
            if (e.ContainsKey("StatusCode"))
            {
                var ee = e["StatusCode"];
                if (ee is IConvertible)
                {
                    return System.Convert.ToInt32(ee);
                }
            }
            return StatusCodes.Status400BadRequest;
        }
    }
}