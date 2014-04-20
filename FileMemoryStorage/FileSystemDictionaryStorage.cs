using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemorizeIt.DictionaryStorage;
using MemorizeIt.Model;
using Newtonsoft.Json;

namespace FileDictionaryStorage
{
    public class FileSystemDictionaryStorage : IDictionaryStorage
    {
		private readonly IDictionaryStorage memcacheStorage;
      

        public FileSystemDictionaryStorage()
        {
			this.memcacheStorage = new InMemoryDictionaryStorage();
            if (File.Exists(FilePath))
            {
                var fileContent = File.ReadAllText(FilePath,Encoding.UTF8);
                var table = GetObjectMemoryTable(fileContent);
                    memcacheStorage.Store(table);
            }
        }

        protected string FilePath {
            get { return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "file_storage"); }
        }
        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        private string GetJsonData(object payload)
        {
            var data = JsonConvert.SerializeObject(payload, Formatting.None,
                                                   new JsonSerializerSettings
                                                   {
                                                       TypeNameHandling = TypeNameHandling.Objects
                                                   });
            
            return data;
        }

        private DictionaryTable GetObjectMemoryTable(string json)
        {
            return JsonConvert.DeserializeObject<DictionaryTable>(json,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }

        public void Store(DictionaryTable data)
        {
            StoreInternal(data);
            memcacheStorage.Store(data);
        }

        private void StoreInternal(DictionaryTable data)
        {
            File.WriteAllText(FilePath, GetJsonData(data), Encoding.UTF8);
        }

        public void Clear()
        {
            File.Delete(FilePath);
            memcacheStorage.Clear();
        }

		public bool Empty ()
		{
			return memcacheStorage.Empty ();
		}

		public DictionaryItem GetItemById(Guid id)
        {
			return memcacheStorage.GetItemById(id);
        }

        public IEnumerable<DictionaryItem> Items {
            get { return memcacheStorage.Items; }
        }


		public string GetTableName ()
		{
			return memcacheStorage.GetTableName ();
		}

        public void ItemSuccess(Guid id)
        {
            memcacheStorage.ItemSuccess(id);
			StoreInternal(new DictionaryTable(memcacheStorage.GetTableName(),memcacheStorage.Items.ToArray()));
        }

        public void ItemFail(Guid id)
        {
            memcacheStorage.ItemFail(id);
			StoreInternal(new DictionaryTable(memcacheStorage.GetTableName(),memcacheStorage.Items.ToArray()));
        }

		public event EventHandler SotrageChanged {add{ memcacheStorage.SotrageChanged+= value;}
			remove{ memcacheStorage.SotrageChanged -= value;}
		}
    }
}
