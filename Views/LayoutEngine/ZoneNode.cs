namespace OldenEraTemplateEditor.Views.LayoutEngine
{
    /// <summary>
    /// 画布上的区域节点，包含坐标和力导向迭代速度
    /// </summary>
    public class ZoneNode
    {
        public string Name { get; set; } = "";
        public float Size { get; set; } = 1.0f;

        // 画布坐标
        public double X { get; set; }
        public double Y { get; set; }

        // 力导向迭代速度（每次布局后清零）
        public double Vx { get; set; }
        public double Vy { get; set; }
    }
}
