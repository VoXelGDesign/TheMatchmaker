namespace Application.Exceptions;

public class BaseApplicationException : Exception
{
    internal BaseApplicationException(string message) : base($"APPLICATION_{message}")
    {
    }
}

