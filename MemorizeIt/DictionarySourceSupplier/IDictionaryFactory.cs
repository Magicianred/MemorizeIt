using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.DictionarySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;

namespace MemorizeIt.DictionarySourceSupplier
{
    public interface IDictionaryFactory
    {
        ICredentialsStorage CredentialsStorage { get; }
        DictionaryTable DownloadMemories(string source );
        IEnumerable<string> ListOfSources { get; }
		void CreateTemplate ();
    }
}
