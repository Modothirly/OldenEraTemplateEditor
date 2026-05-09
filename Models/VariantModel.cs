using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Models
{
    public class VariantModel
    {
        public Dictionary<string, ZoneNode> zoneNodeDict = new();
        public Dictionary<string, ZoneConnection> zoneConnectionDict = new();

        public void RebuildCanvasData(Variant variant)
        {
            zoneNodeDict.Clear();
            zoneConnectionDict.Clear();

            // 构建 ZoneNode
            if (variant.Zones != null)
            {
                foreach (var zone in variant.Zones)
                {
                    zoneNodeDict[zone.Name] = new ZoneNode
                    {
                        Name = zone.Name,
                        Size = (float)(zone.Size ?? 1.0)
                    };
                }
            }
            else
            {
                variant.Zones = new();
            }

            // 构建 ZoneConnection
            if (variant.Connections != null)
            {
                foreach (var conn in variant.Connections)
                {
                    string connName = conn.Name ?? $"{conn.From}_{conn.To}";
                    zoneConnectionDict[connName] = new ZoneConnection
                    {
                        Name = connName,
                        From = conn.From,
                        To = conn.To,
                        ConnectionType = conn.ConnectionType ?? "Direct",
                        GuardValue = conn.GuardValue ?? 0
                    };
                }
            }
            else
            {
                variant.Connections = new();
            }
        }
    }


}