﻿using System.Reflection;
using AutoMapper;
using Hqs.Framework.Filters;
using Hqs.Framework.Middlewares;
using Hqs.Helper;
using Hqs.Model.Users;
using Hqs.Service;
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
            var connectionStringForMssql = Configuration["ConnectionStrings:SqlServer"];
            var connectionStringForMySql = Configuration["ConnectionStrings:MySql"];
            var connectionString = Configuration["ConnectionStrings:DataType"] == "MySql"
                ? connectionStringForMySql
                : connectionStringForMssql;
            //UseRowNumberForPaging(),解决EF使用skip、take报错问题
            services.AddDbContext<DataContext>(options => options.UseMySql(connectionString));

            //注册数据库上下文
            services.AddTransient<DbContext, DataContext>();

            //注册HttpContext
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //注册Service
            foreach (var item in Util.GetClassName("Hqs.Service"))
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
                        options.Authority = Configuration["AuthConfig:AuthServer"];
                        options.RequireHttpsMetadata = false;   //不需要https    
                        options.ApiName = Configuration["AuthConfig:ApiName"];    //api的name，需要和config的名称相同
                    });

            #endregion

            #region 跨域

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:8080", "http://http://devhqs.vicp.io")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            #endregion

            services.AddDIHelper();
            services.AddAutoMapper();   //注册AutoMapper
            services.AddMemoryCache();  //注册内存缓存
            services.AddOptions();      //注册Options绑定Appsetting
            services.Configure<AppSetting>(Configuration);
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ActionFilter>();
                })
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
            
            app.UseCors("AllowAllOrigin");
            app.UseDIHelper();
            //app.UseMiddleware(typeof(ApiValidateMiddleware));
            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
