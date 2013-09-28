using System;
using MemorizeIt.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MemorizeIt.MemorySourceSupplier
{
    public class SimpleMemorySourceSupplier : IMemorySourceSupplier
    {
        private readonly MemoryTable source;
        private readonly IEnumerable<string> sourceList;
        public SimpleMemorySourceSupplier(string name, IEnumerable<MemoryItem> items, IEnumerable<string> sourceList)
        {
            this.source = new MemoryTable(name, items.ToArray());
            this.sourceList = sourceList;
        }

        public MemoryTable Download(string sheetName)
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

