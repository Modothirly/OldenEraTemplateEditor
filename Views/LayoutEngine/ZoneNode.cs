using System.Text.Json.Serialization;

namespace OldenEraTemplateEditor.Views.LayoutEngine
{
    /// <summary>
    /// 画布上的区域节点，包含坐标和力导向迭代速度
    /// </summary>
    public class ZoneNode
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("size")]
        public float Size { get; set; } = 1.0f;

        // 画布坐标
        [JsonPropertyName("x")]
        public double X { get; set; }
        [JsonPropertyName("y")]
        public double Y { get; set; }

        // 力导向迭代速度（每次布局后清零）
        [JsonPropertyName("vx")]
        public double Vx { get; set; }
        [JsonPropertyName("vy")]
        public double Vy { get; set; }
    }
}
