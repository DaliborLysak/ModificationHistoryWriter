using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModificationHistoryWriter
{
    /// <inheritdoc cref="IModificationHistoryFileWriter"/>
    internal class ModificationHistoryFileWriter : IModificationHistoryFileWriter
    {
        private readonly IFileSystem _fileSystem;

        public ModificationHistoryFileWriter() : this(new FileSystem()) { }

        internal ModificationHistoryFileWriter(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        public void Write(string path, string log)
        {
            if (!String.IsNullOrEmpty(path) && _fileSystem.File.Exists(path) && !String.IsNullOrEmpty(log))
            {
                try
                {
                    var encoding = GetEncoding(path);
                    var lines = _fileSystem.File.ReadAllLines(path, encoding);
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

                    _fileSystem.File.WriteAllLines(path, lines, encoding);
                }
                catch (Exception ex)
                {
                    throw new IOException($"Failed to write modification history to '{path}': {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Detects the encoding of the file at <paramref name="path"/> by inspecting
        /// its byte order mark (BOM). Falls back to UTF-8 without BOM when no BOM is present.
        /// </summary>
        /// <param name="path">The absolute path to the file to inspect.</param>
        /// <returns>The detected <see cref="Encoding"/> of the file.</returns>
        internal Encoding GetEncoding(string path)
        {
            using var stream = _fileSystem.File.OpenRead(path);
            using var reader = new StreamReader(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), detectEncodingFromByteOrderMarks: true);
            reader.Peek();
            return reader.CurrentEncoding;
        }

        /// <summary>
        /// Returns <c>true</c> when <paramref name="line"/> is a history comment and
        /// <paramref name="nextLine"/> is empty, indicating the insertion point.
        /// </summary>
        private static bool IsLastHistoryLine(string line, string nextLine)
        {
            return IsHistoryLine(line) && String.IsNullOrEmpty(nextLine);
        }

        /// <summary>
        /// Returns <c>true</c> when <paramref name="line"/> matches the modification
        /// history entry pattern: a comment starting with a date in <c>DD.MM.YYYY</c> format,
        /// e.g. <c>// 19.03.2026  dlysak   REQ012345 blablabla</c>.
        /// </summary>
        private static bool IsHistoryLine(string line)
        {
            return Regex.IsMatch(line, @"^\s*//\s+\d{2}\.\d{2}\.\d{4}");
        }
    }
}
