using System.Collections.Generic;

namespace Serenno.API.SwgohGG.Models
{
    public class GearIngredient
    {
        public int amount { get; set; }
        public string gear { get; set; }
    }

    public class GearRecipe
    {
        public string base_id { get; set; }
        public string result_id { get; set; }
        public int cost { get; set; }
        public List<GearIngredient> ingredients { get; set; }
    }

    public class GearStats
    {
        public double _3 { get; set; }
        public double _4 { get; set; }
        public double _6 { get; set; }
        public double? _17 { get; set; }
        public double? _2 { get; set; }
        public double? _10 { get; set; }
        public double? _1 { get; set; }
        public double? _7 { get; set; }
        public double? _18 { get; set; }
        public double? _14 { get; set; }
        public double? _9 { get; set; }
        public double? _5 { get; set; }
        public double? _27 { get; set; }
        public double? _11 { get; set; }
        public double? _15 { get; set; }
        public double? _8 { get; set; }
        public double? _28 { get; set; }
    }

    public class Gear
    {
        public string base_id { get; set; }
        public List<GearRecipe> recipes { get; set; }
        public int tier { get; set; }
        public int required_level { get; set; }
        public GearStats stats { get; set; }
        public string mark { get; set; }
        public int cost { get; set; }
        public string image { get; set; }
        public string url { get; set; }
        public List<GearIngredient> ingredients { get; set; }
        public string name { get; set; }
    }
}