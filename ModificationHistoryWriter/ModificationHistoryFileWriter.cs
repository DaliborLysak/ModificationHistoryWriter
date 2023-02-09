using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    internal class ModificationHistoryFileWriter : IModificationHistoryFileWriter
    {
        public void Write(string path, string log)
        {
            if (!String.IsNullOrEmpty(path) && File.Exists(path) && !String.IsNullOrEmpty(log))
            {
                var lines = File.ReadAllLines(path, Encoding.UTF8);
                var lineNumber = 0;

                while (lineNumber < lines.Length)
                {
                    var line = lines[lineNumber];

                    if (!String.IsNullOrEmpty(line) || line.Equals(Environment.NewLine))
                    {
                        if ((lineNumber + 1 < lines.Length) && IsLastHistoryLine(line, lines[lineNumber + 1]))
                        {
                            lines[lineNumber] = $"{line}{Environment.NewLine}{log}";
                            lineNumber = lines.Length;
                        }
                    }

                    lineNumber++;
                }

                File.WriteAllLines(path, lines, Encoding.UTF8);
            }
        }

        private static bool IsLastHistoryLine(string line, string nextLine)
        {
            return IsHistoryLine(line) && String.IsNullOrEmpty(nextLine);
        }

        private static bool IsHistoryLine(string line)
        {
            var isHistoryLine = false;
            foreach (Match match in Regex.Matches(line, @"//.*\[{0,1}.*\]{0,1}\s*.*", RegexOptions.IgnoreCase))
            {
                isHistoryLine = match.Success; // should by only one
            }

            return isHistoryLine;
        }
    }
}
