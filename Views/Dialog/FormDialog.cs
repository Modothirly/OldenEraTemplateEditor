using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldenEraTemplateEditor.Views.Dialog
{
    public class FormDialog<T> : Form
    {
        private PropertyGrid grid;
        private Button okBtn;
        private Button cancelBtn;

        protected virtual string title => "";
        protected virtual int width => 600;
        protected virtual int height => 400;

        public FormDialog(T obj)
        {
            Text = title;
            Width = width;
            Height = height;
            StartPosition = FormStartPosition.CenterParent;

            // ===== Grid =====
            grid = new PropertyGrid
            {
                Dock = DockStyle.Fill,
            };
            grid.SelectedObject = obj;

            // ===== Bottom panel =====
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            okBtn = new Button
            {
                Text = "OK",
                Width = 80,
                Left = 380,
                Top = 10,
                DialogResult = DialogResult.OK
            };

            cancelBtn = new Button
            {
                Text = "Cancel",
                Width = 80,
                Left = 470,
                Top = 10,
                DialogResult = DialogResult.Cancel
            };

            okBtn.Click += OkBtn_Click;

            panel.Controls.Add(okBtn);
            panel.Controls.Add(cancelBtn);

            Controls.Add(grid);
            Controls.Add(panel);

            AcceptButton = okBtn;
            CancelButton = cancelBtn;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}