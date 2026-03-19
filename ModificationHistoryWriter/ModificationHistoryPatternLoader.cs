using System.IO.Abstractions;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ModificationHistoryWriter
{
    /// <inheritdoc cref="IModificationHistoryPatternLoader"/>
    internal class ModificationHistoryPatternLoader : IModificationHistoryPatternLoader
    {
        private const string PATTERN_FILE_NAME = "pattern.json";
        private const string MODIFICATION_HISTORY_WRITER = "ModificationHistoryWriter";

        private readonly IFileSystem _fileSystem;
        private readonly string _appDataPath;

        public ModificationHistoryPatternLoader()
            : this(new FileSystem(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) { }

        internal ModificationHistoryPatternLoader(IFileSystem fileSystem, string appDataPath)
        {
            _fileSystem = fileSystem;
            _appDataPath = appDataPath;
        }

        /// <inheritdoc/>
        public ModificationHistoryPattern Load()
        {
            ModificationHistoryPattern pattern = new ModificationHistoryPattern();

            var patternFile = _fileSystem.Path.Combine(_appDataPath, MODIFICATION_HISTORY_WRITER, PATTERN_FILE_NAME);

            if (_fileSystem.File.Exists(patternFile))
            {
                try
                {
                    string jsonString = _fileSystem.File.ReadAllText(patternFile);
                    pattern = JsonSerializer.Deserialize<ModificationHistoryPattern>(jsonString) ?? pattern;
                }
                catch (JsonException)
                {
                    // corrupted or invalid JSON — fall back to default empty pattern
                }
            }

            return IsValid(pattern) ? pattern : new ModificationHistoryPattern();
        }

        private static bool IsValid(ModificationHistoryPattern pattern)
        {
            if (string.IsNullOrEmpty(pattern.TicketPattern) || string.IsNullOrEmpty(pattern.DateFormat))
                return false;

            try
            {
                _ = new Regex(pattern.TicketPattern);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task Save(ModificationHistoryPattern pattern)
        {
            var patternFolder = _fileSystem.Path.Combine(_appDataPath, MODIFICATION_HISTORY_WRITER);

            if (!_fileSystem.Directory.Exists(patternFolder))
                _fileSystem.Directory.CreateDirectory(patternFolder);

            using var stream = _fileSystem.File.Create(_fileSystem.Path.Combine(patternFolder, PATTERN_FILE_NAME));
            await JsonSerializer.SerializeAsync(stream, pattern);
            await stream.DisposeAsync();
        }
    }
}
