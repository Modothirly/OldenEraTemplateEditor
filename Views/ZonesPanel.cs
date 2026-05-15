using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Services;
using OldenEraTemplateEditor.Views.Dialog;
using OldenEraTemplateEditor.Views.LayoutEngine;
using OldenEraTemplateEditor.Views.PanelSupport;

namespace OldenEraTemplateEditor.Views
{
    public class ZonesPanel : Panel
    {

        public readonly Rmg rmg;

        private int currentVariantIndex = 0;
        private Point viewOffset = new Point(0, 0);

        public PanelMod PanelMod = PanelMod.None;

        private Point lastMouse;

        private Selection selection;

        private string? connectionFrom;  // 连线起点 zone name
        private Point tempLineEnd;       // 临时线终点（屏幕坐标）

        public event Action<object> SelectionChanged;

        public ZonesPanel(Rmg rmg, Action<object> SelectionChanged)
        {
            this.rmg = rmg;
            DoubleBuffered = true;
            this.Paint += RefreshZonesPanel;
            this.MouseDown += panel_MouseDown;
            this.MouseMove += panel_MouseMove;
            this.MouseUp += panel_MouseUp;
            this.SelectionChanged = SelectionChanged;
            this.MouseLeave += (s, e) => panel_MouseUp(s, null);
        }

        public int getCurrentVariantIndex()
        {
            return currentVariantIndex;
        }

        public void setCurrentVariantIndex(int index)
        {
            if (currentVariantIndex != index)
            {
                currentVariantIndex = index;
                this.Invalidate();
            }
        }

        private void RefreshZonesPanel(object sender, PaintEventArgs e)
        {
            if (currentVariantIndex < 0 || currentVariantIndex >= rmg.variantList.Count)
                return;
            Graphics g = e.Graphics;
            g.TranslateTransform(viewOffset.X, viewOffset.Y);

            var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
            var variantModel = rmg.variantList[currentVariantIndex];

            ImageSupport.drawImage(variant, variantModel, g);
            // 画临时连线
            if (connectionFrom != null && variantModel.ZoneNodeDict.ContainsKey(connectionFrom))
            {
                var fromNode = variantModel.ZoneNodeDict[connectionFrom];
                var pen = new Pen(Color.Red, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                g.DrawLine(pen, (float)fromNode.X, (float)fromNode.Y,
                    tempLineEnd.X - viewOffset.X, tempLineEnd.Y - viewOffset.Y);
                pen.Dispose();
            }


        }

        private void ShowSelectionMenu<T>(List<T> items, Func<T, string> getName, Action<T> onSelect, Point location)
        {
            if (items.Count == 1)
            {
                onSelect(items[0]);
                return;
            }
            var menu = new ContextMenuStrip();
            foreach (var item in items)
            {
                var menuItem = menu.Items.Add(getName(item));
                menuItem.Tag = item;
                menuItem.Click += (s, args) =>
                {
                    onSelect((T)((ToolStripItem)s).Tag);
                };
            }
            menu.Show(this, location);
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (PanelMod == PanelMod.AddZone)
            {
                ZoneFormDto ZoneFormDto = new();
                ZoneFormDialog ZoneFormDialog = new(ZoneFormDto);
                if (ZoneFormDialog.ShowDialog() == DialogResult.OK)
                {
                    rmg.AddZone(ZoneFormDto, currentVariantIndex, e.X - viewOffset.X, e.Y - viewOffset.Y);
                    this.Invalidate();
                }
            }
            else if (PanelMod == PanelMod.AddConnection)
            {
                var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
                var variantModel = rmg.variantList[currentVariantIndex];
                selection = Selection.HitTest(e.Location, viewOffset, variant, variantModel);
                if (selection.Type == SelectionType.Zone)
                {
                    connectionFrom = selection.zone.Name;
                    tempLineEnd = e.Location;
                }
            }
            else if (PanelMod == PanelMod.DeleteZone)
            {
                var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
                var variantModel = rmg.variantList[currentVariantIndex];
                selection = Selection.HitTest(e.Location, viewOffset, variant, variantModel);
                if (selection.Type == SelectionType.Zone)
                {
                    var deleteZoneName = selection.zone.Name;
                    rmg.DeleteZone(deleteZoneName, currentVariantIndex);
                    this.Invalidate();
                }
            }
            else if (PanelMod == PanelMod.DeleteConnection)
            {
                var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
                var variantModel = rmg.variantList[currentVariantIndex];
                selection = Selection.HitTest(e.Location, viewOffset, variant, variantModel);
                if (selection.Type == SelectionType.Connection)
                {
                    ShowSelectionMenu(selection.connections,
                           c => (c.Name ?? $"{c.From}_{c.To}") + "(" + c.GuardValue + ")",
                           c =>
                           {
                               rmg.DeleteConnection(c.Name, currentVariantIndex);
                               this.Invalidate();
                           },
                           e.Location);

                }
            }
            else
            {
                var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
                var variantModel = rmg.variantList[currentVariantIndex];

                lastMouse = e.Location;
                selection = Selection.HitTest(e.Location, viewOffset, variant, variantModel);

                switch (selection.Type)
                {
                    case SelectionType.None:
                        PanelMod = PanelMod.DraggingCanvas;
                        break;
                    case SelectionType.Zone:
                        PanelMod = PanelMod.DraggingZone;
                        SelectionChanged(selection.zone);
                        break;
                    case SelectionType.Connection:
                        ShowSelectionMenu(selection.connections,
                            c => (c.Name ?? $"{c.From}_{c.To}") + "(" + c.GuardValue + ")",
                            c => SelectionChanged(c),
                            e.Location);
                        break;
                }
            }

        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastMouse.X;
            int dy = e.Y - lastMouse.Y;

            // 拖拽连线时画临时线
            if (PanelMod == PanelMod.AddConnection && connectionFrom != null)
            {
                tempLineEnd = e.Location;
                this.Invalidate();
            }
            else if (PanelMod == PanelMod.DraggingZone && selection.Type == SelectionType.Zone)
            {
                selection.zoneNode.X += dx;
                selection.zoneNode.Y += dy;

                this.Invalidate();
            }
            else if (PanelMod == PanelMod.DraggingCanvas)
            {
                viewOffset.X += dx;
                viewOffset.Y += dy;
                this.Invalidate();
            }
            lastMouse = e.Location;
        }
        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (PanelMod == PanelMod.AddConnection && connectionFrom != null && e != null)
            {
                var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
                var variantModel = rmg.variantList[currentVariantIndex];
                var hit = Selection.HitTest(e.Location, viewOffset, variant, variantModel);
                if (hit.Type == SelectionType.Zone && hit.zone.Name != connectionFrom)
                {
                    ConnectionFormDto ConnectionFormDto = new()
                    {
                        From = connectionFrom,
                        To = hit.zone.Name
                    };
                    ConnectionFormDialog ConnectionFormDialog = new(ConnectionFormDto);
                    if (ConnectionFormDialog.ShowDialog() == DialogResult.OK)
                    {
                        rmg.AddConnection(ConnectionFormDto, currentVariantIndex);
                        this.Invalidate();
                    }
                }
                connectionFrom = null;
                this.Invalidate();
            }
            else if (PanelMod == PanelMod.DraggingCanvas || PanelMod == PanelMod.DraggingZone)
            {
                PanelMod = PanelMod.None;
            }
        }

    }
}