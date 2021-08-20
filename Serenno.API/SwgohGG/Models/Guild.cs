using System;
using System.Collections.Generic;

namespace Serenno.API.SwgohGG.Models
{
    public class Guild
    {
        public List<GuildMember> players { get; set; }
        public GuildMetadata data { get; set; }
    }

    public class GuildMetadata
    {
        public string name { get; set; }
        public int member_count { get; set; }
        public int galactic_power { get; set; }
        public int rank { get; set; }
        public int profile_count { get; set; }
        public int id { get; set; }
    }

    public class GuildMember
    {
        public List<MemberCharacters> units { get; set; }
        public MemberMetadata data { get; set; }
    }

    public class MemberMetadata
    {
        public object arena { get; set; }
        public object arena_rank { get; set; }
        public object arena_leader_base_id { get; set; }
        public DateTime last_updated { get; set; }
        public string name { get; set; }
        public int galactic_war_won { get; set; }
        public int ally_code { get; set; }
        public int galactic_power { get; set; }
        public int level { get; set; }
        public int pve_hard_won { get; set; }
        public int pve_battles_won { get; set; }
        public int character_galactic_power { get; set; }
        public object fleet_arena { get; set; }
        public int ship_galactic_power { get; set; }
        public int pvp_battles_won { get; set; }
        public int guild_exchange_donations { get; set; }
        public string url { get; set; }
        public int guild_contribution { get; set; }
        public int ship_battles_won { get; set; }
        public int guild_raid_won { get; set; }
    }

    public class MemberCharacters
    {
        public CharacterData data { get; set; }
    }

    public class CharacterData
    {
        public int relic_tier { get; set; }
        public int gear_level { get; set; }
        public List<CharacterGear> gear { get; set; }
        public int power { get; set; }
        public int level { get; set; }
        public string url { get; set; }
        public int combat_type { get; set; }
        public string[] mod_set_ids { get; set; }
        public int rarity { get; set; }
        public string base_id { get; set; }
        public CharacterStatistics stats { get; set; }
        public List<string> zeta_abilities { get; set; }
        public List<CharacterAbility> ability_data { get; set; }
        public string name { get; set; }
    }

    public class CharacterStatistics
    {
        public float _27 { get; set; }
        public float _28 { get; set; }
        public float _40 { get; set; }
        public float _1 { get; set; }
        public float _3 { get; set; }
        public float _2 { get; set; }
        public float _5 { get; set; }
        public float _4 { get; set; }
        public float _7 { get; set; }
        public float _6 { get; set; }
        public float _9 { get; set; }
        public float _8 { get; set; }
        public float _39 { get; set; }
        public float _12 { get; set; }
        public float _11 { get; set; }
        public float _10 { get; set; }
        public float _13 { get; set; }
        public float _38 { get; set; }
        public float _15 { get; set; }
        public float _14 { get; set; }
        public float _17 { get; set; }
        public float _16 { get; set; }
        public float _18 { get; set; }
        public float _37 { get; set; }
    }

    public class CharacterGear
    {
        public int slot { get; set; }
        public bool is_obtained { get; set; }
        public string base_id { get; set; }
    }

    public class CharacterAbility
    {
        public bool is_omega { get; set; }
        public bool is_zeta { get; set; }
        public string name { get; set; }
        public int ability_tier { get; set; }
        public string id { get; set; }
        public int tier_max { get; set; }
    }
}