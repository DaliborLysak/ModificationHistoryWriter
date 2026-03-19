using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ModificationHistoryWriter.Test
{
    public class ModificationHistoryFileWriterTest
    {
        private const string TestFilePath = @"C:\test\file.cs";

        private static MockFileSystem CreateFileSystem(string path, string[] lines, Encoding? encoding = null)
        {
            encoding ??= new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            var content = string.Join(Environment.NewLine, lines) + Environment.NewLine;
            return new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [path] = new MockFileData(encoding.GetPreamble().Concat(encoding.GetBytes(content)).ToArray())
            });
        }

        // --- GetEncoding tests ---

        [Fact]
        public void GetEncoding_Utf8WithBom_ReturnsUtf8WithBom()
        {
            var bom = Encoding.UTF8.GetPreamble();
            var content = bom.Concat(Encoding.UTF8.GetBytes("hello")).ToArray();
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(content)
            });

            var encoding = new ModificationHistoryFileWriter(fs).GetEncoding(TestFilePath);

            Assert.Equal(Encoding.UTF8.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf8WithoutBom_ReturnsEncodingWithNoPreamble()
        {
            var content = Encoding.UTF8.GetBytes("hello");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(content)
            });

            var encoding = new ModificationHistoryFileWriter(fs).GetEncoding(TestFilePath);

            Assert.Empty(encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf16LittleEndianBom_ReturnsUtf16Le()
        {
            var content = Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes("hello")).ToArray();
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(content)
            });

            var encoding = new ModificationHistoryFileWriter(fs).GetEncoding(TestFilePath);

            Assert.Equal(Encoding.Unicode.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf16BigEndianBom_ReturnsUtf16Be()
        {
            var content = Encoding.BigEndianUnicode.GetPreamble().Concat(Encoding.BigEndianUnicode.GetBytes("hello")).ToArray();
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(content)
            });

            var encoding = new ModificationHistoryFileWriter(fs).GetEncoding(TestFilePath);

            Assert.Equal(Encoding.BigEndianUnicode.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_EmptyFile_ReturnsEncodingWithNoPreamble()
        {
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(Array.Empty<byte>())
            });

            var encoding = new ModificationHistoryFileWriter(fs).GetEncoding(TestFilePath);

            Assert.Empty(encoding.GetPreamble());
        }

        // --- IsHistoryLine tests (via Write behaviour) ---

        [Theory]
        [InlineData("// 19.03.2026  dlysak   REQ012345 blablabla", true)]
        [InlineData("// 01.01.2024  jsmith   DEF001 fix null ref", true)]
        [InlineData("// 31.12.2023  a.novak  IMP999 refactor service", true)]
        [InlineData("// MODIFICATION HISTORY", false)]
        [InlineData("// -----------------------------------------------------------------------------", false)]
        [InlineData("// TODO: fix this bug", false)]
        [InlineData("// Copyright 2026 Acme Corp", false)]
        [InlineData("// 2026/03/19  dlysak  REQ1  wrong separator", false)]
        [InlineData("// 19-03-2026  dlysak  REQ1  wrong separator", false)]
        [InlineData("public class Foo { }", false)]
        [InlineData("", false)]
        public void Write_IsHistoryLine_MatchesOnlyDotSeparatedDateEntries(string line, bool expectedIsHistory)
        {
            // Build a file where the candidate line is followed by an empty line.
            // If IsHistoryLine returns true, Write() will insert after it; otherwise the file stays unchanged.
            var originalLines = new[] { line, "" };
            var fs = CreateFileSystem(TestFilePath, originalLines);
            var originalContent = fs.File.ReadAllText(TestFilePath);

            new ModificationHistoryFileWriter(fs).Write(TestFilePath, "// 19.03.2026  dlysak   REQ099 new entry");

            var contentAfter = fs.File.ReadAllText(TestFilePath);
            if (expectedIsHistory)
                Assert.NotEqual(originalContent, contentAfter);
            else
                Assert.Equal(originalContent, contentAfter);
        }

        // --- Write tests ---

        [Fact]
        public void Write_NullPath_DoesNotThrow()
        {
            var writer = new ModificationHistoryFileWriter(new MockFileSystem());
            var ex = Record.Exception(() => writer.Write(null!, "// 11.03.2026  Author  REQ1  fix"));
            Assert.Null(ex);
        }

        [Fact]
        public void Write_EmptyPath_DoesNotThrow()
        {
            var writer = new ModificationHistoryFileWriter(new MockFileSystem());
            var ex = Record.Exception(() => writer.Write(string.Empty, "// 11.03.2026  Author  REQ1  fix"));
            Assert.Null(ex);
        }

        [Fact]
        public void Write_NonExistentFile_DoesNotThrow()
        {
            var writer = new ModificationHistoryFileWriter(new MockFileSystem());
            var ex = Record.Exception(() => writer.Write(@"C:\nonexistent\file.cs", "// 11.03.2026  Author  REQ1  fix"));
            Assert.Null(ex);
        }

        [Fact]
        public void Write_EmptyLog_DoesNotThrow()
        {
            var fs = CreateFileSystem(TestFilePath, new[] { "// 10.03.2026  Author  REQ1  initial", "" });
            var writer = new ModificationHistoryFileWriter(fs);
            var ex = Record.Exception(() => writer.Write(TestFilePath, string.Empty));
            Assert.Null(ex);
        }

        [Fact]
        public void Write_InsertsLogAfterLastHistoryLine()
        {
            var fs = CreateFileSystem(TestFilePath, new[]
            {
                "// MODIFICATION HISTORY",
                "// -----------------------------------------------------------------------------",
                "// 10.03.2026  Author  REQ1000          Initial implementation",
                ""
            });

            var newEntry = "// 11.03.2026  Author  REQ1234          Fix login timeout";
            new ModificationHistoryFileWriter(fs).Write(TestFilePath, newEntry);

            var result = fs.File.ReadAllLines(TestFilePath);
            Assert.Contains(newEntry, result);
            var lastHistoryIndex = Array.FindLastIndex(result, l => l.StartsWith("// 10.03.2026"));
            var newEntryIndex = Array.FindIndex(result, l => l == newEntry);
            Assert.True(newEntryIndex > lastHistoryIndex);
        }

        [Fact]
        public void Write_NoHistoryBlock_FileUnchanged()
        {
            var lines = new[] { "public class Foo", "{", "}" };
            var fs = CreateFileSystem(TestFilePath, lines);
            var originalContent = fs.File.ReadAllText(TestFilePath);

            new ModificationHistoryFileWriter(fs).Write(TestFilePath, "// 11.03.2026  Author  REQ1234  fix");

            Assert.Equal(originalContent, fs.File.ReadAllText(TestFilePath));
        }

        [Fact]
        public void Write_PreservesExistingFileContent()
        {
            var fs = CreateFileSystem(TestFilePath, new[]
            {
                "// MODIFICATION HISTORY",
                "// -----------------------------------------------------------------------------",
                "// 10.03.2026  Author  REQ1000          Initial",
                "",
                "public class Foo { }"
            });

            new ModificationHistoryFileWriter(fs).Write(TestFilePath, "// 11.03.2026  Author  REQ1234  Fix");

            var result = fs.File.ReadAllLines(TestFilePath);
            Assert.Contains("public class Foo { }", result);
        }

        [Fact]
        public void Write_WhenReadFails_ThrowsIOExceptionWithFilePath()
        {
            var fileInfo = Substitute.For<IFileInfoFactory>();
            var fs = Substitute.For<IFileSystem>();
            fs.File.Exists(TestFilePath).Returns(true);
            fs.File.OpenRead(TestFilePath).Returns(_ => throw new UnauthorizedAccessException("Access denied"));

            var writer = new ModificationHistoryFileWriter(fs);
            var ex = Assert.Throws<IOException>(() => writer.Write(TestFilePath, "// 11.03.2026  Author  REQ1  fix"));

            Assert.Contains(TestFilePath, ex.Message);
            Assert.IsType<UnauthorizedAccessException>(ex.InnerException);
        }

        [Fact]
        public void Write_WhenWriteFails_ThrowsIOExceptionWithFilePath()
        {
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                [TestFilePath] = new MockFileData(Encoding.UTF8.GetBytes(
                    "// MODIFICATION HISTORY\r\n// 10.03.2026  Author  REQ1  init\r\n\r\n"))
            });

            var wrappedFs = Substitute.For<IFileSystem>();
            var fileSub = wrappedFs.File;
            fileSub.Exists(TestFilePath).Returns(true);
            fileSub.OpenRead(TestFilePath).Returns(_ => fs.File.OpenRead(TestFilePath));
            fileSub.ReadAllLines(TestFilePath, Arg.Any<Encoding>())
                .Returns(_ => fs.File.ReadAllLines(TestFilePath));
            fileSub.When(f => f.WriteAllLines(Arg.Any<string>(), Arg.Any<string[]>(), Arg.Any<Encoding>()))
                .Do(_ => throw new IOException("Disk full"));

            var writer = new ModificationHistoryFileWriter(wrappedFs);
            var ex = Assert.Throws<IOException>(() => writer.Write(TestFilePath, "// 11.03.2026  Author  REQ1  fix"));

            Assert.Contains(TestFilePath, ex.Message);
        }

        [Fact]
        public void Write_PreservesEncoding_Utf8WithoutBom()
        {
            var fs = CreateFileSystem(TestFilePath, new[]
            {
                "// MODIFICATION HISTORY",
                "// 10.03.2026  Author  REQ1000          Initial",
                ""
            }, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            new ModificationHistoryFileWriter(fs).Write(TestFilePath, "// 11.03.2026  Author  REQ1234  Fix");

            var bytes = fs.File.ReadAllBytes(TestFilePath);
            Assert.False(bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF);
        }
    }
}
