namespace Domain.Exceptions.CustomExceptions
{
    internal sealed class InvalidArgumentException : BaseDomainException
    {
        internal InvalidArgumentException() 
            : base(DomainExceptionCodes.INVALID_ARGUMENT())
        {
        }
    }
}
