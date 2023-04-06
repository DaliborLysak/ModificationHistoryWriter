using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void toolStripButtonShowPattern_Click(object sender, EventArgs e)
        {
            this.listBoxFiles.Items.Clear();
            this.listBoxFiles.Items.Add($"{nameof(pattern.Pattern)}: {pattern.Pattern}");
            this.listBoxFiles.Items.Add($"{nameof(pattern.DateFormat)}: {pattern.DateFormat}");
            this.listBoxFiles.Items.Add($"{nameof(pattern.Author)}: {pattern.Author}");
            this.listBoxFiles.Items.Add($"{nameof(pattern.TicketPattern)}: {pattern.TicketPattern}");
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
    }
}