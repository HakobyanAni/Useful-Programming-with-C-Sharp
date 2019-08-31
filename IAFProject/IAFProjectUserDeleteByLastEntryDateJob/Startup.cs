using IAFProject.BLL.General;
using IAFProjectUserDeleteByLastEntryDateJob.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace IAFProjectUserDeleteByLastEntryDateJob
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public static string ConnectionString { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Configurations.ConfigureUserDeleteProcessingServices(Configuration.GetConnectionString("IAFProjectDatabase"), services);
            ConfigureLocalServices(services);

            ServiceProvider container = services.BuildServiceProvider();
            JobFactory jobFactory = new JobFactory(container);

            // construct a scheduler factory
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(new System.Collections.Specialized.NameValueCollection());

            // get a scheduler
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            scheduler.JobFactory = jobFactory;

            scheduler.Start().GetAwaiter().GetResult();

            // define the job and tie it to UserDeleteJob class
            IJobDetail userDeleteJob = JobBuilder.Create<UserDeleteProcessorJob>()
                .Build();

            // trigger the job to run now, and then once a year
            ITrigger userDeleteJobTrigger = TriggerBuilder.Create()
                .StartNow()
                .WithCalendarIntervalSchedule(x => x.WithIntervalInYears(1))
                .Build();

            scheduler.ScheduleJob(userDeleteJob, userDeleteJobTrigger).Wait();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWelcomePage("/");
        }

        void ConfigureLocalServices(IServiceCollection services)
        {
            services.AddTransient<UserDeleteProcessorJob>();
        }
    }
}
