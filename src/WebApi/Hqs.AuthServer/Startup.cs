using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hqs.AuthServer.AuthValidator;
using Hqs.IRepository;
using Hqs.Model.Users;
using Hqs.Repository.SqlServer;
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
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]));

            #region 依赖注入

            //注册数据库上下文
            services.AddScoped<IDataContext, SqlServerContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));

            //注册Repository
            foreach (var item in GetClassName("Hqs.Repository.SqlServer"))
            {
                foreach (var typeArray in item.Value)
                {
                    if (typeArray.Name == typeof(IBaseRepository<>).Name)
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

            #endregion

            #region 配置ids4

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddDapperStore(options =>
                    {
                        options.DbConnectionString = Configuration["ConnectionStrings:SqlServer"];
                    })
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddProfileService<ProfileService>();

            #endregion

            #region 跨域

            services.AddCors(options =>
            {
                options.AddPolicy("CorsConfigure", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5001")
                        .WithMethods("POST", "DELETE", "PUT", "GET")
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            #endregion

            services.AddMvc()
                    .AddJsonOptions(options => { options.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss"; })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("CorsConfigure");
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
