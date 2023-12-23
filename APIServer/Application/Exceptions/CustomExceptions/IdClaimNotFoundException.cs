namespace Application.Exceptions.CustomExceptions
{
    internal sealed class IdClaimNotFoundException : BaseApplicationException
    {
        internal IdClaimNotFoundException() 
            : base(ApplicationExceptionCodes.ID_CLAIM_NOT_FOUND())
        {
        }
    }
}
