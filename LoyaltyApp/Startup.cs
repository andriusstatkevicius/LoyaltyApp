using LoyaltyAppData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection;

namespace LoyaltyApp
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
            //services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));

            services.AddDbContext<AccessUserDBContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("IdentityContext"),
                                                       sql => sql.MigrationsAssembly(typeof(Startup)
                                                                 .GetTypeInfo().Assembly.GetName().Name)));

            services.AddScoped<IUserStore<AccessUser>, UserOnlyStore<AccessUser, AccessUserDBContext>>();

            services.AddDefaultIdentity<AccessUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AccessUserDBContext>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<LoyaltyAppContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("LoyaltyAppContext")));


            services.AddAuthentication("cookies")
                    .AddCookie("cookies",
                               options =>
                               {
                                   options.LoginPath = "/home/login";
                               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AbleToIssueACard", policy => policy.RequireRole("Issuer"));
                options.AddPolicy("AbleRecordTransaction", policy => policy.RequireRole("Recorder"));
            });
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
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
