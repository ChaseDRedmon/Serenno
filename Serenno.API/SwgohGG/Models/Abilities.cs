namespace Serenno.API.SwgohGG.Models
{
    public class Abilities
    {
        public string base_id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string url { get; set; }
        public int tier_max { get; set; }
        public bool is_zeta { get; set; }
        public bool is_omega { get; set; }
        public int combat_type { get; set; }
        public int? type { get; set; }
        public string character_base_id { get; set; }
        public string ship_base_id { get; set; }
    }
}