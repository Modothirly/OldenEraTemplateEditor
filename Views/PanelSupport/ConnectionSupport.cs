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
        public static void drawConnection(Connection connection, Dictionary<string, ZoneNode> zoneNodeDict, Graphics g)
        {
            if (!zoneNodeDict.ContainsKey(connection.From) || !zoneNodeDict.ContainsKey(connection.To))
                return;
            var from = zoneNodeDict[connection.From];
            var to = zoneNodeDict[connection.To];
            g.DrawLine(Pens.Black, (float)from.X, (float)from.Y, (float)to.X, (float)to.Y);

            double x = (from.X + to.X) / 2;
            double y = (from.Y + to.Y) / 2;
            // name
            Font font = SystemFonts.DefaultFont;
            g.DrawString(connection.GuardValue + "", font, Brushes.Red, (float)x, (float)y);

        }
    }
}