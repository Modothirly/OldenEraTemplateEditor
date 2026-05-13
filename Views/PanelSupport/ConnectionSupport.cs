using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Views.PanelSupport
{
    public class ConnectionSupport
    {
        private Dictionary<(string, string), List<int>> ConnectionGuard = new();

        // 封装一个获取唯一 Key 的方法
        private List<int> GetGuard(string idA, string idB)
        {
            // 字符串排序，确保 (A, B) 和 (B, A) 返回的结果完全一致
            (string, string) key = string.Compare(idA, idB) < 0 ? (idA, idB) : (idB, idA);

            if (!ConnectionGuard.ContainsKey(key))
            {
                ConnectionGuard[key] = new();
            }
            return ConnectionGuard[key];
        }
        private void AddGuard(string idA, string idB, int? Guard)
        {
            // 字符串排序，确保 (A, B) 和 (B, A) 返回的结果完全一致
            (string, string) key = string.Compare(idA, idB) < 0 ? (idA, idB) : (idB, idA);

            if (!ConnectionGuard.ContainsKey(key))
            {
                ConnectionGuard[key] = new();
            }
            ConnectionGuard[key].Add(Guard ?? 0);

        }

        public void collectConnection(Connection connection, Dictionary<string, ZoneNode> zoneNodeDict)
        {
            if (!zoneNodeDict.ContainsKey(connection.From) || !zoneNodeDict.ContainsKey(connection.To))
                return;
            AddGuard(connection.From, connection.To, connection.GuardValue);

        }

        public void draw(Dictionary<string, ZoneNode> zoneNodeDict, Graphics g)
        {
            foreach (var key in ConnectionGuard.Keys)
            {
                var from = zoneNodeDict[key.Item1];
                var to = zoneNodeDict[key.Item2];
                g.DrawLine(Pens.Black, (float)from.X, (float)from.Y, (float)to.X, (float)to.Y);

                double x = (from.X + to.X) / 2;
                double y = (from.Y + to.Y) / 2;
                // guardValue
                Font font = SystemFonts.DefaultFont;

                var GuardValue = ConnectionGuard[key];
                g.DrawString(string.Join(",", GuardValue) + "", font, Brushes.Red, (float)x, (float)y);
            }

        }
    }
}
