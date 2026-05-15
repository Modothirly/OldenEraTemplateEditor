using System.Text.Json.Serialization;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Models
{
    public class VariantModel
    {
        [JsonPropertyName("zoneNodeDict")]
        public Dictionary<string, ZoneNode> ZoneNodeDict { get; set; } = new();

        public void RebuildCanvasData(Variant variant)
        {
            ZoneNodeDict.Clear();

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

            if (variant.Connections == null)
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
