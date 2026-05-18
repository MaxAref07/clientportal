namespace ClientPortal.Application.Exceptions;

public class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException() : base("Project was not found") { }

    public ProjectNotFoundException(string message) : base(message) { }

    public ProjectNotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}