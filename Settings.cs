using Newtonsoft.Json;

namespace LightProsperity
{
    public class Settings
    {
        [JsonProperty("townRecruitProsperityCost")]
        public float townRecruitProsperityCost { get; set; }

        [JsonProperty("villageRecruitProsperityCost")]
        public float villageRecruitProsperityCost { get; set; }

        [JsonProperty("notableNobleRecruitPowerCost")]
        public int notableNobleRecruitPowerCost { get; set; }

        [JsonProperty("townMinProsperityForRecruit")]
        public float townMinProsperityForRecruit { get; set; }

        [JsonProperty("villageMinProsperityForRecruit")]
        public float villageMinProsperityForRecruit { get; set; }

        [JsonProperty("townProsperityThreshould")]
        public float townProsperityThreshould { get; set; }

        [JsonProperty("townProsperityPerBonusSlot")]
        public float townProsperityPerBonusSlot { get; set; }

        [JsonProperty("villageProsperityThreshould")]
        public float villageProsperityThreshould { get; set; }

        [JsonProperty("villageProsperityPerBonusSlot")]
        public float villageProsperityPerBonusSlot { get; set; }
    }
}
