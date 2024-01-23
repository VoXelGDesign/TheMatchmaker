using Domain.Exceptions;

namespace Infrastructure.Exceptions.CustomExceptions;

internal class InconsistencyInDatabaseException : BaseInfrastructureException
{
    internal InconsistencyInDatabaseException()
        : base(InfrastructureExceptionCodes.INCOSISTENCY_IN_DATABASE())
    {
    }
}
