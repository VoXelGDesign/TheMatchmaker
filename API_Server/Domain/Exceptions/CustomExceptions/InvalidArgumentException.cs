namespace Domain.Exceptions.CustomExceptions
{
    internal class InvalidArgumentException : BaseDomainException
    {
        internal InvalidArgumentException() 
            : base(DomainExceptionCodes.INVALID_ARGUMENT())
        {
        }
    }
}
