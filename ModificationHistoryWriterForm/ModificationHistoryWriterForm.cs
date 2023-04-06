using ModificationHistoryWriter;

namespace ModificationHistoryWriterForm
{
    public partial class ModificationHistoryWriterForm : Form
    {
        public ModificationHistoryWriterForm()
        {
            InitializeComponent();

            pattern = ModificationHistoryWriterProvider.ModificationHistoryPatternLoader.Load();
        }

        private static ModificationHistoryPattern pattern = new();

        private const string HEADER_INPLACE_PATTERN = 
@"// MODIFICATION HISTORY
// -----------------------------------------------------------------------------
";

        public string[] ApplicationArguments { get; set; } = Array.Empty<string>();
        private string lastLine = string.Empty;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonFormat_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var line = ModificationHistoryWriterProvider.ModificationHistoryFormater.Format(pattern, Clipboard.GetText());
                richTextBoxInput.Text = $"{HEADER_INPLACE_PATTERN}{line}";
                this.lastLine = line;

                ToastContentBuilderHelper.ShowMessage("Moddification history formated:", $"{this.lastLine}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonShowPattern_Click(object sender, EventArgs e)
        {
            var patternInfo = $"{nameof(pattern.Pattern)}: {pattern.Pattern}";
            var dateFormatInfo = $"{nameof(pattern.DateFormat)}: {pattern.DateFormat}";
            var authorInfo = $"{nameof(pattern.Author)}: {pattern.Author}";
            var ticketPatternInfo = $"{nameof(pattern.TicketPattern)}: {pattern.TicketPattern}";

            this.listBoxFiles.Items.Clear();
            this.listBoxFiles.Items.Add(patternInfo);
            this.listBoxFiles.Items.Add(dateFormatInfo);
            this.listBoxFiles.Items.Add(authorInfo);
            this.listBoxFiles.Items.Add(ticketPatternInfo);

            ToastContentBuilderHelper.ShowMessage(
                "Pattern Info",
                $"{patternInfo}{Environment.NewLine}{dateFormatInfo}{Environment.NewLine}{authorInfo}{Environment.NewLine}{ticketPatternInfo}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            this.listBoxFiles.Items.Clear();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            var files = Array.Empty<string>();

            if (e is DragEventArgs dragEventArgs)
            {
                files = dragEventArgs.Data?.GetData(DataFormats.FileDrop) as string[] ?? Array.Empty<string>();
            }

            this.AddFilesToListBox(files);
        }

        private void AddFilesToListBox(string[] fileNames)
        {
            listBoxFiles.Items.Clear();
            listBoxFiles.Items.AddRange(fileNames);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e is DragEventArgs dragEventArgs)
            {
                dragEventArgs.Effect = DragDropEffects.All;
            }
        }

        private void ModificationHistoryWriterForm_Shown(object sender, EventArgs e)
        {
            AddFilesToListBox(ApplicationArguments as string[]);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.lastLine))
                Clipboard.SetText(this.lastLine);

            ToastContentBuilderHelper.ShowMessage("Copy to clipboard", $"{this.lastLine}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.lastLine))
                return;

            Cursor.Current = Cursors.WaitCursor;
            this.Enabled = false;
            try
            {
                var tasks = new List<Task>();
                foreach (string path in listBoxFiles.Items)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ModificationHistoryWriterProvider.ModificationHistoryFileWriter.Write(path, this.lastLine);
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                ToastContentBuilderHelper.ShowMessage("Modification history was written to all files", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
            }
            catch (AggregateException exc)
            {
                listBoxFiles.Items.Clear();
                listBoxFiles.Items.Add(exc.ToString());

                if (exc.InnerExceptions.Count > 0)
                    listBoxFiles.Items.AddRange(exc.InnerExceptions.ToArray());
            }
            finally
            {
                this.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void ModificationHistoryWriterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToastContentBuilderHelper.ClearAndClose();
        }
    }
}