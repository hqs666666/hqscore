using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hqs.Helper
{
    public class DIHelper
    {
        public static IServiceCollection Services { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }
    }

    public static class Extensions
    {
        public static IServiceCollection AddDIHelper(this IServiceCollection services)
        {
            DIHelper.Services = services;

            return services;
        }

        public static IApplicationBuilder UseDIHelper(this IApplicationBuilder builder)
        {
            DIHelper.ServiceProvider = builder.ApplicationServices;

            return builder;
        }

    }
}
