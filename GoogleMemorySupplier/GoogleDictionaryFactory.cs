using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt;
using MemorizeIt.DictionarySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;
using MemorizeIt.DictionarySourceSupplier;

namespace GoogleDictionarySupplier
{
    public class GoogleDictionaryFactory : AbstractDictionaryFactory
    {
        private readonly ICredentialsStorage credentials=new GoogleCredentials();

        public override ICredentialsStorage CredentialsStorage {
            get { return credentials; }
        }
		public override IEnumerable<string> ListOfSources {
			get {
				if (credentials.IsLoggedIn)
					return base.ListOfSources;
				return Enumerable.Empty<string> ();
			}
		}
        protected override IDictionarySourceSupplier CreateMemorySourceSupplier()
        {
            if (!credentials.IsLoggedIn)
                throw new InvalidOperationException("user need to be logged in");
            var user = credentials.GetCurrentUser();
            return new GoogleDictionarySourceSupplier(user.Login, user.Password);
        }
    }
}
