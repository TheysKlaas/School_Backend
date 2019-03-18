using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Models.Data;
using School_Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Logging;

namespace School_Models
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.RollingFile("Serilogs/DBSchool-{Date}.txt").CreateLogger();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //reg context
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SchoolDBContext>(options => options.UseSqlServer(connectionString));

            //registratie repositories 
            services.AddScoped<IDataInitializer, DataInitializer>();
            //services.AddScoped<IStudentRepo, StudentRepo_Fake>();
            services.AddScoped<IStudentRepo, StudentRepo_SQL>(); //laatste wordt uitgvoerd
            services.AddScoped<IEducationRepo, EducationRepo_SQL>();
            services.AddScoped<ITeacherRepo, TeacherRepo>();
            services.AddScoped<ITeacherRepo, TeacherRepo>();
            services.AddScoped<INewsRepo, NewsRepo>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(); //default arg is de Log.Logger 
            Log.Logger.Warning("Serilog Warning test"); //test only

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Student}/{action=IndexAsync}");
            });
        }
    }
}
