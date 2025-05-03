using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementSystem.Infrastructure.Persistence.Extensions;

public static class ConfigurationExtensions
{
    public static void AddTaskDatabase(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException("ConnectionString for Task Database is missing");
        }

        services.AddDbContext<TaskDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)))
        );
    }

    public static void AddInMemoryTaskDatabase(this IServiceCollection services)
    {
        services.AddDbContext<TaskDbContext>(options =>
            options.UseInMemoryDatabase("TaskManagementSystemDb")
        );
    }

    public static void InitDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
        dbContext.Database.Migrate();
    }
}
