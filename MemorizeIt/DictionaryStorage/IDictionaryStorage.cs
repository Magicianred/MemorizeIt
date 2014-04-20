using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.DictionaryStorage
{
    public interface IDictionaryStorage
    {
        void Store(DictionaryTable data);
        void Clear();
		bool Empty();
        
        DictionaryItem GetItemById(Guid id);
        IEnumerable<DictionaryItem> Items { get; }
		string GetTableName ();

        void ItemSuccess(Guid id);
        void ItemFail(Guid id);

		event EventHandler SotrageChanged;
    }
}
