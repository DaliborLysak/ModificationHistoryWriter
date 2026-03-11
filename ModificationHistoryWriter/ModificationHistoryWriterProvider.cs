using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <summary>
    /// Provides lazily-initialized singleton instances of the core services.
    /// Use this class to obtain the shared <see cref="IModificationHistoryFormater"/>,
    /// <see cref="IModificationHistoryPatternLoader"/>, and
    /// <see cref="IModificationHistoryFileWriter"/> without creating new instances
    /// on each call.
    /// </summary>
    public static class ModificationHistoryWriterProvider
    {
        private static readonly Lazy<IModificationHistoryFormater> modificationHistoryFormater
            = new Lazy<IModificationHistoryFormater>(() => new ModificationHistoryFormater());

        private static readonly Lazy<IModificationHistoryPatternLoader> modificationHistoryPatternLoader
            = new Lazy<IModificationHistoryPatternLoader>(() => new ModificationHistoryPatternLoader());

        private static readonly Lazy<IModificationHistoryFileWriter> modificationHistoryFileWriter
            = new Lazy<IModificationHistoryFileWriter>(() => new ModificationHistoryFileWriter());

        /// <summary>Gets the shared <see cref="IModificationHistoryFormater"/> instance.</summary>
        public static IModificationHistoryFormater ModificationHistoryFormater => modificationHistoryFormater.Value;

        /// <summary>Gets the shared <see cref="IModificationHistoryPatternLoader"/> instance.</summary>
        public static IModificationHistoryPatternLoader ModificationHistoryPatternLoader => modificationHistoryPatternLoader.Value;

        /// <summary>Gets the shared <see cref="IModificationHistoryFileWriter"/> instance.</summary>
        public static IModificationHistoryFileWriter ModificationHistoryFileWriter => modificationHistoryFileWriter.Value;
    }
}
