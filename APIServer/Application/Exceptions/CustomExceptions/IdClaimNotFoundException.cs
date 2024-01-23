namespace Application.Exceptions.CustomExceptions;

public sealed class IdClaimNotFoundException : BaseApplicationException
{
    internal IdClaimNotFoundException() 
        : base(ApplicationExceptionCodes.ID_CLAIM_NOT_FOUND())
    {
    }
}
