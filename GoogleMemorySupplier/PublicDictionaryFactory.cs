using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.DictionarySourceSupplier;

namespace GoogleDictionarySupplier
{
    public class PublicDictionaryFactory : AbstractDictionaryFactory
    {
        private readonly string spreadsheetKey;
        private readonly string userName;
        private readonly string password;

        public PublicDictionaryFactory(string spreadsheetKey, string userName, string password)
        {
            this.spreadsheetKey = spreadsheetKey;
            this.userName = userName;
            this.password = password;
        }

        protected override IDictionarySourceSupplier CreateMemorySourceSupplier()
        {
			return new PublicGoogleDictionarySourceSupplier(spreadsheetKey, userName, password);
        }
    }
}
