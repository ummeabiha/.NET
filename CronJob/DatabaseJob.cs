using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class DatabaseJob : IJob
{
    private readonly ILogger<DatabaseJob> _logger;
    private readonly ApplicationDbContext _dbContext;

    public DatabaseJob(ILogger<DatabaseJob> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var data = new Teachers
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            HireDate = DateTime.UtcNow,
            Salary = 50000,
            Department = "Math",
            Address = "123 Main St",
            City = "Anytown"
        };

        _dbContext.Teachers.Add(data);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Inserted record at {DateTime.Now}");

        await Task.CompletedTask;
    }
}
