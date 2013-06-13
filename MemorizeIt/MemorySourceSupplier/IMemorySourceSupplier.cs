using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.MemorySourceSupplier
{
    public interface IMemorySourceSupplier
    {
        MemoryTable Download();
    }
}
