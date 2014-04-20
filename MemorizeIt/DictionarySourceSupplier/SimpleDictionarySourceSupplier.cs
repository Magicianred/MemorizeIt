using System;
using MemorizeIt.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MemorizeIt.DictionarySourceSupplier
{
    public class SimpleDictionarySourceSupplier : IDictionarySourceSupplier
    {
        private readonly DictionaryTable source;
        private readonly IEnumerable<string> sourceList;
        public SimpleDictionarySourceSupplier(string name, IEnumerable<DictionaryItem> items, IEnumerable<string> sourceList)
        {
            this.source = new DictionaryTable(name, items.ToArray());
            this.sourceList = sourceList;
        }

        public DictionaryTable Download(string sheetName)
        {
            return source;
        }

        public IEnumerable<string> GetSourcesList()
        {
            return sourceList;
        }

        public void CreateTemplate()
        {
            throw new NotImplementedException();
        }
    }
}

