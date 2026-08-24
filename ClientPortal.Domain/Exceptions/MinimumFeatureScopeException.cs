namespace ClientPortal.Domain.Exceptions;

public class MinimumFeatureScopeException : Exception
{
    public MinimumFeatureScopeException(int featuresCount, int newFeatureScope) : base($"Project already has {featuresCount} features, can't reduce feature scope to {newFeatureScope}") { }

    public MinimumFeatureScopeException(string message) : base(message) { }

    public MinimumFeatureScopeException(string message, Exception innerException) 
        : base(message, innerException) { }
}