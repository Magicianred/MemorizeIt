using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.DictionarySourceSupplier
{
    public interface IDictionarySourceSupplier
    {
        DictionaryTable Download(string sheetName);
        IEnumerable<string> GetSourcesList();
        void CreateTemplate();
    }
}
