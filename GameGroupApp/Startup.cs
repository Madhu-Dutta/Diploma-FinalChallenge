using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using GameGroupApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GameGroupApp.Data.Policies;
using GameGroupApp.Data.Claims;

namespace GameGroupApp
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
             .AddGoogle(options =>
             {
                 IConfigurationSection googleAuthNSection =
                     Configuration.GetSection("Authentication:Google");
                 options.ClientId = "933774729644-han4fo3g5gfn7mpp072mtl84m0nn0pvk.apps.googleusercontent.com";
                 options.ClientSecret = "4d8PQHaM3gBYswjyOKLlRlx0";
             });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    MemberPolicies.Approved,
                    policyBuilder => policyBuilder
                        .RequireClaim(MemberClaims.MemberStatus, MemberStatus.Approved.ToString()));
            });

            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/AllRecords");
                options.Conventions.AuthorizeFolder("/Approvals");
                options.Conventions.AuthorizeFolder("/Expenses");
                options.Conventions.AuthorizeFolder("/PaidUnPaid");
            });
            services.AddControllersWithViews();

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/AllRecords", MemberPolicies.Approved);
                options.Conventions.AuthorizeFolder("/Expenses", MemberPolicies.Approved);
                options.Conventions.AuthorizeFolder("/PaidUnPaid", MemberPolicies.Approved);
                options.Conventions.AuthorizeFolder("/Approvals", MemberPolicies.Approved);
            });

            //services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
