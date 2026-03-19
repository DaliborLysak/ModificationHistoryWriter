namespace ModificationHistoryWriter
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds the configuration used to format a modification history entry.
    /// An instance of this class is loaded from <c>pattern.json</c> and passed
    /// to <see cref="IModificationHistoryFormater.Format"/> at runtime.
    /// </summary>
    public class ModificationHistoryPattern
    {
        /// <summary>
        /// The output template for a single history line.
        /// The following tokens are replaced at format time:
        /// <list type="bullet">
        ///   <item><term>DATE</term><description>Today's date formatted with <see cref="DateFormat"/>.</description></item>
        ///   <item><term>AUTHOR</term><description>The value of <see cref="Author"/>.</description></item>
        ///   <item><term>TICKET</term><description>The ticket identifier extracted from the input.</description></item>
        ///   <item><term>MESSAGE</term><description>The description text extracted from the input.</description></item>
        /// </list>
        /// Example: <c>// DATE  AUTHOR  TICKET          MESSAGE</c>
        /// </summary>
        public string Pattern { get; set; } = string.Empty;

        /// <summary>
        /// A .NET date format string used to render today's date into the <c>DATE</c> token.
        /// Example: <c>dd.MM.yyyy</c>
        /// </summary>
        public string DateFormat { get; set; } = string.Empty;

        /// <summary>
        /// The author name inserted into the <c>AUTHOR</c> token.
        /// Accented characters are automatically stripped before writing.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// A regular expression applied to the raw input string (e.g. a commit message).
        /// <para>
        /// Expected capture groups:
        /// <list type="bullet">
        ///   <item><term>Group 1</term><description>Full ticket identifier (e.g. <c>REQ1234</c>).</description></item>
        ///   <item><term>Group 3</term><description>Description / message text.</description></item>
        /// </list>
        /// </para>
        /// Example: <c>((REQ|DEF)\d*)\s*(.*)</c>
        /// </summary>
        public string TicketPattern { get; set; } = string.Empty;
    }
}