using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    public static class ModificationHistoryWriterProvider
    {
        private static readonly Lazy<IModificationHistoryFormater> modificationHistoryFormater 
            = new Lazy<IModificationHistoryFormater>(() => new ModificationHistoryFormater());

        private static readonly Lazy<IModificationHistoryPatternLoader> modificationHistoryPatternLoader
            = new Lazy<IModificationHistoryPatternLoader>(() => new ModificationHistoryPatternLoader());

        private static readonly Lazy<IModificationHistoryFileWriter> modificationHistoryFileWriter
            = new Lazy<IModificationHistoryFileWriter>(() => new ModificationHistoryFileWriter());

        public static IModificationHistoryFormater ModificationHistoryFormater => modificationHistoryFormater.Value;
        public static IModificationHistoryPatternLoader ModificationHistoryPatternLoader => modificationHistoryPatternLoader.Value;
        public static IModificationHistoryFileWriter ModificationHistoryFileWriter => modificationHistoryFileWriter.Value;
    }
}
