using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.CustomExceptions
{
    internal class ResourceCreationFailed : BaseApplicationException
    {
        internal ResourceCreationFailed() : 
            base(ApplicationExceptionCodes.RESOURCE_CREATION_FAILED())
        {
        }
    }
}
