using TaskManagementSystem.Api.Controllers;
using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.ConfigureAppLogging(builder.Configuration);

builder.RegisterServices();

var app = builder.Build();

app.Services.InitDatabase();

app.RegisterMiddlewares();

app.RegisterTaskEndpoints();

app.Run();