using MediatR;

namespace ClientPortal.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Recourse was not found") { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}