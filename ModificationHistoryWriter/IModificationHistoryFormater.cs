using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <summary>
    /// Formats a raw input string (typically a commit message) into a single
    /// modification history line according to a <see cref="ModificationHistoryPattern"/>.
    /// </summary>
    public interface IModificationHistoryFormater
    {
        /// <summary>
        /// Applies <paramref name="pattern"/> to <paramref name="input"/> and returns
        /// the formatted history line.
        /// </summary>
        /// <param name="pattern">
        /// The pattern configuration that defines the output template, date format,
        /// author name, and ticket regex.
        /// </param>
        /// <param name="input">
        /// The raw input string to parse, e.g. a git commit message such as
        /// <c>REQ1234 Fix login timeout</c>.
        /// </param>
        /// <returns>
        /// The formatted history line with all tokens replaced, or
        /// <see cref="string.Empty"/> when <paramref name="pattern"/> is <c>null</c>
        /// or <paramref name="input"/> is null or empty.
        /// </returns>
        string Format(ModificationHistoryPattern pattern, string input);
    }
}
