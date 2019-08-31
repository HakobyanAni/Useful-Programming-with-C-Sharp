using IAFProject.Authentication;
using IAFProject.BLL.Interfaces;
using IAFProject.BLL.UserDeleteProcessing;
using IAFProject.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IAFProject.BLL.General
{
    public class Configurations
    {
        public static void ConfigureUserDeleteProcessingServices(string connectionString, IServiceCollection services)
        {
            services.AddDbContext<IAFProjectDBContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<UserManager<User>>();
            services.AddTransient<IUserDeleteProcessor, UserDeleteProcessor>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
        }
    }
}