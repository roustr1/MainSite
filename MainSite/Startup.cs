using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Dal;
using Application.Services.Menu;
using Application.Services.News;
using Microsoft.EntityFrameworkCore;

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

            services.AddControllersWithViews(mvcOtions =>
            {
                mvcOtions.EnableEndpointRouting = false;
            });
            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<IShowMenu, MenuService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<INewsService, NewsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(endpoints =>
            {
                endpoints.MapRoute(
                    null,
                    "",
                    defaults: new { controller = "Home", action = "News", page = 1 }
                );
                endpoints.MapRoute(
                    name: null, "Page{page}",
                    defaults: new { controller = "Home", action = "News", page = (string)null },
                    constraints: new { page = @"/d+" }
                );
                endpoints.MapRoute(
                    null, "{News}",
                    new { controller = "Home", action = "News", page = 1 },
                    new { page = @"\d+" }
                );


                endpoints.MapRoute(
                    null, "{News}/Page{page}",
                    new { controller = "Home", action = "News" },
                    new { page = @"\d+" }
                );

                endpoints.MapRoute(
                    null, "{controller}/{action}"
                );


                //endpoints.MapRoute(name: "default",
                //    template: "{controller=Home}/{action?}/{id?}");


            });
        }
    }
}
