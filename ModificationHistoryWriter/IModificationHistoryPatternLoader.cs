using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <summary>
    /// Loads and saves a <see cref="ModificationHistoryPattern"/> from persistent storage.
    /// The default implementation persists the pattern as <c>pattern.json</c> under
    /// <c>%AppData%\ModificationHistoryWriter\</c>.
    /// </summary>
    public interface IModificationHistoryPatternLoader
    {
        /// <summary>
        /// Loads the <see cref="ModificationHistoryPattern"/> from persistent storage.
        /// </summary>
        /// <returns>
        /// The deserialized <see cref="ModificationHistoryPattern"/>, or a default
        /// (empty) instance when the configuration file does not exist.
        /// </returns>
        public ModificationHistoryPattern Load();

        /// <summary>
        /// Saves <paramref name="pattern"/> to persistent storage, creating the
        /// target directory if it does not already exist.
        /// </summary>
        /// <param name="pattern">The pattern configuration to persist.</param>
        void Save(ModificationHistoryPattern pattern);
    }
}
