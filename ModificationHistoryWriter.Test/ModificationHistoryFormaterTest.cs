namespace ModificationHistoryWriter.Test
{
    public class ModificationHistoryFormaterTest
    {
        [Theory]
        [InlineData("IMP666 improvement", "// 09.02.2023  Author  IMP666          improvement")]
        [InlineData("BUG666 bug", "// 09.02.2023  Author  BUG666          bug")]
        public void FormatTest(string ticketIdAndName, string logLine)
        {
            var formater = new ModificationHistoryFormater();
            var log = formater.Format(
                new ModificationHistoryPattern()
                {
                    Author = "Author",
                    DateFormat = "dd.MM.yyyy",
                    Pattern = "// DATE  AUTHOR  TICKET          MESSAGE",
                    TicketPattern = "((IMP|BUG)\\d*)\\s*(.*)"
                },
                ticketIdAndName);

            Assert.Equal(logLine, log);
        }
    }
}