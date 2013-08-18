using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt;
using MemorizeIt.MemorySourceSupplier;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;

namespace GoogleMemorySupplier
{
    public class GoogleMemoryFactory : AbstractMemoryFactory
    {
        private readonly ICredentialsStorage credentials=new GoogleCredentials();

        public ICredentialsStorage CredentialsStorage {
            get { return credentials; }
        }

        protected override IMemorySourceSupplier CreateMemorySourceSupplier()
        {
            if (!credentials.IsLoggedIn)
                throw new InvalidOperationException("user need to be logged in");
            var user = credentials.GetCurrentUser();
            return new GoogleMemorySourceSupplier(user.Login, user.Password);
        }
    }
}
