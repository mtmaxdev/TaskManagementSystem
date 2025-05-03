using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using TaskManagementSystem.Api.Middlewares;
using TaskManagementSystem.Api.Validation;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.EventHandler;
using TaskManagementSystem.Infrastructure.Messaging;
using TaskManagementSystem.Infrastructure.Persistence.Extensions;
using TaskManagementSystem.Infrastructure.Persistence.Repositories;

namespace TaskManagementSystem.Api.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        var rabbitMqSettings = builder
            .Configuration.GetSection(nameof(RabbitMqSettings))
            .Get<RabbitMqSettings>();

        builder.Services.AddSingleton(rabbitMqSettings);

        RegisterSwagger(builder);

        builder.Services.AddValidatorsFromAssemblyContaining<AddTaskRequestValidation>(
            ServiceLifetime.Transient
        );

        builder.Services.AddTaskDatabase(
            builder.Configuration.GetConnectionString("TaskManagementSystemDb")
        );

        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ITaskEventService, TaskEventService>();
        builder.Services.AddScoped<IEventPublisher, RabbitMqPublisher>();

        builder.Services.AddHostedService<TaskEventConsumer>();

        //Health checks
        builder.Services.AddHealthChecks();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        // Swagger middleware
        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management System");
                c.RoutePrefix = string.Empty;
            });

        //Developer exception page on dev environment
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        //HealthCheck Middleware
        app.MapHealthChecks("api/healthz");
    }

    private static void RegisterSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

        builder.Services.AddRouting(options =>
        {
            options.ConstraintMap["regex"] =
                typeof(Microsoft.AspNetCore.Routing.Constraints.RegexRouteConstraint);
        });
    }
}
