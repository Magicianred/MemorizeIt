using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryStorage
{
    public interface IMemoryStorage
    {
        void Store(MemoryTable data);
        void Clear();
		bool Empty();
        
        MemoryItem GetItemById(Guid id);
        IEnumerable<MemoryItem> Items { get; }
		string GetTableName ();

        void ItemSuccess(Guid id);
        void ItemFail(Guid id);

		event EventHandler SotrageChanged;
    }
}
