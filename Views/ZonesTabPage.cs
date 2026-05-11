using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Views
{
    public class ZonesTabPage : EditorTabPage
    {
        private ToolStrip toolStrip;
        private TreeView treeView;
        private ZonesPanel zonesPanel;
        private PropertyGrid propertyGrid;

        public ZonesTabPage(Rmg rmg, Settings settings) : base(rmg, settings)
        {
            Text = "Zones";
            InitUI();
            RefreshData();
        }

        public override void RefreshData()
        {
            // TODO
            RefreshTree();
            zonesPanel.Invalidate();

        }
        public void RefreshTree()
        {
            treeView.Nodes.Clear();
            if (rmg.rmgTemplate.Variants == null)
            {
                rmg.rmgTemplate.Variants = new();
            }
            var variants = rmg.rmgTemplate.Variants;
            for (int i = 0; i < variants.Count(); i++)
            {
                var variant = rmg.rmgTemplate.Variants[i];
                var variantModel = rmg.variantList[i];
                TreeNode variantNode = treeView.Nodes.Add("variant " + i);
                variantNode.Tag = variant;
                TreeNode zones = variantNode.Nodes.Add("zones");
                TreeNode connections = variantNode.Nodes.Add("connections");

                foreach (Zone zone in variant.Zones)
                {
                    TreeNode zoneNode = zones.Nodes.Add(zone.Name);
                    zoneNode.Tag = zone;
                    if (zone.MainObjects == null)
                    {
                        zone.MainObjects = new();
                    }
                    for (int j = 0; j < zone.MainObjects?.Count(); j++)
                    {
                        TreeNode mainObjectNode = zoneNode.Nodes.Add(j + " : " + zone.MainObjects[j].Type);
                        mainObjectNode.Tag = zone.MainObjects[j];
                    }
                }
                foreach (Connection connection in variant.Connections)
                {
                    TreeNode connectionNode = connections.Nodes.Add(connection.Name);
                    connectionNode.Tag = connection;
                }
            }
        }


        void InitUI()
        {
            var mainSplit = new SplitContainer();
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.SplitterDistance = 100;

            var leftSplit = new SplitContainer();
            leftSplit.Dock = DockStyle.Fill;
            leftSplit.SplitterDistance = 30;

            this.treeView = new TreeView();
            treeView.Dock = DockStyle.Fill;

            this.zonesPanel = new ZonesPanel(rmg, (o) => { this.propertyGrid.SelectedObject = o; });
            zonesPanel.Dock = DockStyle.Fill;
            zonesPanel.BackColor = Color.FromArgb(250, 250, 250);

            this.propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.PropertySort = PropertySort.Categorized;

            leftSplit.Panel1.Controls.Add(treeView);
            leftSplit.Panel2.Controls.Add(zonesPanel);

            mainSplit.Panel1.Controls.Add(leftSplit);
            mainSplit.Panel2.Controls.Add(propertyGrid);

            Controls.Add(mainSplit);

            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;

            // TODO
            toolStrip.Items.Add(new ToolStripButton("Add Node"));
            toolStrip.Items.Add(new ToolStripButton("Auto Layout"));
            Controls.Add(toolStrip);

            treeView.AfterSelect += treeView_AfterSelect;

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag != null)
            {
                this.propertyGrid.SelectedObject = e.Node.Tag;

            }
            TreeNode node = e.Node;
            while (node != null && node.Tag is not Variant)
            {
                node = node.Parent;
            }
            if (node != null && node.Tag is Variant)
            {
                zonesPanel.setCurrentVariantIndex(treeView.Nodes.IndexOf(node));
            }
        }


    }
}
