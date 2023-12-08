

namespace Application.Exceptions.CustomExceptions
{
    internal sealed class ResourceCreationFailed : BaseApplicationException
    {
        internal ResourceCreationFailed() : 
            base(ApplicationExceptionCodes.RESOURCE_CREATION_FAILED())
        {
        }
    }
}
