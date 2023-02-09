using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    public interface IModificationHistoryPatternLoader
    {
        public ModificationHistoryPattern Load();
        void Save(ModificationHistoryPattern pattern);
    }
}
