namespace Domain.Exceptions
{
    public class BaseDomainException : Exception
    {
        internal BaseDomainException(string message) : base($"DOMAIN_{message}")
        {
        }
    }
}

