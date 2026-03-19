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
            if (pattern is null || string.IsNullOrEmpty(input))
                return string.Empty;

            var output = pattern.Pattern;

            var match = Regex.Matches(input, pattern.TicketPattern, RegexOptions.IgnoreCase)
                             .Cast<Match>()
                             .FirstOrDefault();

            if (match != null)
            {
                var tokens = new Dictionary<string, string>
                {
                    ["DATE"]    = DateTime.Today.ToString(pattern.DateFormat),
                    ["AUTHOR"]  = pattern.Author,
                    ["TICKET"]  = match.Groups[1]?.Value ?? string.Empty,
                    ["MESSAGE"] = match.Groups[3]?.Value ?? string.Empty,
                };

                var tokenPattern = string.Join("|", tokens.Keys.Select(k => Regex.Escape(k)));
                output = Regex.Replace(output, tokenPattern, m => tokens[m.Value]);
            }

            return RemoveDiacritics(output);
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
