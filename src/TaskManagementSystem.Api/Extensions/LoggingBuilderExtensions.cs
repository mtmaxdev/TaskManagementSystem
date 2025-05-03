namespace TaskManagementSystem.Api.Extensions;

public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Method for configuring logging in the application (e.g. error tracking systems or cloud consoles)
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static ILoggingBuilder ConfigureAppLogging(
        this ILoggingBuilder builder,
        IConfiguration configuration
    )
    {
        // configure logging, like a console for debugging and cloud logging for production
        builder.AddConsole();

        return builder;
    }
}
