using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Views;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Models
{
    public class Rmg : IToolStripFileModel
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public RmgTemplate rmgTemplate = new();
        public Dictionary<string, ZoneNode> zoneNodeDict = new();
        public Dictionary<string, ZoneConnection> zoneConnectionDict = new();

        public void input(string json)
        {
            var s = JsonSerializer.Deserialize<RmgTemplate>(json);
            if (s is null) throw new InvalidDataException("File is empty or invalid.");
            rmgTemplate = s;
            RebuildCanvasData();
        }

        public string output()
        {
            return JsonSerializer.Serialize(rmgTemplate, JsonOptions);
        }

        /// <summary>
        /// 从 Unfrozen 模型重建画布数据（ZoneNode / ZoneConnection）
        /// </summary>
        public void RebuildCanvasData()
        {
            zoneNodeDict.Clear();
            zoneConnectionDict.Clear();

            var variant = rmgTemplate.Variants?.FirstOrDefault();
            if (variant == null) return;

            // 构建 ZoneNode
            if (variant.Zones != null)
            {
                foreach (var zone in variant.Zones)
                {
                    zoneNodeDict[zone.Name] = new ZoneNode
                    {
                        Name = zone.Name,
                        Size = (float)(zone.Size ?? 1.0)
                    };
                }
            }

            // 构建 ZoneConnection
            if (variant.Connections != null)
            {
                foreach (var conn in variant.Connections)
                {
                    string connName = conn.Name ?? $"{conn.From}_{conn.To}";
                    zoneConnectionDict[connName] = new ZoneConnection
                    {
                        Name = connName,
                        From = conn.From,
                        To = conn.To,
                        ConnectionType = conn.ConnectionType ?? "Direct",
                        GuardValue = conn.GuardValue ?? 0
                    };
                }
            }
        }
    }
}