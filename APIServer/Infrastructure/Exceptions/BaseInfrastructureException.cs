namespace Infrastructure.Exceptions;

public class BaseInfrastructureException : Exception
{
    internal BaseInfrastructureException(string message) : base($"INFRASTRUCTURE_{message}")
    {
        
    }
}
