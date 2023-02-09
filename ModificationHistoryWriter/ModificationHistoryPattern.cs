namespace ModificationHistoryWriter
{
    using System.Collections.Generic;

    public class ModificationHistoryPattern
    {
        public string Pattern { get; set; } = String.Empty;
        public string DateFormat { get; set; } = String.Empty;
        public string Author { get; set; } = String.Empty;
        public string TicketPattern { get; set; } = String.Empty;
    }
}