using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views
{
    public class ToolStripFile
    {
        public bool NeedReset;
        public string? ResetFileName;
        public string? currentFilePath;

        public string label;
        public readonly IToolStripFileModel model;
        public Action OnFileOpened;

        public ToolStripFile(string label, IToolStripFileModel model, Action OnFileOpened)
        {
            NeedReset = false;
            ResetFileName = null;
            this.label = label;
            this.model = model;
            this.OnFileOpened = OnFileOpened;
        }
        public ToolStripFile(string label, IToolStripFileModel model, Action OnFileOpened, string ResetFileName)
        {
            NeedReset = true;
            this.ResetFileName = ResetFileName;
            this.label = label;
            this.model = model;
            this.OnFileOpened = OnFileOpened;
        }

        public void NewFile()
        {
            currentFilePath = null;
            model.neww();
            OnFileOpened.Invoke();
            MessageBox.Show("new success !");
        }

        public void OpenFile()
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = model.dialogFilter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                model.input(dialog.FileName);
                currentFilePath = dialog.FileName;
                OnFileOpened.Invoke();
                MessageBox.Show("Open success !");
            }
        }

        public void SaveFile()
        {
            if (currentFilePath != null)
            {
                model.output(currentFilePath);
                MessageBox.Show("Save success !");
            }
            else
            {
                SaveAsFile();
            }
        }

        public void SaveAsFile()
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = model.dialogFilter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                model.output(dialog.FileName);
                currentFilePath = dialog.FileName;
                MessageBox.Show("SaveAs success !");
            }
        }

        public void Reset()
        {
            OnFileOpened.Invoke();
        }

        public void CreateButtonGroup(ToolStrip toolStrip)
        {
            if (NeedReset)
            {
                var resetBtn = new ToolStripButton("Reset " + this.label);
                resetBtn.Click += (s, e) => this.Reset();
                toolStrip.Items.Add(resetBtn);
            }

            var newBtn = new ToolStripButton("New " + this.label);
            newBtn.Click += (s, e) => this.NewFile();
            toolStrip.Items.Add(newBtn);

            var openBtn = new ToolStripButton("Open " + this.label);
            openBtn.Click += (s, e) => this.OpenFile();
            toolStrip.Items.Add(openBtn);

            var saveBtn = new ToolStripButton("Save " + this.label);
            saveBtn.Click += (s, e) => this.SaveFile();
            toolStrip.Items.Add(saveBtn);

            var saveAsBtn = new ToolStripButton("SaveAs " + this.label);
            saveAsBtn.Click += (s, e) => this.SaveAsFile();
            toolStrip.Items.Add(saveAsBtn);
        }
    }
    public interface IToolStripFileModel
    {
        string dialogFilter { get; }

        public void neww();
        public void input(string path);
        public void output(string path);
    }
}