using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    public interface IModificationHistoryFormater
    {
        string Format(ModificationHistoryPattern pattern, string input);
    }
}
