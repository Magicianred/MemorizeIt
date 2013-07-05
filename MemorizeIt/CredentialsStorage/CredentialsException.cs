using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemorizeIt.CredentialsStorage
{
    public class CredentialsException:Exception
    {
        public CredentialsException()
        {
        }

        public CredentialsException(string message) : base(message)
        {
        }

        public CredentialsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
