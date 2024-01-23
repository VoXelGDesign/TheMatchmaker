namespace Application.Exceptions.CustomExceptions;

public sealed class ResourceMissingException : BaseApplicationException
{
    internal ResourceMissingException()
        : base(ApplicationExceptionCodes.RESOURCE_MISSING())
    {
    }
}
