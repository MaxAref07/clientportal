namespace ClientPortal.Application.Exceptions;

public class InvalidFeatureStatusTransitionException : Exception
{
    public InvalidFeatureStatusTransitionException() : base("Invalid feature status transition") { }

    public InvalidFeatureStatusTransitionException(string message) : base(message) { }

    public InvalidFeatureStatusTransitionException(string message, Exception innerException) 
        : base(message, innerException) { }
}