using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportCommunityRM.WebSite.Data;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.WebSite.WorkerServices;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace SportCommunityRM.WebSite
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SCRMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDatabase, Database>(_ => new Database(new SCRMContext(Configuration.GetConnectionString("DefaultConnection"))));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUrlService, UrlService>();

            services.AddScoped<AccountControllerWorkerServices>();
            services.AddScoped<HomeControllerWorkerServices>();
            services.AddScoped<ManageControllerWorkerServices>();
            services.AddScoped<CoachControllerWorkerServices>();
            services.AddScoped<TeamControllerWorkerServices>();

            services.AddLocalization(options => options.ResourcesPath = nameof(Resources));

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddPaging(options => options.ViewName = "Bootstrap3");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }
    }
}
