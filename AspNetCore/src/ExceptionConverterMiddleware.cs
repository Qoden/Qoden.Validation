using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Qoden.Util;

namespace Qoden.Validation.AspNetCore
{
    public interface IExceptionConverter
    {
        Task Convert(Exception exception, HttpContext context);
    }

    public class ExceptionConverters
    {
        private Dictionary<Type, IExceptionConverter> _converters = new Dictionary<Type, IExceptionConverter>();

        public ExceptionConverters()
        {
            _converters.Add(typeof(ErrorException), new ErrorExceptionConverter());
            _converters.Add(typeof(MultipleErrorsException), new MultipleErrorsExceptionConverter());
        }

        public void Add<T>(IExceptionConverter converter)
        {
            Add(typeof(T), converter);
        }

        public void Add(Type type, IExceptionConverter converter)
        {
            Assert.Argument(type, nameof(type)).NotNull();
            Assert.Argument(converter, nameof(converter)).NotNull();
            _converters[type] = converter;
        }

        public IEnumerable<KeyValuePair<Type, IExceptionConverter>> Converters
        {
            get { return _converters; }
        }
    }

    public class ExceptionConverterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TypeMap<IExceptionConverter> _converters = new TypeMap<IExceptionConverter>();
        private readonly ILogger _logger;

        public ExceptionConverterMiddleware(RequestDelegate next, ExceptionConverters converters, ILoggerFactory loggerFactory)
        {
            Assert.Argument(next, nameof(next)).NotNull();
            Assert.Argument(loggerFactory, nameof(loggerFactory)).NotNull();
            _next = next;
            _logger = loggerFactory.CreateLogger(nameof(ExceptionConverterMiddleware));
            foreach (var kv in converters.Converters)
            {
                _converters.Add(kv.Key, kv.Value);
            }
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
                    context.Response.Headers.Add("Content-Type", "application/json");
                    await converter.Convert(e, context);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public static class ExceptionConvertersExtensions
    {
        public static void UseExceptionConverters(this IApplicationBuilder app, ExceptionConverters options = null)
        {
            if (options == null) options = new ExceptionConverters();
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory));
            Assert.State(loggerFactory, nameof(loggerFactory)).NotNull("Cannot find ILoggerFactory");
            app.UseMiddleware<ExceptionConverterMiddleware>(options, loggerFactory);
        }
    }
}