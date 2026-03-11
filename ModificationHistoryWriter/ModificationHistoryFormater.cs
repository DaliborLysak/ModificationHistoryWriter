using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <inheritdoc cref="IModificationHistoryFormater"/>
    internal class ModificationHistoryFormater : IModificationHistoryFormater
    {
        /// <inheritdoc/>
        public string Format(ModificationHistoryPattern pattern, string input)
        {
            if (pattern is null || String.IsNullOrEmpty(input))
                return String.Empty;

            var output = pattern.Pattern;

            foreach (var match in Regex.Matches(input, pattern.TicketPattern, RegexOptions.IgnoreCase).Cast<Match>())
            {
                output = output.Replace("DATE", DateTime.Today.ToString(pattern.DateFormat));
                output = output.Replace("AUTHOR", pattern.Author);
                output = output.Replace("TICKET", match.Groups[1]?.Value ?? String.Empty);
                output = output.Replace("MESSAGE", match.Groups[3]?.Value ?? String.Empty);
            }

            output = RemoveDiacritics(output);

            return output;
        }

        /// <summary>
        /// Normalizes <paramref name="text"/> to Unicode form D, removes all
        /// non-spacing mark characters (accents), and re-normalizes to form C.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>The input string with all diacritics removed.</returns>
        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
