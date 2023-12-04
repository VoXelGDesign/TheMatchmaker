namespace Application.Exceptions.CustomExceptions
{
    internal class IdClaimNotFound : BaseApplicationException
    {
        internal IdClaimNotFound() 
            : base(ApplicationExceptionCodes.ID_CLAIM_NOT_FOUND())
        {
        }
    }
}
