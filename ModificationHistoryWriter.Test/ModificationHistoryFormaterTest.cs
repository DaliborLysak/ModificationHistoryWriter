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

        [Fact]
        public void Format_AuthorContainsDateToken_DoesNotDoubleReplaceDate()
        {
            var formater = new ModificationHistoryFormater();
            var pattern = new ModificationHistoryPattern
            {
                Author = "DATE Smith",
                DateFormat = "dd.MM.yyyy",
                Pattern = "// DATE  AUTHOR  TICKET  MESSAGE",
                TicketPattern = "((IMP|BUG)\\d*)\\s*(.*)"
            };

            var log = formater.Format(pattern, "IMP1 fix");

            // "DATE" inside the author value must not be replaced with the actual date
            Assert.Contains("DATE Smith", log);
            // The DATE token in the pattern must still be replaced with today's date
            Assert.Contains(DateTime.Today.ToString("dd.MM.yyyy"), log);
        }

        [Fact]
        public void Format_MessageContainsTicketToken_DoesNotDoubleReplaceTicket()
        {
            var formater = new ModificationHistoryFormater();
            var log = formater.Format(DefaultPattern, "IMP666 fix TICKET overflow");

            // "TICKET" inside the message must stay as literal text, not be replaced again
            Assert.Contains("fix TICKET overflow", log);
            Assert.Contains("IMP666", log);
        }

        [Fact]
        public void Format_MultipleRegexMatches_UsesFirstMatchOnly()
        {
            var formater = new ModificationHistoryFormater();
            // Two matches on separate lines so each message doesn't bleed into the other
            var log = formater.Format(DefaultPattern, "IMP1 first\nIMP2 second");

            Assert.Contains("IMP1", log);
            Assert.Contains("first", log);
            Assert.DoesNotContain("IMP2", log);
        }
    }
}