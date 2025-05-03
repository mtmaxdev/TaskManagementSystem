using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Mapping;
using TaskManagementSystem.Api.Models.Requests;
using TaskManagementSystem.Api.Models.Response;
using TaskManagementSystem.Api.Validation;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Api.Controllers;

public static class TaskEndpoints
{
    public static void RegisterTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var tasksRoutes = routes.MapGroup("/api/tasks");

        tasksRoutes
            .MapGet(
                "/{taskId:int}",
                async (ITaskService service, int taskId) =>
                {
                    var task = await service.GetById(taskId);

                    return task.ToTaskResponse();
                }
            )
            .WithName("GetTaskById")
            .WithSummary("Get Task by Id")
            .Produces<TaskResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        tasksRoutes
            .MapGet(
                "",
                async (
                    ITaskService service,
                    IValidator<GetTasksRequest> validator,
                    [AsParameters] GetTasksRequest request
                ) =>
                {
                    (await validator.ValidateAsync(request)).ThrowExceptionIfNotValid();

                    var tasks = await service.Get(request.Page, request.Size);

                    return tasks.ToTasksResponse();
                }
            )
            .WithName("GetTasks")
            .WithSummary("Get Tasks with pagination")
            .WithDescription("'Page' must be greater than 0, 'Size' - [1-50]")
            .Produces<TasksResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        tasksRoutes
            .MapPost(
                "",
                async (
                    ITaskService service,
                    IValidator<AddTaskRequest> validator,
                    [FromBody] AddTaskRequest request
                ) =>
                {
                    (await validator.ValidateAsync(request)).ThrowExceptionIfNotValid();

                    var taskId = await service.Add(request.ToTaskDto());

                    return Results.Created($"/tasks/{taskId}", new TaskCreatedResponse(taskId));
                }
            )
            .WithName("CreateNewTask")
            .WithSummary("Create new Task")
            .Produces<TaskCreatedResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        tasksRoutes
            .MapPost(
                "/{taskId:int}",
                async (
                    ITaskService service,
                    IValidator<UpdateTaskRequest> validator,
                    int taskId,
                    [FromBody] UpdateTaskRequest request
                ) =>
                {
                    (await validator.ValidateAsync(request)).ThrowExceptionIfNotValid();

                    await service.Update(request.ToTaskDto(taskId));
                }
            )
            .WithName("Update Task")
            .WithSummary("Update task by Id")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
