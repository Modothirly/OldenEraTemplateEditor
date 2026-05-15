using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;

namespace OldenEraTemplateEditor.Views
{
    public class ZoneLayoutsTabPage : EditorTabPage
    {
        private ListBox listBox;
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;

        public ZoneLayoutsTabPage(Rmg rmg) : base(rmg)
        {
            Text = "ZoneLayouts";
            InitUI();
            RefreshData();
        }

        public override void RefreshData()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            listBox.Items.Clear();
            if (rmg.rmgTemplate.ZoneLayouts == null)
            {
                rmg.rmgTemplate.ZoneLayouts = new();
            }
            foreach (var layout in rmg.rmgTemplate.ZoneLayouts)
            {
                listBox.Items.Add(layout);
            }
        }

        void InitUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 50;

            this.listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.DisplayMember = "Name";

            this.propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.PropertySort = PropertySort.Categorized;

            split.Panel1.Controls.Add(listBox);
            split.Panel2.Controls.Add(propertyGrid);

            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            InitToolStrip();

            Controls.Add(split);
            Controls.Add(toolStrip);

            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
        }

        private void listBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBox.SelectedItem is ZoneLayout layout)
            {
                propertyGrid.SelectedObject = layout;
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private void InitToolStrip()
        {
            var addBtn = new ToolStripButton("➕Add");
            toolStrip.Items.Add(addBtn);
            var deleteBtn = new ToolStripButton("➖Delete");
            toolStrip.Items.Add(deleteBtn);
            var copyNameBtn = new ToolStripButton("📋CopyName");
            toolStrip.Items.Add(copyNameBtn);

            addBtn.Click += (s, e) =>
            {
                var dto = new ZoneLayoutNameDto();
                var dialog = new ZoneLayoutNameDialog(dto);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    rmg.AddZoneLayout(dto.name);
                }
                RefreshList();
            };

            deleteBtn.Click += (s, e) =>
            {
                if (listBox.SelectedItem is not ZoneLayout layout) return;
                if (!rmg.DeleteZoneLayout(layout))
                {
                    MessageBox.Show("This layout is still referenced by a zone.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                RefreshList();
            };

            copyNameBtn.Click += (s, e) =>
            {
                if (listBox.SelectedItem is ZoneLayout layout)
                {
                    Clipboard.SetText(layout.Name);
                }
            };
        }
    }
}
