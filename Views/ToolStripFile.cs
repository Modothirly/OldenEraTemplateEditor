using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace OldenEraTemplateEditor.Views
{
    public class ToolStripFile
    {
        public bool NeedReset;
        public string? ResetFileName;
        public string? content;
        public string? currentFilePath;

        public string label;
        public readonly IToolStripFileModel model;

        public ToolStripFile(string label, IToolStripFileModel model)
        {
            NeedReset = false;
            ResetFileName = null;
            this.label = label;
            this.model = model;
        }
        public ToolStripFile(string label, IToolStripFileModel model, string ResetFileName)
        {
            NeedReset = true;
            this.ResetFileName = ResetFileName;
            this.label = label;
            this.model = model;
        }

        public void OpenFile()
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "JSON Files (*.json)|*.json";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = dialog.FileName;
                content = File.ReadAllText(dialog.FileName);
                model.input(content);

                MessageBox.Show("Open success !");
            }
        }

        public void SaveFile()
        {
            if (currentFilePath != null)
            {
                content = model.output();
                File.WriteAllText(currentFilePath, content);
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
            dialog.Filter = "JSON Files (*.json)|*.json";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = dialog.FileName;

                content = model.output();
                File.WriteAllText(currentFilePath, content);
                MessageBox.Show("SaveAs success !");
            }
        }

        public void Reset()
        {

        }

        public void CreateButtonGroup(ToolStrip toolStrip)
        {
            if (NeedReset)
            {
                var resetBtn = new ToolStripButton("Reset " + this.label);
                resetBtn.Click += (s, e) => this.Reset();
                toolStrip.Items.Add(resetBtn);
            }

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
        public void input(string json);
        public string output();
    }
}