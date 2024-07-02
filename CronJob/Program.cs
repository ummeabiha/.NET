using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var schedulerFactory = serviceProvider.GetService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        scheduler.JobFactory = new SimpleJobFactory(serviceProvider);

        var job = JobBuilder.Create<DatabaseJob>()
            .WithIdentity("databaseJob", "group1")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("databaseTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(60)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        await scheduler.Start();

        Console.WriteLine("Press any key to close the application");
        Console.ReadKey();

        await scheduler.Shutdown();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        services.AddSingleton<IJobFactory, SimpleJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<DatabaseJob>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Server=DESKTOP-3MORQJH;Database=Teachers;Trusted_Connection=True;Encrypt=False;"));
    }
}

public class SimpleJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job) { }
}
