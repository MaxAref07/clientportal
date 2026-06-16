namespace ClientPortal.Application.Exceptions;

public class MinimumFeatureScopeException : Exception
{
    public MinimumFeatureScopeException(int featuresCount, int newFeatureScope) : base($"Project already has {featuresCount} feature scope, can't reduce to {newFeatureScope}") { }

    public MinimumFeatureScopeException(string message) : base(message) { }

    public MinimumFeatureScopeException(string message, Exception innerException) 
        : base(message, innerException) { }
}