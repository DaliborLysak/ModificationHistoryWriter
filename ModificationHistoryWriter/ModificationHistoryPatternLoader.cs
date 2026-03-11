using System.IO.Abstractions;
using System.Text.Json;

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
                string jsonString = _fileSystem.File.ReadAllText(patternFile);
                pattern = JsonSerializer.Deserialize<ModificationHistoryPattern>(jsonString)!;
            }

            return pattern;
        }

        /// <inheritdoc/>
        public async void Save(ModificationHistoryPattern pattern)
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
