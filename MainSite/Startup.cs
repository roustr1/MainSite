using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Dal;
using Application.Dal.Infrastructure;
using Application.Services.Files;
using Application.Services.Menu;
using Application.Services.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Services.Birthday;
using Application.Services.Permissions;
using Application.Services.Settings;
using MainSite.Models;
using Application.Services.Users;
using MainSite.Areas.Admin.Factories;
using Microsoft.AspNetCore.Server.HttpSys;
using Application.Services.PlanCalendar;

namespace MainSite
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

            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));

            services.AddControllersWithViews();
            services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            services.AddControllersWithViews(mvcOtions =>
            {
                mvcOtions.EnableEndpointRouting = false;
            });
            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<MainModel, MainModel>();
            services.AddTransient<ConfigDb, ConfigDb>();
            services.AddTransient<FirstConfigService, FirstConfigService>();
            services.AddTransient<PinNewsService, PinNewsService>();
            services.AddTransient<IShowMenu, MenuService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IFileDownloadService, FileDownloadService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<IAppFileProvider, AppFileProvider>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<ISettingsService, SettingsService>();

            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IBirthdayService, BirthdayService>();
            services.AddTransient<IBirthdayFactory, BirthdayFactory>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IPlanCalendarSevice, PlanCalendarSevice>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IPlanCalendarFactory, PlanCalendarFactory>();
            services.AddTransient<ISecurityModelFactory, SecurityModelFactory>();
            services.AddTransient<IUserRoleModelFactory, UserRoleModelFactory>();
            services.AddTransient<IUserModelFactory, UserModelFactory>();

            services.AddTransient<PlanCalendarRepository>();
            services.AddTransient<NewsItemRepository>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseAuthentication();


            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Content")),
            //    RequestPath = "/files"
            //});
            app.UseRouting();
            app.UseMvc(routes =>
            {
         
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new {controller = "Home", action = "Index"});
            });

        }
    }
}
