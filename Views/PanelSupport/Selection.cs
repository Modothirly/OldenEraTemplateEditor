using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Views.PanelSupport
{
    public enum SelectionType
    {
        None,
        Zone,
        Connection
    }
    public class Selection
    {
        public SelectionType Type = SelectionType.None;
        public ZoneNode zoneNode;
        public Zone zone;
        public List<Connection> connections;

        public static Selection HitTest(Point mouse, Point viewOffset, Variant variant, VariantModel variantModel)
        {

            int worldX = mouse.X - viewOffset.X;
            int worldY = mouse.Y - viewOffset.Y;

            // 1. 先检测节点
            foreach (var zoneNode in variantModel.ZoneNodeDict.Values.Reverse())
            {
                Rectangle rect = new Rectangle(
                    (int)zoneNode.X - 50,
                    (int)zoneNode.Y - 50,
                    100,
                    100);

                if (rect.Contains(worldX, worldY))
                {
                    return new Selection
                    {
                        Type = SelectionType.Zone,
                        zoneNode = zoneNode,
                        zone = findZone(zoneNode, variant)
                    };
                }
            }

            List<Connection> connections = new();
            // 2. 再检测连线（点到线的距离）
            foreach (var conn in variant.Connections)
            {
                if (!variantModel.ZoneNodeDict.ContainsKey(conn.From) ||
                    !variantModel.ZoneNodeDict.ContainsKey(conn.To))
                    continue;

                var from = variantModel.ZoneNodeDict[conn.From];
                var to = variantModel.ZoneNodeDict[conn.To];

                if (DistanceToSegment(worldX, worldY,
                    from.X, from.Y,
                    to.X, to.Y) < 6)
                {
                    connections.Add(conn);
                }
            }
            if (connections.Count > 0)
            {
                return new Selection
                {
                    Type = SelectionType.Connection,
                    connections = connections
                };
            }

            return new Selection { Type = SelectionType.None };
        }

        private static double DistanceToSegment(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;

            if (dx == 0 && dy == 0)
                return Math.Sqrt((px - x1) * (px - x1) + (py - y1) * (py - y1));

            double t = ((px - x1) * dx + (py - y1) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            double projX = x1 + t * dx;
            double projY = y1 + t * dy;

            return Math.Sqrt((px - projX) * (px - projX) +
                             (py - projY) * (py - projY));
        }

        private static Zone? findZone(ZoneNode zoneNode, Variant variant)
        {
            foreach (Zone zone in variant.Zones)
            {
                if (zone.Name == zoneNode.Name)
                {
                    return zone;
                }
            }
            return null;
        }
    }
}
