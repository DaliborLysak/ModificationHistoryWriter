namespace ModificationHistoryWriter.Test
{
    public class ModificationHistoryFormaterTest
    {
        private static readonly ModificationHistoryPattern DefaultPattern = new()
        {
            Author = "Author",
            DateFormat = "dd.MM.yyyy",
            Pattern = "// DATE  AUTHOR  TICKET          MESSAGE",
            TicketPattern = "((IMP|BUG)\\d*)\\s*(.*)"
        };

        [Theory]
        [InlineData("IMP666 improvement", "IMP666", "improvement")]
        [InlineData("BUG666 bug", "BUG666", "bug")]
        public void Format_ValidInput_ReturnsFormattedLine(string ticketIdAndName, string expectedTicket, string expectedMessage)
        {
            var date = DateTime.Today.ToString("dd.MM.yyyy");
            var expectedLine = $"// {date}  Author  {expectedTicket}          {expectedMessage}";

            var formater = new ModificationHistoryFormater();
            var log = formater.Format(DefaultPattern, ticketIdAndName);

            Assert.Equal(expectedLine, log);
        }

        [Fact]
        public void Format_NullPattern_ReturnsEmpty()
        {
            var formater = new ModificationHistoryFormater();
            var log = formater.Format(null!, "IMP666 improvement");

            Assert.Equal(string.Empty, log);
        }

        [Fact]
        public void Format_NullInput_ReturnsEmpty()
        {
            var formater = new ModificationHistoryFormater();
            var log = formater.Format(DefaultPattern, null!);

            Assert.Equal(string.Empty, log);
        }

        [Fact]
        public void Format_EmptyInput_ReturnsEmpty()
        {
            var formater = new ModificationHistoryFormater();
            var log = formater.Format(DefaultPattern, string.Empty);

            Assert.Equal(string.Empty, log);
        }

        [Fact]
        public void Format_InputWithNoPatternMatch_ReturnsUnchangedTemplate()
        {
            var formater = new ModificationHistoryFormater();
            // "XYZ999" does not match ((IMP|BUG)\d*) pattern
            var log = formater.Format(DefaultPattern, "XYZ999 no match");

            Assert.Equal(DefaultPattern.Pattern, log);
        }

        [Fact]
        public void Format_AuthorWithDiacritics_StripsDiacritics()
        {
            var formater = new ModificationHistoryFormater();
            var pattern = new ModificationHistoryPattern
            {
                Author = "Ján Novák",
                DateFormat = "dd.MM.yyyy",
                Pattern = "// DATE  AUTHOR  TICKET          MESSAGE",
                TicketPattern = "((IMP|BUG)\\d*)\\s*(.*)"
            };

            var log = formater.Format(pattern, "IMP1 fix");

            Assert.Contains("Jan Novak", log);
        }

        [Fact]
        public void Format_DateFormat_IsAppliedCorrectly()
        {
            var formater = new ModificationHistoryFormater();
            var pattern = new ModificationHistoryPattern
            {
                Author = "Author",
                DateFormat = "yyyy/MM/dd",
                Pattern = "DATE AUTHOR TICKET MESSAGE",
                TicketPattern = "((IMP|BUG)\\d*)\\s*(.*)"
            };

            var log = formater.Format(pattern, "IMP1 fix");

            Assert.Contains(DateTime.Today.ToString("yyyy/MM/dd"), log);
        }
    }
}