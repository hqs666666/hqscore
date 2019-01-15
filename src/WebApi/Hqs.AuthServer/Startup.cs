using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hqs.AuthServer.AuthValidator;
using Hqs.IRepository;
using Hqs.IRepository.Users;
using Hqs.Model.Users;
using Hqs.Repository.SqlServer;
using Hqs.Repository.SqlServer.Users;
using IdentityServer4.Dapper.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hqs.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(User));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]));

            //注册数据库上下文
            services.AddScoped<IDataContext, SqlServerContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));

            //注册Repository
            foreach (var item in GetClassName("Hqs.Repository.SqlServer"))
            {
                foreach (var typeArray in item.Value)
                {
                    if(typeArray.Name == typeof(IBaseRepository<>).Name)
                        continue;
                    services.AddScoped(typeArray, item.Key);
                }
            }

            //注册Service
            foreach (var item in GetClassName("Hqs.Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }

            //配置ids4
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddDapperStore(options => options.DbConnectionString = Configuration["ConnectionStrings:SqlServer"])
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddProfileService<ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseMvc();
        }

        private Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }
    }
}
