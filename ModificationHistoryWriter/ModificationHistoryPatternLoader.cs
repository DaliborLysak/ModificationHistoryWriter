using System.Reflection;
using System.Text.Json;

namespace ModificationHistoryWriter
{
    /// <inheritdoc cref="IModificationHistoryPatternLoader"/>
    internal class ModificationHistoryPatternLoader : IModificationHistoryPatternLoader
    {
        private const string PATTERN_FILE_NAME = "pattern.json";
        private const string MODIFICATION_HISTORY_WRITER = "ModificationHistoryWriter";

        /// <inheritdoc/>
        public ModificationHistoryPattern Load()
        {
            ModificationHistoryPattern pattern = new ModificationHistoryPattern();

            var patternFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                MODIFICATION_HISTORY_WRITER,
                PATTERN_FILE_NAME);

            if (File.Exists(patternFile))
            {
                string jsonString = File.ReadAllText(patternFile);
                pattern = JsonSerializer.Deserialize<ModificationHistoryPattern>(jsonString)!;
            }

            return pattern;
        }

        /// <inheritdoc/>
        public async void Save(ModificationHistoryPattern pattern)
        {
            var patternFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                MODIFICATION_HISTORY_WRITER);

            if (!Directory.Exists(patternFolder))
                Directory.CreateDirectory(patternFolder);

                using FileStream stream = File.Create(Path.Combine(patternFolder, PATTERN_FILE_NAME));
                await JsonSerializer.SerializeAsync(stream, pattern);
                await stream.DisposeAsync();
        }
    }
}
