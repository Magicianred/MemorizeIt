using System;
using System.Collections.Generic;
using System.Linq;
using MemorizeIt.Model;

namespace MemorizeIt.DictionaryStorage
{
    public class InMemoryDictionaryStorage : IDictionaryStorage
    {
        private readonly Dictionary<Guid, DictionaryItem> itemHolder;
		private string tableName;
		private bool empty=true;

		public InMemoryDictionaryStorage(Dictionary<Guid, DictionaryItem> itemHolder)
        {
            this.itemHolder = itemHolder;
        }

        public InMemoryDictionaryStorage():this(new Dictionary<Guid, DictionaryItem>())
        {
        }

        public void Store(DictionaryTable data)
        {
			itemHolder.Clear();
            foreach (var memoryItem in data.Items)
            {
                itemHolder.Add(memoryItem.Id, memoryItem);
            }
			tableName = data.Name;
			empty = false;
			OnSotrageChanged ();
        }

        public void Clear()
        {
            itemHolder.Clear();
			tableName = string.Empty;
			empty = true;
			OnSotrageChanged ();
        }


		public bool Empty ()
		{
			return empty;
		}


        public DictionaryItem GetItemById(Guid id)
        {
            return itemHolder[id];
        }

		public string GetTableName ()
		{
			return tableName;
		}      

        public IEnumerable<DictionaryItem> Items {
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
