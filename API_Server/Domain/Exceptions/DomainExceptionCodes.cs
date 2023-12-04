using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    internal class DomainExceptionCodes
    {
        internal static string INVALID_ARGUMENT() 
            => nameof(INVALID_ARGUMENT);
    }
}
