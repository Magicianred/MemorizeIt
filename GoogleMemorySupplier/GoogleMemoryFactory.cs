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
    public class GoogleMemoryFactory : IMemoryFactory
    {
        private readonly ICredentialsStorage _credentials=new GoogleCredentials();

        public ICredentialsStorage CredentialsStorage {
            get { return _credentials; }
        }

        public MemoryTable DownloadMemories(string source)
        {
            if(!_credentials.IsLoggedIn)
                throw new InvalidOperationException("user need to be logged in");
            var user = _credentials.GetCurrentUser();
            return new GoogleMemorySourceSupplier(source, user.Login, user.Password).Download();
        }

        public IEnumerable<string> ListOfSources
        {
            get
            {

				if (!_credentials.IsLoggedIn)
					return Enumerable.Empty<string> ();
         
                var user = _credentials.GetCurrentUser();
                return new GoogleListOfSourcesSupplier(user.Login, user.Password).GetSourcesList();
            }
        }
    }
}
