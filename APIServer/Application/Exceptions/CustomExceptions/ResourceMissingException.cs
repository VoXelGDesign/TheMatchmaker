namespace Application.Exceptions.CustomExceptions
{
    internal sealed class ResourceMissingException : BaseApplicationException
    {
        internal ResourceMissingException()
            : base(ApplicationExceptionCodes.RESOURCE_MISSING())
        {
        }
    }
}
