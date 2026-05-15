using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Views.PanelSupport
{
    public class ImageSupport
    {
        public static void drawImage(Variant variant, VariantModel variantModel, Graphics g)
        {
            ConnectionSupport ConnectionSupport = new();
            foreach (Connection connection in variant.Connections)
            {
                ConnectionSupport.collectConnection(connection, variantModel.ZoneNodeDict);
            }
            ConnectionSupport.draw(variantModel.ZoneNodeDict, g);
            foreach (Zone zone in variant.Zones)
            {
                ZoneNode zoneNode = variantModel.ZoneNodeDict[zone.Name];

                ZoneSupport.drawZone(zone, zoneNode, g);
            }
        }

        public static void outputImage(Variant variant, VariantModel variantModel, string filePath)
        {
            const int zoneSize = 100;
            const int halfZone = zoneSize / 2;

            const int padding = halfZone;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            if (variant.Zones.Count == 0)
                return;
            foreach (Zone zone in variant.Zones)
            {
                var node = variantModel.ZoneNodeDict[zone.Name];

                float left = (float)node.X - halfZone;
                float top = (float)node.Y - halfZone;
                float right = (float)node.X + halfZone;
                float bottom = (float)node.Y + halfZone;

                minX = Math.Min(minX, left);
                minY = Math.Min(minY, top);

                maxX = Math.Max(maxX, right);
                maxY = Math.Max(maxY, bottom);
            }
            int width = (int)(maxX - minX) + padding * 2;
            int height = (int)(maxY - minY) + padding * 2;

            using Bitmap bmp = new Bitmap(width, height);

            using Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.Clear(Color.White);

            // 世界坐标重定位
            g.TranslateTransform(
                -minX + padding,
                -minY + padding
            );

            drawImage(variant, variantModel, g);
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}