using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using IAFProject.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IAFProject.Authentication
{
    public static class ServiceConfiguration
    {
        public static void Configure(string connectionString, IServiceCollection services)
        {
            services.AddDbContext<IAFProjectDBContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<UserManager<User>>();
            services.AddTransient<SignInManager<User>>();
            services.AddTransient<AccountService>();
            services.AddTransient<EmailSenderService>();
        }

        public static void Authenticate(IServiceCollection services, string appSettingsSecret)
        {
            var key = Encoding.ASCII.GetBytes(appSettingsSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-:@.";
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

            }).AddEntityFrameworkStores<IAFProjectDBContext>().AddDefaultTokenProviders();
        }
    }
}
