using System.Reflection;
using AutoMapper;
using Hqs.AuthServer.AuthValidator;
using Hqs.Framework.Filters;
using Hqs.Framework.Middlewares;
using Hqs.Helper;
using Hqs.Model.Users;
using Hqs.Service;
using IdentityServer4.Dapper.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            var connectionString = Configuration["ConnectionStrings:SqlServer"];
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            
            #region 依赖注入

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

            #region 配置ids4

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddDapperStore(options => { options.DbConnectionString = connectionString; })
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
            
            services.AddDIHelper();     //添加依赖注册扩展
            services.AddAutoMapper();   //注册AutoMapper
            services.AddMemoryCache();  //注册内存缓存
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ActionFilter>();        //添加过滤器
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
            
            app.UseDIHelper();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("CorsConfigure");
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
