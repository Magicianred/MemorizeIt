using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.DictionarySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;

namespace MemorizeIt.DictionarySourceSupplier
{
	public abstract class AbstractDictionaryFactory:IDictionaryFactory
    {
        private IDictionarySourceSupplier memourySource;
        protected abstract IDictionarySourceSupplier CreateMemorySourceSupplier();
        protected IDictionarySourceSupplier MemorySources
        {
            get { return memourySource ?? (memourySource = CreateMemorySourceSupplier()); }
        }

        public DictionaryTable DownloadMemories(string source)
        {
            return MemorySources.Download(source);
        }

        public virtual IEnumerable<string> ListOfSources {
			get { 
				try {
					return MemorySources.GetSourcesList ();
				} catch {
					return new string[0];
				}
			 
			}
		
		}
		public virtual ICredentialsStorage CredentialsStorage { get{return null;} }

		public void CreateTemplate (){
			memourySource.CreateTemplate ();
		}
	}
}
