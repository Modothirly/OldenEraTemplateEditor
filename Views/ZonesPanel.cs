using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;
using OldenEraTemplateEditor.Views.PanelSupport;

namespace OldenEraTemplateEditor.Views
{
    public class ZonesPanel : Panel
    {

        public readonly Rmg rmg;

        private int currentVariantIndex = 0;
        private Point viewOffset = new Point(0, 0);

        private bool draggingCanvas = false;
        private bool draggingZone = false;

        private Point lastMouse;

        private Selection selection;

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
            this.MouseLeave += (s, e) => { draggingCanvas = false; draggingZone = false; };
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

            foreach (Connection connection in variant.Connections)
            {
                ConnectionSupport.drawConnection(connection, variantModel.ZoneNodeDict, g);

            }
            foreach (Zone zone in variant.Zones)
            {
                ZoneNode zoneNode = variantModel.ZoneNodeDict[zone.Name];

                ZoneSupport.drawZone(zone, zoneNode, g);
            }


        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var variant = rmg.rmgTemplate.Variants[currentVariantIndex];
            var variantModel = rmg.variantList[currentVariantIndex];

            lastMouse = e.Location;
            selection = Selection.HitTest(e.Location, viewOffset, variant, variantModel);

            switch (selection.Type)
            {
                case SelectionType.None:
                    draggingCanvas = true;
                    break;
                case SelectionType.Zone:
                    draggingZone = true;
                    SelectionChanged(selection.zone);
                    break;
                case SelectionType.Connection:
                    SelectionChanged(selection.connection);
                    break;
            }
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastMouse.X;
            int dy = e.Y - lastMouse.Y;
            if (draggingZone && selection.Type == SelectionType.Zone)
            {
                selection.zoneNode.X += dx;
                selection.zoneNode.Y += dy;

                this.Invalidate();
            }
            else if (draggingCanvas)
            {
                viewOffset.X += dx;
                viewOffset.Y += dy;
                this.Invalidate();
            }
            lastMouse = e.Location;
        }
        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            draggingZone = false;
            draggingCanvas = false;
        }


    }
}