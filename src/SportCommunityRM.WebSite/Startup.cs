using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Data;
using SportCommunityRM.WebSite.Data.Migrations;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.WorkerServices;
using System.Globalization;
using System.Linq;

namespace SportCommunityRM.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SCRMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDatabase, Database>(_ => new Database(new SCRMContext(Configuration.GetConnectionString("DefaultConnection"))));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUrlService, UrlService>();
            services.AddScoped<IStorageService, LocalStorageService>();

            services.AddScoped<AccountControllerWorkerServices>();
            services.AddScoped<HomeControllerWorkerServices>();
            services.AddScoped<ManageControllerWorkerServices>();
            services.AddScoped<CoachControllerWorkerServices>();
            services.AddScoped<TeamControllerWorkerServices>();
            services.AddScoped<UserControllerWorkerServices>();

            services.AddLocalization(options => options.ResourcesPath = nameof(Resources));

            services.AddAuthorization(options =>
            {
                foreach (var claim in ClaimPoliciesConstants.AvailableClaims)
                    options.AddPolicy(claim.Key, policy => policy.RequireClaim(claim.Value, claim.Value));
            });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddPaging(options => options.ViewName = "Bootstrap3");
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            
            var cultureInfos = appSettings.SupportedLanguages
                .Select(fourLettersIsoLanguage => new CultureInfo(fourLettersIsoLanguage))
                .ToArray();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(appSettings.SupportedLanguages.First()),
                SupportedCultures = cultureInfos,
                SupportedUICultures = cultureInfos
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            //TODO: comment when apply migration
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                ApplicationDbContextSeed.InitializeAdminsAsync(serviceScope).Wait();
            }
        }
    }
}
