using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <summary>
    /// Writes a formatted modification history line into the header of a source file.
    /// </summary>
    public interface IModificationHistoryFileWriter
    {
        /// <summary>
        /// Inserts <paramref name="log"/> into the modification history block of the
        /// file at <paramref name="path"/>.
        /// <para>
        /// The method searches for the last existing history comment line (matching
        /// <c>// ... [...] ...</c>) that is immediately followed by an empty line and
        /// appends the new entry after it. If no such line is found, the file is left
        /// unchanged.
        /// </para>
        /// </summary>
        /// <param name="path">The absolute path to the source file to update.</param>
        /// <param name="log">The formatted history line to insert.</param>
        void Write(string path, string log);
    }
}
