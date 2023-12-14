

namespace Application.Exceptions.CustomExceptions
{
    internal sealed class ResourceCreationFailedException : BaseApplicationException
    {
        internal ResourceCreationFailedException() : 
            base(ApplicationExceptionCodes.RESOURCE_CREATION_FAILED())
        {
        }
    }
}
