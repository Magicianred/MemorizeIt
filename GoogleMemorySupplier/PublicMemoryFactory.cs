using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemorySourceSupplier;

namespace GoogleMemorySupplier
{
    public class PublicMemoryFactory : AbstractMemoryFactory
    {
        private readonly string spreadsheetKey;
        private readonly string userName;
        private readonly string password;

        public PublicMemoryFactory(string spreadsheetKey, string userName, string password)
        {
            this.spreadsheetKey = spreadsheetKey;
            this.userName = userName;
            this.password = password;
        }

        protected override IMemorySourceSupplier CreateMemorySourceSupplier()
        {
            return new PublicGoogleMemorySourceSupplier(spreadsheetKey, userName, password);
        }
    }
}
