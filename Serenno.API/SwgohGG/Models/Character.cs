using System.Collections.Generic;

namespace Serenno.API.SwgohGG.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class GearLevel
    {
        public int tier { get; set; }
        public List<string> gear { get; set; }
    }

    public class Character
    {
        public string name { get; set; }
        public string base_id { get; set; }
        public int pk { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public int power { get; set; }
        public string description { get; set; }
        public int combat_type { get; set; }
        public List<GearLevel> gear_levels { get; set; }
        public string alignment { get; set; }
        public List<string> categories { get; set; }
        public List<string> ability_classes { get; set; }
        public string role { get; set; }
        public string ship { get; set; }
        public int? ship_slot { get; set; }
        public int activate_shard_count { get; set; }
    }
}