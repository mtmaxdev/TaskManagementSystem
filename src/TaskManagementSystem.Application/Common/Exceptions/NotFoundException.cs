namespace TaskManagementSystem.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base() { }

    public NotFoundException(int id)
        : base($"Object '{id}' is not found") { }
}
