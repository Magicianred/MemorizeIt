using System;

namespace MemorizeIt.DictionarySourceSupplier.CredentialsStorage
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
