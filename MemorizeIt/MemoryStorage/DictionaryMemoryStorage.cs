using System;
using System.Collections.Generic;
using System.Linq;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryStorage
{
    public class DictionaryMemoryStorage : IMemoryStorage
    {
        private readonly Dictionary<Guid, MemoryItem> itemHolder;
		private string tableName;

		public DictionaryMemoryStorage(Dictionary<Guid, MemoryItem> itemHolder)
        {
            this.itemHolder = itemHolder;
        }

        public DictionaryMemoryStorage():this(new Dictionary<Guid, MemoryItem>())
        {
        }

        public void Store(MemoryTable data)
        {
			itemHolder.Clear();
            foreach (var memoryItem in data.Items)
            {
                itemHolder.Add(memoryItem.Id, memoryItem);
            }
			tableName = data.Name;
			OnSotrageChanged ();
        }

        public void Clear()
        {
            itemHolder.Clear();
			tableName = string.Empty;
			OnSotrageChanged ();
        }

        public MemoryItem GetItemById(Guid id)
        {
            return itemHolder[id];
        }

		public string GetTableName ()
		{
			return tableName;
		}      

        public IEnumerable<MemoryItem> Items {
			get { return itemHolder.Select (i => i.Value); }
        }

        public void ItemSuccess(Guid id)
        {
            itemHolder[id].Upvote();
			OnSotrageChanged ();
        }

        public void ItemFail(Guid id)
        {
            itemHolder[id].Downvote();
			OnSotrageChanged ();
        }
		public event EventHandler SotrageChanged;

		protected void OnSotrageChanged(){
			var handler = SotrageChanged;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}
    }
}
