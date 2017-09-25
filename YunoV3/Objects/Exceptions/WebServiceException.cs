using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects.Exceptions
{
    public class WebServiceException : Exception
    {

        public WebServiceException() { }

        public WebServiceException(string message)
            : base(message) { }

        public WebServiceException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
