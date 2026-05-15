using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldenEraTemplateEditor.Common
{
    public class Constant
    {
        public static readonly string[] Players =
        {
            "Player1",
            "Player2",
            "Player3",
            "Player4",
            "Player5",
            "Player6",
            "Player7",
            "Player8",
            "Neutral"
        };
        public static readonly string[] ConnectionType =
        {
            "Direct",
            "Portal",
            "Proximity",
            "Default",
            "GladiatorArena"
        };
        public static readonly string[] Sids =
        {
            "mine_gold", "mine_wood", "mine_ore", "mine_crystals", "mine_mercury", "mine_gemstones",
            "alchemy_lab", "watchtower", "mana_well", "fountain", "fountain_2",
            "market", "forge", "arena", "stables", "university",
            "beer_fountain", "quixs_path", "mysterious_stone", "pile_of_books",
            "crystal_trail", "tear_of_truth", "celestial_sphere", "sacrificial_shrine",
            "chimerologist", "wise_owl", "circus", "infernal_cirque",
            "tree_of_abundance", "fickle_shrine", "insaras_eye", "flattering_mirror", "wind_rose",
            "huntsmans_camp", "pandora_box", "resource_gold", "resource_wood", "resource_ore",
            "resource_crystals", "resource_mercury", "resource_gemstones",
            "random_item_common", "random_item_rare", "random_item_epic", "random_item_legendary",
        };
        public static readonly string[] MineSids =
        {
            "alchemy_lab","mine_gold", "mine_wood", "mine_ore", "mine_crystals", "mine_mercury", "mine_gemstones",
        };

        public static readonly string[] ContentLists =
        {
            "basic_content_list_basic_resources",
            "basic_content_list_rare_resources",
            "basic_content_list_special_resources",
            "basic_content_list_basic_mines",
            "basic_content_list_rare_mines",
            "basic_content_list_rare_mines_by_biome",
            "basic_content_list_special_mines",
            "basic_content_list_pickup_random_items",
            "basic_content_list_pickup_pandora_box",
            "basic_content_list_pickup_pandora_box_gold",
            "basic_content_list_pickup_pandora_box_exp",
            "basic_content_list_pickup_pandora_box_units",
            "basic_content_list_pickup_scroll_box",
            "basic_content_list_pickup_scroll_box_tier_1",
            "basic_content_list_pickup_scroll_box_tier_2",
            "basic_content_list_pickup_scroll_box_tier_3",
            "basic_content_list_pickup_scroll_box_tier_4",
            "basic_content_list_pickup_scroll_box_tier_5",
            "basic_content_list_basic_storage",
            "basic_content_list_building_hero_exp_tier_1",
            "basic_content_list_building_hero_exp_tier_2",
            "basic_content_list_building_hero_stats_and_skills_tier_1",
            "basic_content_list_building_hero_stats_and_skills_tier_2",
            "basic_content_list_building_hero_stats_and_skills_tier_3",
            "basic_content_list_building_resource_banks_tier_1",
            "basic_content_list_building_resource_banks_tier_2",
            "basic_content_list_building_guarded_resource_banks_tier_1",
            "basic_content_list_building_guarded_resource_banks_tier_2",
            "basic_content_list_building_guarded_resource_banks_tier_2_no_biome_restriction",
            "basic_content_list_building_guarded_resource_banks_tier_3",
            "basic_content_list_building_random_hires",
            "content_list_pickup_scroll_box_tier_1",
            "content_list_pickup_scroll_box_tier_2",
            "content_list_pickup_scroll_box_tier_3",
            "content_list_pickup_scroll_box_tier_4",
            "content_list_pickup_scroll_box_tier_5",
            "content_list_building_random_hires_low_tier",
            "basic_content_list_non_content",
            "basic_content_list_vision_buildings_tier_1",
            "basic_content_list_vision_buildings_tier_2",
            "basic_content_list_pickup_prison",
            "basic_content_list_pickup_enchanted_scroll_box",
            "basic_content_list_pickup_mythic_scroll_box",
            "basic_content_list_building_magic_tier_1",
            "basic_content_list_building_magic_tier_2",
            "basic_content_list_building_hero_buff_tier_1",
            "basic_content_list_building_uncommon_interact",
            "basic_content_list_building_epic_interact",
            "basic_content_list_building_guarded_units_banks",
            "basic_content_list_building_guarded_units_banks_no_biome_restriction",
            "basic_content_list_building_guarded_units_banks_only_biome_restriction",

        };
        public static readonly string[] MineContentLists =
        {
            "basic_content_list_basic_mines",
            "basic_content_list_rare_mines",
            "basic_content_list_rare_mines_by_biome",
            "basic_content_list_special_mines",
        };

        public static readonly string[] MapSizeOptions =
        {
            "S(64×64)",
            "M(80×80)",
            "M(96×96)",
            "L(112×112)",
            "L(128×128)",
            "XL(144×144)",
            "XL(160×160)",
            "H(176×176)",
            "H(192×192)",
            "G(208×208)",
            "G(256×256)",
            "C",
        };

        public static int MapSizeOptionToValue(string option)
        {
            if (option == "C") return 0;
            var start = option.IndexOf('(') + 1;
            var end = option.IndexOf('×');
            return int.Parse(option.Substring(start, end - start));
        }
    }
}