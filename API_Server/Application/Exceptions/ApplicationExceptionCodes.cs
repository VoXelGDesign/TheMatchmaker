using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    internal class ApplicationExceptionCodes
    {
        internal static string ID_CLAIM_NOT_FOUND()
            => nameof(ID_CLAIM_NOT_FOUND);

        internal static string RESOURCE_CREATION_FAILED()
            => nameof(RESOURCE_CREATION_FAILED);
    }
}


