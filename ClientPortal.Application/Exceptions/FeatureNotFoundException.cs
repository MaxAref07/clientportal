namespace ClientPortal.Application.Exceptions;

public class FeatureNotFoundException : NotFoundException
{
    public FeatureNotFoundException() : base("Feature was not found") { }

    public FeatureNotFoundException(string message) : base(message) { }

    public FeatureNotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}