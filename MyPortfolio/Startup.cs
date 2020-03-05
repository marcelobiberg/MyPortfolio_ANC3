using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MyPortfolio.Data;
using MyPortfolio.Helpers;
using MyPortfolio.Models;

namespace MyPortfolio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private async Task CreateUserDefault(IServiceProvider serviceProvider)
        {
            //... Seed para user
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var email = Configuration["User:Email"];

            var result = await userManager.FindByEmailAsync(email);
            if (result == null)
            {
                //Variáveis do sistema
                var userId = Guid.NewGuid().ToString();
                var Name = Configuration["User:Name"];
                var UserName = Configuration["User:UserName"];
                var Email = Configuration["User:Email"];
                var Pwd = Configuration["User:Pwd"];
                var Cargo = Configuration["User:Cargo"];
                var Avatar = Configuration["User:Avatar"];
                var AboutDescription = Configuration["User:AboutDescription"];

                //Cria o usuário padrão
                var User = new ApplicationUser
                {
                    Id = userId,
                    Nome = Name,
                    Email = Email,
                    UserName = UserName,
                    Ativo = true,
                    Cargo = Cargo,
                    AboutDescription = AboutDescription,
                    Avatar = Avatar,
                    CreatedOn = DateTime.UtcNow
                };
                await userManager.CreateAsync(User);
                await userManager.AddPasswordAsync(User, Pwd);
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StrCon")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AcessoNegado";
                options.SlidingExpiration = true;
            });

            services.AddMemoryCache();
            services.AddSession();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddControllersWithViews();

            services.AddScoped<ProfileManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Site}/{action=Index}");
            });

            //Cria user padrão
            CreateUserDefault(serviceProvider).Wait();
        }
    }
}
