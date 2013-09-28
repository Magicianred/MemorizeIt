﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemorySourceSupplier
{
	public abstract class AbstractMemoryFactory:IMemoryFactory
    {
        private IMemorySourceSupplier memourySource;
        protected abstract IMemorySourceSupplier CreateMemorySourceSupplier();
        protected IMemorySourceSupplier MemorySources
        {
            get { return memourySource ?? (memourySource = CreateMemorySourceSupplier()); }
        }

        public MemoryTable DownloadMemories(string source)
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
