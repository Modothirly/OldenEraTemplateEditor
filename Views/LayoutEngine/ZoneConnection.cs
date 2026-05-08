namespace OldenEraTemplateEditor.Views.LayoutEngine
{
    /// <summary>
    /// 两个区域之间的连接关系
    /// </summary>
    public class ZoneConnection
    {
        public string Name { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string ConnectionType { get; set; } = "Direct";  // Direct / Portal / Proximity
        public int GuardValue { get; set; }
    }
}
