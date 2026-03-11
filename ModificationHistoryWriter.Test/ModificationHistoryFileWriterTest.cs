using System.Text;

namespace ModificationHistoryWriter.Test
{
    public class ModificationHistoryFileWriterTest : IDisposable
    {
        private readonly List<string> _tempFiles = new();

        /// <summary>
        /// Creates a temporary file with the given raw bytes and registers it for cleanup.
        /// </summary>
        private string CreateTempFile(byte[] content)
        {
            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, content);
            _tempFiles.Add(path);
            return path;
        }

        public void Dispose()
        {
            foreach (var path in _tempFiles)
                if (File.Exists(path))
                    File.Delete(path);
        }

        [Fact]
        public void GetEncoding_Utf8WithBom_ReturnsUtf8WithBom()
        {
            // UTF-8 BOM: EF BB BF
            var bom = Encoding.UTF8.GetPreamble();
            var content = bom.Concat(Encoding.UTF8.GetBytes("hello")).ToArray();
            var path = CreateTempFile(content);

            var encoding = ModificationHistoryFileWriter.GetEncoding(path);

            Assert.Equal(Encoding.UTF8.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf8WithoutBom_ReturnsEncodingWithNoPreamble()
        {
            // No BOM — plain UTF-8
            var content = Encoding.UTF8.GetBytes("hello");
            var path = CreateTempFile(content);

            var encoding = ModificationHistoryFileWriter.GetEncoding(path);

            Assert.Empty(encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf16LittleEndianBom_ReturnsUtf16Le()
        {
            // UTF-16 LE BOM: FF FE
            var bom = Encoding.Unicode.GetPreamble();
            var content = bom.Concat(Encoding.Unicode.GetBytes("hello")).ToArray();
            var path = CreateTempFile(content);

            var encoding = ModificationHistoryFileWriter.GetEncoding(path);

            Assert.Equal(Encoding.Unicode.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_Utf16BigEndianBom_ReturnsUtf16Be()
        {
            // UTF-16 BE BOM: FE FF
            var bom = Encoding.BigEndianUnicode.GetPreamble();
            var content = bom.Concat(Encoding.BigEndianUnicode.GetBytes("hello")).ToArray();
            var path = CreateTempFile(content);

            var encoding = ModificationHistoryFileWriter.GetEncoding(path);

            Assert.Equal(Encoding.BigEndianUnicode.GetPreamble(), encoding.GetPreamble());
        }

        [Fact]
        public void GetEncoding_EmptyFile_ReturnsEncodingWithNoPreamble()
        {
            var path = CreateTempFile(Array.Empty<byte>());

            var encoding = ModificationHistoryFileWriter.GetEncoding(path);

            Assert.Empty(encoding.GetPreamble());
        }
    }
}
