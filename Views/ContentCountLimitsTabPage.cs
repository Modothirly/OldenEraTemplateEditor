using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;

namespace OldenEraTemplateEditor.Views
{
    public class ContentCountLimitsTabPage : EditorTabPage
    {
        private TreeView treeView;
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;

        public ContentCountLimitsTabPage(Rmg rmg) : base(rmg)
        {
            Text = "ContentCountLimits";
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
            if (rmg.rmgTemplate.ContentCountLimits == null)
            {
                rmg.rmgTemplate.ContentCountLimits = new();
            }
            foreach (var limit in rmg.rmgTemplate.ContentCountLimits)
            {
                TreeNode limitNode = treeView.Nodes.Add(limit.Name);
                limitNode.Tag = limit;
                if (limit.Limits != null)
                {
                    foreach (var sidLimit in limit.Limits)
                    {
                        TreeNode itemNode = limitNode.Nodes.Add($"{sidLimit.Sid} ({sidLimit.MaxCount})");
                        itemNode.Tag = sidLimit;
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
                var dto = new ContentCountLimitGroupDto();
                var dialog = new ContentCountLimitGroupDialog(dto);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    rmg.AddContentCountLimit(dto);
                }
                RefreshTree();
            };

            addItemBtn.Click += (s, e) =>
            {
                ContentCountLimit? targetLimit = null;

                if (treeView.SelectedNode?.Tag is ContentCountLimit g)
                {
                    targetLimit = g;
                }
                else if (treeView.SelectedNode?.Tag is ContentSidLimit)
                {
                    var parentNode = treeView.SelectedNode.Parent;
                    if (parentNode?.Tag is ContentCountLimit pg)
                    {
                        targetLimit = pg;
                    }
                }

                if (targetLimit == null) return;
                var dto = new ContentCountLimitItemDto();
                var dialog = new ContentCountLimitItemDialog(dto);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    rmg.AddContentCountLimitItem(targetLimit, dto);
                }
                RefreshTree();
            };

            deleteBtn.Click += (s, e) =>
            {
                if (treeView.SelectedNode == null) return;

                if (treeView.SelectedNode.Tag is ContentCountLimit limit)
                {
                    if (!rmg.DeleteContentCountLimit(limit))
                    {
                        MessageBox.Show("This limit is still referenced by a zone.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (treeView.SelectedNode.Tag is ContentSidLimit item)
                {
                    var parentNode = treeView.SelectedNode.Parent;
                    if (parentNode?.Tag is ContentCountLimit pg)
                    {
                        rmg.DeleteContentCountLimitItem(pg, item);
                    }
                }
                RefreshTree();
            };

            copyNameBtn.Click += (s, e) =>
            {
                if (treeView.SelectedNode?.Tag is ContentCountLimit limit)
                {
                    Clipboard.SetText(limit.Name);
                }
            };
        }
    }
}
