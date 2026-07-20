namespace ClientPortal.Application.Exceptions;

public class InvalidMagicLinkException : Exception
{
    public InvalidMagicLinkException() : base("Invalid magic link") { }
    
    public InvalidMagicLinkException(string message) : base(message) { }

    public InvalidMagicLinkException(string message, Exception innerException) 
        : base(message, innerException) { }
}