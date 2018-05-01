using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Qoden.Validation.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds MVC services to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="setupAction">An System.Action`1 to configure the provided Microsoft.AspNetCore.Mvc.MvcOptions.</param>
        /// <returns>An Microsoft.Extensions.DependencyInjection.IMvcBuilder that can be used to further configure the MVC services.</returns>
        public static IMvcBuilder AddMyMvc(this IServiceCollection services, Action<MvcOptions> setupAction = null)
        {
            return services.AddMvc(o =>
            {
                o.Filters.Add<ApiExceptionFilterAttribute>();
                o.Filters.Add<ValidateModelAttribute>();
                setupAction?.Invoke(o);
            });
        }
    }
}