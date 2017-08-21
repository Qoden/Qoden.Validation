using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Qoden.Validation.AspNetCore
{
    public abstract class ExceptionConverter<T> : IExceptionConverter
        where T : Exception
    {
        public Task Convert(Exception exception, HttpContext context)
        {
            Assert.Argument(exception as T, nameof(exception)).NotNull();
            Assert.Argument(context, nameof(context)).NotNull();
            return Convert((T) exception, context);
        }

        protected abstract Task Convert(T e, HttpContext context);

        protected virtual async Task WriteBody(HttpResponse response, object body)
        {
            var responseStr = JsonConvert.SerializeObject(body);
            using (var writer = new StreamWriter(response.Body, Encoding.UTF8))
            {
                await writer.WriteAsync(responseStr.ToCharArray());
            }
        }
    }
}