using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789*-._@+/";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();
        }
        public static void AddConfigureApplicationCookieExtension(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                var cookieBuilder = new CookieBuilder();
                cookieBuilder.Name = "AppCookie";
                options.LoginPath = new PathString("/Home/SignIn");
                options.LogoutPath = new PathString("/Member/Logout");
                options.Cookie = cookieBuilder;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });
        }
    }
}