using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using EmployeeManager.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Mvc.Security;

namespace EmployeeManager.Mvc
{
    public class Startup
    {
        private IConfiguration config = null;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(this.config.GetConnectionString("AppDb")));
            services.AddDbContext<AppIdentityDbContext>(options =>  options.UseSqlServer(this.config.GetConnectionString("AppDb")));

            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Security/SignIn";
                opt.AccessDeniedPath = "/Security/AccessDenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=EmployeeManager}/{action=List}/{id?}");
            });
        }
    }
}
