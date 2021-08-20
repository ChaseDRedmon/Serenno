using System.Collections.Generic;

namespace Serenno.API.SwgohGG.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class GLRequiredUnit
    {
        public int gearLevel { get; set; }
        public int relicTier { get; set; }
        public string baseId { get; set; }
    }

    public class GLUnit
    {
        public string image { get; set; }
        public List<GLRequiredUnit> requiredUnits { get; set; }
        public string baseId { get; set; }
        public string unitName { get; set; }
    }

    public class GLChecklist
    {
        public List<GLUnit> units { get; set; }
    }
}