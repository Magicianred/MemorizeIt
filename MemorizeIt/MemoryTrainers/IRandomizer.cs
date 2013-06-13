using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryTrainers
{
    public interface IRandomizer
    {
        QuestionAndAnswer GetRandomUnsuccessItem();
    }
}
