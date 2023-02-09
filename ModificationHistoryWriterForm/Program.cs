using System.Windows.Forms;

namespace ModificationHistoryWriterForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var form = new ModificationHistoryWriterForm
            {
                ApplicationArguments = args
            };
            Application.Run(form);
        }
    }
}