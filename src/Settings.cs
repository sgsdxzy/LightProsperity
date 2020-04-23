using Newtonsoft.Json;

namespace LightProsperity
{
    public class Settings
    {
        [JsonProperty("townRecruitProsperityCost")]
        public float townRecruitProsperityCost { get; set; }

        [JsonProperty("villageRecruitProsperityCost")]
        public float villageRecruitProsperityCost { get; set; }

        [JsonProperty("castleRecruitProsperityCost")]
        public float castleRecruitProsperityCost { get; set; }

        [JsonProperty("townMinProsperityForRecruit")]
        public float townMinProsperityForRecruit { get; set; }

        [JsonProperty("villageMinProsperityForRecruit")]
        public float villageMinProsperityForRecruit { get; set; }

        [JsonProperty("castleMinProsperityForRecruit")]
        public float castleMinProsperityForRecruit { get; set; }

        [JsonProperty("notableNobleRecruitPowerCost")]
        public float notableNobleRecruitPowerCost { get; set; }

        [JsonProperty("notablePowerThreshouldForNobleRecruit")]
        public float notablePowerThreshouldForNobleRecruit { get; set; }

        [JsonProperty("townProsperityThreshould")]
        public float townProsperityThreshould { get; set; }

        [JsonProperty("townProsperityPerBonusSlot")]
        public float townProsperityPerBonusSlot { get; set; }

        [JsonProperty("villageProsperityThreshould")]
        public float villageProsperityThreshould { get; set; }

        [JsonProperty("villageProsperityPerBonusSlot")]
        public float villageProsperityPerBonusSlot { get; set; }

        [JsonProperty("castleProsperityThreshould")]
        public float castleProsperityThreshould { get; set; }

        [JsonProperty("bonusSlotsFor")]
        public int bonusSlotsFor { get; set; }
    }
}
