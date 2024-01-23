

namespace Application.Exceptions.CustomExceptions;

public sealed class ResourceCreationFailedException : BaseApplicationException
{
    internal ResourceCreationFailedException() : 
        base(ApplicationExceptionCodes.RESOURCE_CREATION_FAILED())
    {
    }
}
