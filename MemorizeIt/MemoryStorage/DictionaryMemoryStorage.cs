using System;
using System.Collections.Generic;
using System.Linq;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryStorage
{
    public class DictionaryMemoryStorage : IMemoryStorage
    {
        private readonly Dictionary<Guid, MemoryItem> itemHolder;
        private readonly Random random;
        public DictionaryMemoryStorage(Dictionary<Guid, MemoryItem> itemHolder)
        {
            this.itemHolder = itemHolder;
            this.random = new Random();
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
			OnSotrageChanged ();
        }

        public void Clear()
        {
            itemHolder.Clear();
			OnSotrageChanged ();
        }

        public MemoryItem Get(Guid id)
        {
            return itemHolder[id];
        }

      

        public IList<MemoryItem> Items {
            get { return itemHolder.Select(i => i.Value).ToList(); }
        }

        public void Success(Guid id)
        {
            itemHolder[id].Upvote();
			OnSotrageChanged ();
        }

        public void Fail(Guid id)
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
