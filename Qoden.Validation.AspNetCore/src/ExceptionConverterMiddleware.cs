using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Qoden.Reflection;
using Qoden.Validation;

namespace Qoden.Validation.AspNetCore
{
    public interface IExceptionConverter
    {
        Task Convert(Exception exception, HttpContext context);
    }

    public class ExceptionConverterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TypeMap<IExceptionConverter> _converters = new TypeMap<IExceptionConverter>();
        private readonly ILogger _logger;

        public ExceptionConverterMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            Assert.Argument(next, nameof(next)).NotNull();
            Assert.Argument(loggerFactory, nameof(loggerFactory)).NotNull();
            _next = next;
            _logger = loggerFactory.CreateLogger(nameof(ExceptionConverterMiddleware));
            _converters.Add(typeof(ErrorException), new ErrorExceptionConverter());
            _converters.Add(typeof(MultipleErrorsException), new MultipleErrorsExceptionConverter());
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }
                var converter = _converters.Implementation(e);
                if (converter != null)
                {
                    await converter.Convert(e, context);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}