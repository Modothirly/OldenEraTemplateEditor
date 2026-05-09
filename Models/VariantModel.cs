using System.Text.Json.Serialization;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Models
{
    public class VariantModel
    {
        [JsonPropertyName("zoneNodeDict")]
        public Dictionary<string, ZoneNode> ZoneNodeDict { get; set; } = new();
        [JsonPropertyName("zoneConnectionDict")]
        public Dictionary<string, ZoneConnection> ZoneConnectionDict { get; set; } = new();

        public void RebuildCanvasData(Variant variant)
        {
            ZoneNodeDict.Clear();
            ZoneConnectionDict.Clear();

            // 构建 ZoneNode
            if (variant.Zones != null)
            {
                foreach (var zone in variant.Zones)
                {
                    ZoneNodeDict[zone.Name] = new ZoneNode
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
                    ZoneConnectionDict[connName] = new ZoneConnection
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

    public class VariantModelList
    {
        [JsonPropertyName("variantList")]
        public List<VariantModel> VariantList { get; set; }
    }
}