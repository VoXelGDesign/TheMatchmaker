namespace Application.Exceptions
{
    internal class BaseApplicationException : Exception
    {
        internal BaseApplicationException(string message) : base($"APPLICATION_{message}")
        {
        }
    }

}

