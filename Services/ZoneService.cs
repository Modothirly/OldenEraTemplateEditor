using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;
using OldenEraTemplateEditor.Views.LayoutEngine;
using Orientation = OldenEraTemplateEditor.Models.Orientation;

namespace OldenEraTemplateEditor.Services
{
    public class ZoneService
    {
        public static int AddVariant(Rmg rmg)
        {
            var variant = new Variant
            {
                Zones = new List<Zone>(),
                Connections = new List<Connection>(),
                Orientation = new Orientation
                {
                    Mode = "MinimalBoundingSquare"
                },
                Border = new Border
                {
                    CornerRadius = 0.8,
                    ObstaclesWidth = 3,
                    ObstaclesNoise = [
                        new NoiseEntry
                        {
                            Amp = 0.5,
                            Freq = 6
                        }
                    ],
                    WaterWidth = 3,
                    WaterNoise = [
                        new NoiseEntry
                        {
                            Amp = 1,
                            Freq = 12
                        }
                    ],
                    WaterType = "water grass"
                }
            };
            rmg.rmgTemplate.Variants.Add(variant);

            var variantModel = new VariantModel();
            variantModel.RebuildCanvasData(variant);
            rmg.variantList.Add(variantModel);
            return rmg.variantList.Count - 1;
        }

        public static void DeleteVariant(Rmg rmg, int variantIndex)
        {
            rmg.rmgTemplate.Variants.RemoveAt(variantIndex);
            rmg.variantList.RemoveAt(variantIndex);
        }

        public static void AddZone(Rmg rmg, ZoneFormDto dto, int variantIndex, int x, int y)
        {
            var variant = rmg.rmgTemplate.Variants[variantIndex];
            var variantModel = rmg.variantList[variantIndex];

            var zone = new Zone
            {
                Name = dto.name,
                Size = 1.0,
                GuardCutoffValue = 1500,
                GuardRandomization = 0.05,
                GuardMultiplier = dto.GuardMultiplier ?? 1.0,
                GuardWeeklyIncrement = 0.05,
                GuardReactionDistribution = new List<int> { 4, 1, 1, 1, 1, 0 },
                GuardedContentValue = dto.GuardedContentValue ?? 500000,
                GuardedContentValuePerArea = dto.GuardedContentValuePerArea ?? 2000,
                UnguardedContentValue = dto.UnguardedContentValue ?? 10000,
                UnguardedContentValuePerArea = dto.UnguardedContentValuePerArea ?? 0,
                ResourcesValue = dto.ResourcesValue ?? 0,
                ResourcesValuePerArea = 0,
                MainObjects = new List<MainObject>(),
                MandatoryContent = new()
            };

            variant.Zones.Add(zone);

            variantModel.ZoneNodeDict[zone.Name] = new ZoneNode
            {
                Name = zone.Name,
                Size = (float)(zone.Size ?? 1.0),
                X = x,
                Y = y
            };

            bool NeutralZone = string.IsNullOrEmpty(dto.spawn) && "Neutral" == dto.spawn;

            if (!NeutralZone)
            {
                zone.MainObjects.Add(new MainObject
                {
                    Type = "Spawn",
                    Spawn = dto.spawn,
                    GuardChance = 0.5,
                    GuardValue = 3000,
                    GuardWeeklyIncrement = 0.05,
                    RemoveGuardIfHasOwner = true,
                    BuildingsConstructionSid = "default_buildings_construction",
                    Placement = "Uniform"
                });

            }

            // City
            for (int i = 0; i < dto.cityAmount; i++)
            {
                var city = new MainObject
                {
                    Type = "City",
                    GuardChance = 0.5,
                    GuardValue = 3000,
                    GuardWeeklyIncrement = 0.05,
                    BuildingsConstructionSid = "default_buildings_construction",
                    Faction = new TypedSelector(),
                    Placement = "Uniform"
                };
                if (!NeutralZone)
                {
                    city.Faction = new TypedSelector
                    {
                        Type = "Match",
                        Args = ["0"]
                    };
                }
                zone.MainObjects.Add(city);
            }

            var BiomeSelector = new BiomeSelector
            {
                Type = "FromList",
                Args = []
            };
            if (zone.MainObjects.Count > 0)
            {
                BiomeSelector = new BiomeSelector
                {
                    Type = "MatchMainObject",
                    Args = ["0"]
                };
            }
            zone.ZoneBiome = BiomeSelector;
            zone.ContentBiome = BiomeSelector;
            zone.MetaObjectsBiome = BiomeSelector;

        }
        public static void AddConnection(Rmg rmg, ConnectionFormDto ConnectionFormDto, int variantIndex)
        {
            var variant = rmg.rmgTemplate.Variants[variantIndex];
            var connection = new Connection
            {
                From = ConnectionFormDto.From,
                To = ConnectionFormDto.To,
                ConnectionType = ConnectionFormDto.ConnectionType ?? "Default",
                GuardEscape = false,
                GuardValue = ConnectionFormDto.GuardValue,
                GuardWeeklyIncrement = 0.05,

            };
            if (ConnectionFormDto.ConnectionType == "Proximity")
            {
                connection.Length = 6;
            }


            int index = 1;
            while (true)
            {
                string name = ConnectionFormDto.From + "-" + ConnectionFormDto.To + "-" + index;
                bool find = false;
                foreach (var item in variant.Connections)
                {
                    if (item.Name == name)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    connection.Name = name;
                    break;
                }
                index++;
            }
            variant.Connections.Add(connection);
        }

        public static void DeleteZone(Rmg rmg, string zoneName, int variantIndex)
        {
            var variant = rmg.rmgTemplate.Variants[variantIndex];
            var variantModel = rmg.variantList[variantIndex];

            // 1. 删除所有引用该 Zone 的 Connection（同时清理相关 road）
            var connectionsToRemove = variant.Connections
                .Where(c => c.From == zoneName || c.To == zoneName)
                .ToList();
            foreach (var conn in connectionsToRemove)
            {
                DeleteConnection(rmg, conn.Name, variantIndex);
            }

            // 2. 从 Zones 列表移除
            variant.Zones.RemoveAll(z => z.Name == zoneName);

            // 3. 从 ZoneNodeDict 移除
            variantModel.ZoneNodeDict.Remove(zoneName);
        }

        public static void DeleteConnection(Rmg rmg, string connectionName, int variantIndex)
        {
            var variant = rmg.rmgTemplate.Variants[variantIndex];

            variant.Connections.RemoveAll(c => c.Name == connectionName);

            // 清理所有 Zone 里引用了该 Connection 的 road
            foreach (var zone in variant.Zones)
            {
                if (zone.Roads != null)
                {
                    zone.Roads.RemoveAll(r =>
                        (r.From?.Type == "Connection" && r.From.Args?.Contains(connectionName) == true) ||
                        (r.To?.Type == "Connection" && r.To.Args?.Contains(connectionName) == true));
                }
            }
        }
    }

}
