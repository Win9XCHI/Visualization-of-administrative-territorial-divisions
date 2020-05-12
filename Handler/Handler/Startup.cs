using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Handler.Models.Repositories;
using Handler.Models.Repositories.Interfaces;

namespace Handler {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            string connectionString = "Data Source=DESKTOP-R19OFME;Initial Catalog=MyDB2;Integrated Security=True";
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>(provider => new AuthenticationRepository(connectionString));
            services.AddTransient<IHome_mapRepository, Home_mapRepository>(provider => new Home_mapRepository(connectionString));
            services.AddTransient<ISearchRepository, SearchRepository>(provider => new SearchRepository(connectionString));
            services.AddTransient<IUserPanelRepository, UserPanelRepository>(provider => new UserPanelRepository(connectionString));
            services.AddTransient<ISoursePanelRepository, SoursePanelRepository>(provider => new SoursePanelRepository(connectionString));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            /*app.Map("/index", Index);
    app.Map("/about", About);
    */

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /*private static void Index(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Index");
    });
}
private static void About(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("About");
    });
}*/
    }
}
