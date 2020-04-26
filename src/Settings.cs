using Newtonsoft.Json;

namespace LightProsperity
{
    public class Settings
    {
        [JsonProperty("townMinProsperityForRecruit")]
        public float townMinProsperityForRecruit { get; set; }

        [JsonProperty("townProsperityThreshold")]
        public float townProsperityThreshold { get; set; }

        [JsonProperty("townProsperityPerBonusSlot")]
        public float townProsperityPerBonusSlot { get; set; }

        [JsonProperty("townRecruitProsperityCost")]
        public float townRecruitProsperityCost { get; set; }

        [JsonProperty("villageMinProsperityForRecruit")]
        public float villageMinProsperityForRecruit { get; set; }

        [JsonProperty("villageProsperityThreshold")]
        public float villageProsperityThreshold { get; set; }

        [JsonProperty("villageProsperityPerBonusSlot")]
        public float villageProsperityPerBonusSlot { get; set; }

        [JsonProperty("villageRecruitProsperityCost")]
        public float villageRecruitProsperityCost { get; set; }

        [JsonProperty("castleMinProsperityForRecruit")]
        public float castleMinProsperityForRecruit { get; set; }

        [JsonProperty("castleProsperityThreshold")]
        public float castleProsperityThreshold { get; set; }

        [JsonProperty("castleRecruitProsperityCost")]
        public float castleRecruitProsperityCost { get; set; }

        [JsonProperty("notablePowerThresholdForNobleRecruit")]
        public float notablePowerThresholdForNobleRecruit { get; set; }

        [JsonProperty("notableNobleRecruitPowerCost")]
        public float notableNobleRecruitPowerCost { get; set; }

        [JsonProperty("bonusSlotsFor")]
        public int bonusSlotsFor { get; set; }

        [JsonProperty("prisonerProsperityValue")]
        public float prisonerProsperityValue { get; set; }

        [JsonProperty("militiaGrowthBonus")]
        public float militiaGrowthBonus { get; set; }

        [JsonProperty("garrisonWagesMultiplier")]
        public float garrisonWagesMultiplier { get; set; }

        [JsonProperty("garrisonFoodConsumpetionMultiplier")]
        public float garrisonFoodConsumpetionMultiplier { get; set; }

        [JsonProperty("prosperityGrowthMultiplier")]
        public float prosperityGrowthMultiplier { get; set; }

        [JsonProperty("newProsperityModel")]
        public bool newProsperityModel { get; set; }

        [JsonProperty("villageGrowthCap")]
        public float villageGrowthCap { get; set; }

        [JsonProperty("townGrowthCap")]
        public float townGrowthCap { get; set; }

        [JsonProperty("castleGrowthCap")]
        public float castleGrowthCap { get; set; }
    }
}
