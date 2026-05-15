using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views.Dialog;

namespace OldenEraTemplateEditor.Services
{
    public class ContentService
    {
        public static void AddMandatoryContentGroup(Rmg rmg, MandatoryContentDto dto)
        {
            var group = new MandatoryContentGroup
            {
                Name = dto.GroupName,
                Content = new List<ContentItem>()
            };
            rmg.rmgTemplate.MandatoryContent ??= new();
            rmg.rmgTemplate.MandatoryContent.Add(group);
        }

        public static void AddMandatoryContentItem(Rmg rmg, MandatoryContentDto dto)
        {
            var group = rmg.rmgTemplate.MandatoryContent?.Find(g => g.Name == dto.GroupName);
            if (group == null) return;

            group.Content ??= new List<ContentItem>();
            var item = new ContentItem
            {
                Sid = dto.Sid,
                IncludeLists = dto.IncludeLists,
                IsGuarded = dto.IsGuarded
            };
            if (item.Sid != null)
            {
                if (Constant.MineSids.Contains(item.Sid))
                {
                    item.IsMine = true;
                }
            }
            else if (item.IncludeLists != null)
            {
                foreach (var IncludeList in item.IncludeLists)
                {
                    if (Constant.MineContentLists.Contains(IncludeList))
                    {
                        item.IsMine = true;
                    }
                }
            }
            group.Content.Add(item);
        }

        public static bool DeleteMandatoryContentGroup(Rmg rmg, MandatoryContentGroup group)
        {
            foreach (var variant in rmg.rmgTemplate.Variants)
            {
                foreach (var zone in variant.Zones)
                {
                    if (zone.MandatoryContent != null && zone.MandatoryContent.Contains(group.Name))
                    {
                        return false;
                    }
                }
            }
            rmg.rmgTemplate.MandatoryContent?.Remove(group);
            return true;
        }

        public static void DeleteMandatoryContentItem(MandatoryContentGroup group, ContentItem item)
        {
            group.Content?.Remove(item);
        }

        public static void AddContentCountLimit(Rmg rmg, ContentCountLimitGroupDto dto)
        {
            var limit = new ContentCountLimit
            {
                Name = dto.name,
                Limits = new List<ContentSidLimit>()
            };
            rmg.rmgTemplate.ContentCountLimits ??= new();
            rmg.rmgTemplate.ContentCountLimits.Add(limit);
        }

        public static void AddContentCountLimitItem(ContentCountLimit limit, ContentCountLimitItemDto dto)
        {
            limit.Limits ??= new List<ContentSidLimit>();
            limit.Limits.Add(new ContentSidLimit
            {
                Sid = dto.sid,
                MaxCount = dto.maxCount
            });
        }

        public static bool DeleteContentCountLimit(Rmg rmg, ContentCountLimit limit)
        {
            foreach (var variant in rmg.rmgTemplate.Variants)
            {
                foreach (var zone in variant.Zones)
                {
                    if (zone.ContentCountLimits != null && zone.ContentCountLimits.Contains(limit.Name))
                    {
                        return false;
                    }
                }
            }
            rmg.rmgTemplate.ContentCountLimits?.Remove(limit);
            return true;
        }

        public static void DeleteContentCountLimitItem(ContentCountLimit limit, ContentSidLimit item)
        {
            limit.Limits?.Remove(item);
        }

        public static void AddZoneLayout(Rmg rmg, string name)
        {
            var layout = new ZoneLayout
            {
                Name = name,
                ObstaclesFill = 0.58,
                ObstaclesFillVoid = 0.58,
                LakesFill = 0.3,
                MinLakeArea = 10,
                ElevationClusterScale = 0.128,
                ElevationModes = new List<ElevationMode>
                {
                    new() { Weight = 1, MinElevatedFraction = 0.0, MaxElevatedFraction = 0.2 },
                    new() { Weight = 1, MinElevatedFraction = 0.7, MaxElevatedFraction = 0.8 }
                },
                RoadClusterArea = 128,
                GuardedEncounterResourceFractions = new GuardedEncounterResourceFractions
                {
                    CountBounds = new List<double>(),
                    Fractions = new List<double> { 0.66 }
                },
                AmbientPickupDistribution = new AmbientPickupDistribution
                {
                    Repulsion = 1.0,
                    Noise = 0.3,
                    RoadAttraction = 0.25,
                    ObstacleAttraction = 0.0,
                    GroupSizeWeights = new List<int> { 4, 1, 1 }
                }
            };
            rmg.rmgTemplate.ZoneLayouts ??= new();
            rmg.rmgTemplate.ZoneLayouts.Add(layout);
        }

        public static bool DeleteZoneLayout(Rmg rmg, ZoneLayout layout)
        {
            foreach (var variant in rmg.rmgTemplate.Variants)
            {
                foreach (var zone in variant.Zones)
                {
                    if (zone.Layout == layout.Name)
                    {
                        return false;
                    }
                }
            }
            rmg.rmgTemplate.ZoneLayouts?.Remove(layout);
            return true;
        }
    }
}
