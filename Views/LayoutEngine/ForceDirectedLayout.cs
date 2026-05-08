namespace OldenEraTemplateEditor.Views.LayoutEngine
{
    /// <summary>
    /// 力导向图布局引擎。
    /// 根据区域节点和连接关系自动计算画布坐标。
    /// </summary>
    public class ForceDirectedLayout
    {
        // 布局参数
        private const double RepulsionForce = 5000;    // 排斥力系数
        private const double AttractionForce = 0.01;   // 吸引力系数
        private const double Damping = 0.9;            // 阻尼（每次迭代速度衰减）
        private const int MaxIterations = 300;          // 最大迭代次数
        private const double MinDelta = 0.5;           // 收敛阈值

        /// <summary>
        /// 对给定的节点和连接执行自动布局，结果写回节点的 X/Y
        /// </summary>
        /// <param name="nodes">区域节点列表</param>
        /// <param name="connections">连接关系列表</param>
        /// <param name="canvasWidth">画布宽度</param>
        /// <param name="canvasHeight">画布高度</param>
        public void AutoLayout(List<ZoneNode> nodes, List<ZoneConnection> connections,
            double canvasWidth = 800, double canvasHeight = 600)
        {
            if (nodes.Count == 0) return;

            // 1. 初始位置：均匀放在圆上
            InitCircleLayout(nodes, Math.Min(canvasWidth, canvasHeight) * 0.35);

            // 2. 力导向迭代
            for (int iter = 0; iter < MaxIterations; iter++)
            {
                double totalDelta = ApplyForces(nodes, connections);
                if (totalDelta < MinDelta) break;
            }

            // 3. 清零速度
            foreach (var node in nodes)
            {
                node.Vx = 0;
                node.Vy = 0;
            }

            // 4. 平移到画布中心
            CenterOnCanvas(nodes, canvasWidth, canvasHeight);
        }

        /// <summary>
        /// 初始布局：将节点均匀分布在圆周上
        /// </summary>
        private void InitCircleLayout(List<ZoneNode> nodes, double radius)
        {
            double angleStep = 2 * Math.PI / nodes.Count;
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].X = radius * Math.Cos(angleStep * i);
                nodes[i].Y = radius * Math.Sin(angleStep * i);
                nodes[i].Vx = 0;
                nodes[i].Vy = 0;
            }
        }

        /// <summary>
        /// 执行一轮力计算并更新位置，返回总位移量
        /// </summary>
        private double ApplyForces(List<ZoneNode> nodes, List<ZoneConnection> connections)
        {
            double totalDelta = 0;

            // 排斥力：每对节点之间
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    double dx = nodes[j].X - nodes[i].X;
                    double dy = nodes[j].Y - nodes[i].Y;
                    double distSq = dx * dx + dy * dy;
                    double dist = Math.Max(Math.Sqrt(distSq), 1);

                    double force = RepulsionForce / Math.Max(distSq, 1);
                    double fx = force * dx / dist;
                    double fy = force * dy / dist;

                    nodes[i].Vx -= fx;
                    nodes[i].Vy -= fy;
                    nodes[j].Vx += fx;
                    nodes[j].Vy += fy;
                }
            }

            // 吸引力：有连接的节点之间
            foreach (var conn in connections)
            {
                var a = nodes.FirstOrDefault(n => n.Name == conn.From);
                var b = nodes.FirstOrDefault(n => n.Name == conn.To);
                if (a == null || b == null) continue;

                double dx = b.X - a.X;
                double dy = b.Y - a.Y;
                double dist = Math.Max(Math.Sqrt(dx * dx + dy * dy), 1);

                double force = AttractionForce * dist;
                double fx = force * dx / dist;
                double fy = force * dy / dist;

                a.Vx += fx;
                a.Vy += fy;
                b.Vx -= fx;
                b.Vy -= fy;
            }

            // 更新位置
            foreach (var node in nodes)
            {
                node.Vx *= Damping;
                node.Vy *= Damping;
                node.X += node.Vx;
                node.Y += node.Vy;
                totalDelta += Math.Abs(node.Vx) + Math.Abs(node.Vy);
            }

            return totalDelta;
        }

        /// <summary>
        /// 将所有节点平移，使其整体居中于画布
        /// </summary>
        private void CenterOnCanvas(List<ZoneNode> nodes, double canvasWidth, double canvasHeight)
        {
            if (nodes.Count == 0) return;

            double minX = nodes.Min(n => n.X);
            double maxX = nodes.Max(n => n.X);
            double minY = nodes.Min(n => n.Y);
            double maxY = nodes.Max(n => n.Y);

            double offsetX = (canvasWidth - (maxX - minX)) / 2 - minX;
            double offsetY = (canvasHeight - (maxY - minY)) / 2 - minY;

            foreach (var node in nodes)
            {
                node.X += offsetX;
                node.Y += offsetY;
            }
        }
    }
}
