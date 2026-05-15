using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;

namespace OldenEraTemplateEditor.Views
{
    public class MandatoryContentTabPage : EditorTabPage
    {
        private TreeView treeView;
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;

        public MandatoryContentTabPage(Rmg rmg) : base(rmg)
        {
            Text = "MandatoryContent";
            InitUI();
            RefreshData();
        }

        public override void RefreshData()
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            treeView.Nodes.Clear();
            if (rmg.rmgTemplate.MandatoryContent == null)
            {
                rmg.rmgTemplate.MandatoryContent = new();
            }
            foreach (var group in rmg.rmgTemplate.MandatoryContent)
            {
                TreeNode groupNode = treeView.Nodes.Add(group.Name);
                groupNode.Tag = group;
                if (group.Content != null)
                {
                    foreach (var item in group.Content)
                    {
                        TreeNode itemNode = groupNode.Nodes.Add(item.Sid ?? item.Name ?? "item");
                        itemNode.Tag = item;
                    }
                }
            }
        }

        void InitUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 50;

            this.treeView = new TreeView();
            treeView.Dock = DockStyle.Fill;

            this.propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.PropertySort = PropertySort.Categorized;

            split.Panel1.Controls.Add(treeView);
            split.Panel2.Controls.Add(propertyGrid);

            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            InitToolStrip();

            Controls.Add(split);
            Controls.Add(toolStrip);

            treeView.AfterSelect += treeView_AfterSelect;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag != null)
            {
                propertyGrid.SelectedObject = e.Node.Tag;
            }
        }

        private void InitToolStrip()
        {
            var addGroupBtn = new ToolStripButton("➕Group");
            toolStrip.Items.Add(addGroupBtn);
            var addItemBtn = new ToolStripButton("➕Item");
            toolStrip.Items.Add(addItemBtn);
            var deleteBtn = new ToolStripButton("➖Delete");
            toolStrip.Items.Add(deleteBtn);
            var copyNameBtn = new ToolStripButton("📋CopyName");
            toolStrip.Items.Add(copyNameBtn);

            addGroupBtn.Click += (s, e) =>
            {
                MandatoryContentDialog mandatoryContentDialog = new(MandatoryContentDialogMode.Group);
                if (mandatoryContentDialog.ShowDialog() == DialogResult.OK)
                {
                    rmg.AddMandatoryContentGroup(mandatoryContentDialog.dto);
                }
                RefreshTree();
            };
            addItemBtn.Click += (s, e) =>
            {
                MandatoryContentGroup? targetGroup = null;

                if (treeView.SelectedNode?.Tag is MandatoryContentGroup g)
                {
                    targetGroup = g;
                }
                else if (treeView.SelectedNode?.Tag is ContentItem)
                {
                    var parentNode = treeView.SelectedNode.Parent;
                    if (parentNode?.Tag is MandatoryContentGroup pg)
                    {
                        targetGroup = pg;
                    }
                }

                if (targetGroup == null) return;
                MandatoryContentDialog mandatoryContentDialog = new(MandatoryContentDialogMode.Item);
                if (mandatoryContentDialog.ShowDialog() == DialogResult.OK)
                {
                    mandatoryContentDialog.dto.GroupName = targetGroup.Name;
                    rmg.AddMandatoryContentItem(mandatoryContentDialog.dto);
                }

                RefreshTree();
            };

            deleteBtn.Click += (s, e) =>
            {
                if (treeView.SelectedNode == null) return;

                if (treeView.SelectedNode.Tag is MandatoryContentGroup group)
                {
                    if (!rmg.DeleteMandatoryContentGroup(group))
                    {
                        MessageBox.Show("This group is still referenced by a zone.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (treeView.SelectedNode.Tag is ContentItem item)
                {
                    var parentNode = treeView.SelectedNode.Parent;
                    if (parentNode?.Tag is MandatoryContentGroup pg)
                    {
                        rmg.DeleteMandatoryContentItem(pg, item);
                    }
                }
                RefreshTree();
            };

            copyNameBtn.Click += (s, e) =>
            {
                if (treeView.SelectedNode?.Tag is MandatoryContentGroup group)
                {
                    Clipboard.SetText(group.Name);
                }
            };
        }
    }
}
