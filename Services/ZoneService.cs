using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Services
{
    public class ZoneService
    {
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
                GuardedContentValuePerArea = 20000,
                UnguardedContentValue = dto.UnguardedContentValue ?? 10000,
                UnguardedContentValuePerArea = 0,
                ResourcesValue = dto.ResourcesValue ?? 0,
                ResourcesValuePerArea = 0,
                MainObjects = new List<MainObject>()
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

        }
    }

}
