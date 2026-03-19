using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace ModificationHistoryWriter.Test
{
    public class ModificationHistoryPatternLoaderTest
    {
        private const string AppDataPath = @"C:\AppData";
        private const string PatternFile = @"C:\AppData\ModificationHistoryWriter\pattern.json";

        [Fact]
        public void Load_WhenFileNotExists_ReturnsDefaultEmptyPattern()
        {
            var fs = new MockFileSystem();
            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);

            var pattern = loader.Load();

            Assert.NotNull(pattern);
            Assert.Equal(string.Empty, pattern.Pattern);
            Assert.Equal(string.Empty, pattern.Author);
            Assert.Equal(string.Empty, pattern.DateFormat);
            Assert.Equal(string.Empty, pattern.TicketPattern);
        }

        [Fact]
        public void Load_WhenFileExists_ReturnsDeserializedPattern()
        {
            var expected = new ModificationHistoryPattern
            {
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                DateFormat = "dd.MM.yyyy",
                Author = "Test Author",
                TicketPattern = "((REQ|DEF)\\d*)\\s*(.*)"
            };

            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData(JsonSerializer.Serialize(expected))
            });

            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);
            var pattern = loader.Load();

            Assert.Equal(expected.Pattern, pattern.Pattern);
            Assert.Equal(expected.DateFormat, pattern.DateFormat);
            Assert.Equal(expected.Author, pattern.Author);
            Assert.Equal(expected.TicketPattern, pattern.TicketPattern);
        }

        [Fact]
        public async Task Save_ThenLoad_RoundTrip()
        {
            var expected = new ModificationHistoryPattern
            {
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                DateFormat = "MM/dd/yyyy",
                Author = "Round Trip Author",
                TicketPattern = "((IMP)\\d*)\\s*(.*)"
            };

            var fs = new MockFileSystem();
            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);
            await loader.Save(expected);

            var pattern = loader.Load();

            Assert.Equal(expected.Pattern, pattern.Pattern);
            Assert.Equal(expected.DateFormat, pattern.DateFormat);
            Assert.Equal(expected.Author, pattern.Author);
            Assert.Equal(expected.TicketPattern, pattern.TicketPattern);
        }

        [Fact]
        public async Task Save_CreatesDirectoryIfNotExists()
        {
            var fs = new MockFileSystem();
            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);

            await loader.Save(new ModificationHistoryPattern { Author = "Test" });

            Assert.True(fs.Directory.Exists(@"C:\AppData\ModificationHistoryWriter"));
            Assert.True(fs.File.Exists(PatternFile));
        }

        [Fact]
        public void Load_WhenFileContainsInvalidJson_ReturnsDefaultEmptyPattern()
        {
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData("this is not valid json {{{")
            });

            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);

            var pattern = loader.Load();

            Assert.NotNull(pattern);
            Assert.Equal(string.Empty, pattern.Pattern);
            Assert.Equal(string.Empty, pattern.Author);
            Assert.Equal(string.Empty, pattern.DateFormat);
            Assert.Equal(string.Empty, pattern.TicketPattern);
        }

        [Fact]
        public void Load_WhenFileContainsJsonNull_ReturnsDefaultEmptyPattern()
        {
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData("null")
            });

            var loader = new ModificationHistoryPatternLoader(fs, AppDataPath);

            var pattern = loader.Load();

            Assert.NotNull(pattern);
            Assert.Equal(string.Empty, pattern.Pattern);
            Assert.Equal(string.Empty, pattern.Author);
            Assert.Equal(string.Empty, pattern.DateFormat);
            Assert.Equal(string.Empty, pattern.TicketPattern);
        }

        [Fact]
        public void Load_WhenTicketPatternIsInvalidRegex_ReturnsDefaultEmptyPattern()
        {
            var invalid = new ModificationHistoryPattern
            {
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                DateFormat = "dd.MM.yyyy",
                Author = "dlysak",
                TicketPattern = "((unclosed"
            };

            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData(JsonSerializer.Serialize(invalid))
            });

            var pattern = new ModificationHistoryPatternLoader(fs, AppDataPath).Load();

            Assert.Equal(string.Empty, pattern.TicketPattern);
        }

        [Fact]
        public void Load_WhenDateFormatIsEmpty_ReturnsDefaultEmptyPattern()
        {
            var invalid = new ModificationHistoryPattern
            {
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                DateFormat = "",
                Author = "dlysak",
                TicketPattern = "((REQ|DEF)\\d*)\\s*(.*)"
            };

            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData(JsonSerializer.Serialize(invalid))
            });

            var pattern = new ModificationHistoryPatternLoader(fs, AppDataPath).Load();

            Assert.Equal(string.Empty, pattern.DateFormat);
        }

        [Fact]
        public void Load_WhenTicketPatternIsEmpty_ReturnsDefaultEmptyPattern()
        {
            var invalid = new ModificationHistoryPattern
            {
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                DateFormat = "dd.MM.yyyy",
                Author = "dlysak",
                TicketPattern = ""
            };

            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [PatternFile] = new MockFileData(JsonSerializer.Serialize(invalid))
            });

            var pattern = new ModificationHistoryPatternLoader(fs, AppDataPath).Load();

            Assert.Equal(string.Empty, pattern.TicketPattern);
        }
    }
}
