namespace ClientPortal.Domain.Exceptions;

public class FeaturesOutOfScopeException : Exception
{
    public FeaturesOutOfScopeException() : base("Project has exceeded the features limit") { }

    public FeaturesOutOfScopeException(string message) : base(message) { }

    public FeaturesOutOfScopeException(string message, Exception innerException) 
        : base(message, innerException) { }
}