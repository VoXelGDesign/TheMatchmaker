﻿namespace Application.Exceptions
{
    public class ApplicationExceptionCodes
    {
        internal static string ID_CLAIM_NOT_FOUND()
            => nameof(ID_CLAIM_NOT_FOUND);
        internal static string RESOURCE_CREATION_FAILED()
            => nameof(RESOURCE_CREATION_FAILED);
        internal static string RESOURCE_MISSING() 
            => nameof(RESOURCE_MISSING);
    }
}


