using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel;
using OldenEraTemplateEditor.Common;


namespace OldenEraTemplateEditor.Models
{
    public class MainObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("spawn")]
        public string? Spawn { get; set; }

        [JsonPropertyName("guardChance")]
        public double? GuardChance { get; set; }

        [JsonPropertyName("guardValue")]
        public int? GuardValue { get; set; }

        [JsonPropertyName("guardWeeklyIncrement")]
        public double? GuardWeeklyIncrement { get; set; }

        [JsonPropertyName("removeGuardIfHasOwner")]
        public bool? RemoveGuardIfHasOwner { get; set; }

        [JsonPropertyName("buildingsConstructionSid")]
        public string? BuildingsConstructionSid { get; set; }

        [TypeConverter(typeof(UniversalObjectConverter<TypedSelector>))]
        [JsonPropertyName("faction")]
        public TypedSelector? Faction { get; set; }

        [JsonPropertyName("placement")]
        public string? Placement { get; set; }

        [JsonPropertyName("placementArgs")]
        public List<string>? PlacementArgs { get; set; }

        [JsonPropertyName("holdCityWinCon")]
        public bool? HoldCityWinCon { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))] 
    public class TypedSelector
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("args")]
        public List<string>? Args { get; set; }
    }
}
