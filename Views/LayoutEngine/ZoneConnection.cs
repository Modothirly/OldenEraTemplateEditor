using System.Text.Json.Serialization;

namespace OldenEraTemplateEditor.Views.LayoutEngine
{
    /// <summary>
    /// 两个区域之间的连接关系
    /// </summary>
    public class ZoneConnection
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("from")]
        public string From { get; set; } = "";
        [JsonPropertyName("to")]
        public string To { get; set; } = "";
        [JsonPropertyName("connectionType")]
        public string ConnectionType { get; set; } = "Direct";  // Direct / Portal / Proximity
        [JsonPropertyName("guardValue")]
        public int GuardValue { get; set; }
    }
}
