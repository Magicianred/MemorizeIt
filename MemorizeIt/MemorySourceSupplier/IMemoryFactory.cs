using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemorySourceSupplier
{
    public interface IMemoryFactory
    {
        ICredentialsStorage CredentialsStorage { get; }
        MemoryTable DownloadMemories(string source );
        IEnumerable<string> ListOfSources { get; }
    }
}
