using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.Repository;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using Plan_current_repairs.Jobs;
using Plan_current_repairs.Extensions;

namespace Plan_current_repairs
{
    public class Startup
    {
        static public string NameProject="/Plan_current_repairs.com";
        public IConfiguration Configuration { get; }


     
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            // добавл€ем контекст ApplicationContext в качестве сервиса в приложение
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connection));

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Home/Login");
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policyBuilder => policyBuilder.RequireClaim("Role", "Admin"));
                options.AddPolicy("IsReviewer", policyBuilder => policyBuilder.RequireClaim("Role", "Reviewer"));
                options.AddPolicy("IsUser", policyBuilder => policyBuilder.RequireClaim("Role", "User"));
            });
            services.AddTransient<IDepartment, DepartmentRepository>();
            services.AddTransient<IEmployee, EmployeeRepository>();
            services.AddTransient<IWorks, WorksRepository>();
            services.AddTransient<IJornal, JornalRepository>();
            services.AddTransient<IYears, YearsRepository>();
            services.AddTransient<IStatus, StatusRepository>();
            services.AddTransient<IAct, ActRepository>();
            services.AddTransient<IDictionarySector, DictionarySectorRepository>();
            services.AddTransient<IEmailService, EmailService>();

            services.Configure<Settings>(Configuration.GetSection("SettingsApp"));

            services.AddQuartz(q =>
            {
                //var newYearKey = new JobKey("Adding new year to the application");

                //q.AddJob<AddNewYearInAppJob>(opts => opts.WithIdentity(newYearKey));
                //q.AddTrigger(opts => opts.ForJob(newYearKey).WithIdentity(newYearKey.Name).WithCronSchedule("0 0 8 1 1 ? *"));

                var SendingMessageKey = new JobKey("«апланированна€ отправка напоминаний");
                q.AddJob<SendingMessageJob>(opts => opts.WithIdentity(SendingMessageKey));
                q.AddTrigger(opts => opts.ForJob(SendingMessageKey).WithIdentity(SendingMessageKey.Name).WithCronSchedule("0 0 8 10 * ? *"));
                q.AddTrigger(opts => opts.ForJob(SendingMessageKey).WithIdentity(SendingMessageKey.Name).WithCronSchedule("0 0 8 20,25 1 ? *"));
                q.AddTrigger(opts => opts.ForJob(SendingMessageKey).WithIdentity(SendingMessageKey.Name).WithCronSchedule("0 0 8 25 12 ? *"));
                //q.AddTrigger(opts => opts.ForJob(SendingMessageKey).WithIdentity(SendingMessageKey.Name).WithCronSchedule("0 */5 * * * ?")); //отправка каждые 5 минут проверка
                //q.AddTrigger(opts => opts.ForJob(SendingMessageKey).WithIdentity(SendingMessageKey.Name).WithCronSchedule("0 0 8 * 1 ?"));   //отправка каждый день в 8 утра
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddTransient<SendingMessageJob>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UsePathBase("/Plan_current_repairs.com");
            //env.EnvironmentName = "Production";
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders();
                app.UseHsts();
            }
            else
            {
                NameProject = "";
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
          
            app.UseStatusCodePages();
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoits =>
            {
                endpoits.MapControllerRoute(
                    name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}