using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagementSystem.Infrastructure.Persistence;

public class MigrationTaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    private const string ConnectionStringArgument = "connectionString=";

    public TaskDbContext CreateDbContext(string[] args)
    {
        var connectionArg = args.FirstOrDefault(a => a.StartsWith(ConnectionStringArgument));
        if (connectionArg == null)
        {
            throw new ArgumentException("Missing ConnectionString argument");
        }

        var connectionString = connectionArg[ConnectionStringArgument.Length..].Trim('"');
        
        Console.WriteLine(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new TaskDbContext(optionsBuilder.Options);
    }
}
