using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hqs.IRepository;
using Hqs.Model.Users;
using Hqs.Repository.SqlServer;
using Hqs.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hqs.WebApi
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
            #region 依赖注入

            var assembly = Assembly.GetAssembly(typeof(User));
            var connectionString = Configuration["ConnectionStrings:SqlServer"];
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(connectionString));

            //注册数据库上下文
            services.AddTransient<IDataContext, SqlServerContext>();
            services.AddTransient(typeof(IBaseRepository<>), typeof(EfRepository<>));

            //注册HttpContext
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //注册Repository
            foreach (var item in GetClassName("Hqs.Repository.SqlServer"))
            {
                foreach (var typeArray in item.Value)
                {
                    if (typeArray.Name == typeof(IBaseRepository<>).Name)
                        continue;
                    services.AddTransient(typeArray, item.Key);
                }
            }

            //注册Service
            foreach (var item in GetClassName("Hqs.Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddTransient(typeArray, item.Key);
                }
            }

            #endregion

            #region 授权

            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration["AppSetting:AuthServer"];
                        options.RequireHttpsMetadata = false;   //不需要https    
                        options.ApiName = Configuration["AppSetting:ApiName"];    //api的name，需要和config的名称相同
                    });

            #endregion

            #region 跨域

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            #endregion

            services.AddMvc(options => options.EnableEndpointRouting = false)
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

            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseAuthentication();
            app.UseCors("AllowAllOrigin");
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
