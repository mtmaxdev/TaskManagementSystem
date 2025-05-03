using Moq;
using TaskManagementSystem.Application.Common.Exceptions;
using TaskManagementSystem.Application.Dtos;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Mapping;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Tests.UnitTests;

public class TaskServiceTests
{
    [Fact]
    public async Task Update_ShouldUpdateTaskStatusAndProcessEvents()
    {
        // Arrange
        var taskDto = new TaskDto(
            id: int.MaxValue,
            name: nameof(Update_ShouldUpdateTaskStatusAndProcessEvents),
            description: string.Empty,
            status: TaskStatusEnum.Completed
        );

        var taskEntity = taskDto.ToTaskEntity();
        taskEntity.Status = TaskStatusEnum.InProgress;

        var mockRepository = new Mock<ITaskRepository>();
        var mockEventService = new Mock<ITaskEventService>();

        mockRepository.Setup(r => r.GetById(taskDto.Id)).ReturnsAsync(taskEntity);

        IDomainEvent[] capturedEvents = null;
        mockEventService
            .Setup(s => s.ProcessEvents(It.IsAny<IReadOnlyCollection<IDomainEvent>>()))
            .Callback<IEnumerable<IDomainEvent>>(events =>
            {
                capturedEvents = events.ToArray();
            })
            .Returns(Task.CompletedTask);

        var service = new TaskService(mockRepository.Object, mockEventService.Object);

        // Act
        await service.Update(taskDto);

        // Assert
        mockRepository.Verify(r => r.Update(It.IsAny<TaskEntity>()), Times.Once);
        Assert.NotNull(capturedEvents);
        Assert.Single(capturedEvents);
        Assert.IsType<TaskCompletedEvent>(capturedEvents.First());
        Assert.Empty(taskEntity.DomainEvents);
    }

    [Fact]
    public async Task Update_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskDto = new TaskDto { Id = int.MaxValue };

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetById(taskDto.Id)).ReturnsAsync((TaskEntity)null!);

        var mockEventService = new Mock<ITaskEventService>();
        var service = new TaskService(mockRepository.Object, mockEventService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(taskDto));
    }
}
