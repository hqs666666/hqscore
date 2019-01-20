using System.Reflection;
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
            var connectionString = Configuration["ConnectionStrings:SqlServer"];
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

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

            services.AddDIHelper();
            services.AddAutoMapper();   //注册AutoMapper
            services.AddMemoryCache();  //注册内存缓存
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

            app.UseDIHelper();
            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseAuthentication();
            app.UseCors("AllowAllOrigin");
            app.UseMvc();
        }
    }
}
