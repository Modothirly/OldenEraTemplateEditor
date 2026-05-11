using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Views.PanelSupport
{
    public class ZoneSupport
    {
        private static Brush ZoneBrush(Zone zone)
        {
            if (zone.MainObjects != null)
            {
                foreach (var mainObject in zone.MainObjects)
                {
                    if (mainObject.Type == "Spawn")
                    {
                        switch (mainObject.Spawn)
                        {
                            // 玩家 1：红色 (Red)
                            // 玩家 2：蓝色 (Blue)
                            // 玩家 3：棕色/褐色 (Tan/Brown)
                            // 玩家 4：绿色 (Green)
                            // 玩家 5：橙色 (Orange)
                            // 玩家 6：紫色 (Purple)
                            // 玩家 7：青色/浅蓝 (Teal)
                            // 玩家 8：粉色/粉红 (Pink)
                            case "Player1":
                                return Brushes.LightCoral;
                            case "Player2":
                                return Brushes.LightSkyBlue;
                            case "Player3":
                                return Brushes.Tan;
                            case "Player4":
                                return Brushes.LightGreen;
                            case "Player5":
                                return Brushes.PeachPuff;
                            case "Player6":
                                return Brushes.Plum;
                            case "Player7":
                                return Brushes.PaleTurquoise;
                            case "Player8":
                                return Brushes.Pink;
                        }
                    }
                }
            }
            return Brushes.White;
        }

        public static void drawZone(Zone zone, ZoneNode zoneNode, Graphics g)
        {
            // 正方形参数
            int x = (int)(zoneNode.X - 50);
            int y = (int)(zoneNode.Y - 50);
            int size = 100;

            // 绘制正方形
            g.FillRectangle(ZoneBrush(zone), x, y, size, size);
            g.DrawRectangle(Pens.Black, x, y, size, size);

            // name
            Font font = SystemFonts.DefaultFont;
            g.DrawString(zone.Name, font, Brushes.Black, x + 5, y + 5);

            // city
            int city = 0;
            if (zone.MainObjects != null)
            {
                foreach (var mainObject in zone.MainObjects)
                {
                    if (mainObject.Type == "Spawn" || mainObject.Type == "City")
                    {
                        city++;
                    }
                }
            }

            g.DrawString("City : " + city, font, Brushes.Black, x + 5, y + 20);
            g.DrawString("GuardMulti : " + zone.GuardMultiplier, font, Brushes.Black, x + 5, y + 35);
            g.DrawString("Resource : " + (zone.GuardedContentValue + zone.UnguardedContentValue + zone.ResourcesValue), font, Brushes.Black, x + 5, y + 50);
        }


    }

}