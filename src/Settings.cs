using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Data;
using MBOptionScreen.Settings;

namespace LightProsperity
{
    public class Settings : AttributeSettings<Settings>
    {
        public override string Id { get; set; } = "Light.Prosperity_v1";
        public override string ModName => "Light Prosperity";
        public override string ModuleFolderName => "LightProsperity";


        [SettingPropertyDropdown(displayName: "Bonus Slots For", Order = 0, RequireRestart = false, HintText = "Who can receive extra recruitment slots from notables.")]
        [SettingPropertyGroup(groupName: "General Settings", order: 0)]
        public DefaultDropdown<string> BonusSlotsFor { get; set; } = new DefaultDropdown<string>(new string[]
        {
            "Everyone",
            "Not at war",
            "Same faction",
            "Same clan"
        }, 2);

        [SettingPropertyFloatingInteger(displayName: "Prisoner Prosperity Value", minValue: 0f, maxValue: 10f, Order = 1, RequireRestart = false, HintText = "Prosperity increase for selling one prisoner.")]
        [SettingPropertyGroup(groupName: "General Settings")]
        public float PrisonerProsperityValue { get; set; } = 1.0f;

        [SettingPropertyFloatingInteger(displayName: "Militia Growth Bonus", minValue: 0f, maxValue: 10f, Order = 2, RequireRestart = true, HintText = "Number of extra militia every day.")]
        [SettingPropertyGroup(groupName: "General Settings")]
        public float MilitiaGrowthBonus { get; set; } = 0;

        [SettingPropertyFloatingInteger(displayName: "Prosperity Growth Multiplier", minValue: 0f, maxValue: 20f, Order = 3, RequireRestart = true, HintText = "The multiplier to overall prosperity and hearth growth rate.")]
        [SettingPropertyGroup(groupName: "General Settings")]
        public float ProsperityGrowthMultiplier { get; set; } = 1.0f;


        [SettingPropertyBool(displayName: "Garrison Settings", Order = 0, RequireRestart = true, HintText = "Modify food consumption and wages of garrison.")]
        [SettingPropertyGroup(groupName: "Garrison Settings", order: 1, isMainToggle: true)]
        public bool ModifyGarrisonConsumption { get; set; } = true;

        [SettingPropertyFloatingInteger(displayName: "Garrison Wages Multiplier", minValue: 0f, maxValue: 1f, Order = 1, RequireRestart = false, HintText = "Multiplier for garrison wages.")]
        [SettingPropertyGroup(groupName: "Garrison Settings")]
        public float GarrisonWagesMultiplier { get; set; } = 0.5f;

        [SettingPropertyFloatingInteger(displayName: "Garrison Food Consumption Multiplier", minValue: 0f, maxValue: 1f, Order = 2, RequireRestart = false, HintText = "Multplier for garrison food consumption, and how many troops at garrison are lost because of food shortage.")]
        [SettingPropertyGroup(groupName: "Garrison Settings")]
        public float GarrisonFoodConsumpetionMultiplier { get; set; } = 0.0f;


        [SettingPropertyBool(displayName: "New Prosperity Model", Order = 0, RequireRestart = true, HintText = "Enable the new prosperity model. The new model gives settlements a natural new born growth rate and is affected by settlement capacity, trade, food storage and enemies around.")]
        [SettingPropertyGroup(groupName: "New Prosperity Model", order: 2, isMainToggle: true)]
        public bool NewProsperityModel { get; set; } = false;

        [SettingPropertyInteger(displayName: "Village Natural Growth Capacity", minValue: 0, maxValue: 3000, Order = 1, RequireRestart = false, HintText = "Natural growth capacity for village. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "New Prosperity Model")]
        public int VillageGrowthCap { get; set; } = 800;

        [SettingPropertyInteger(displayName: "Town Natural Growth Capacity", minValue: 0, maxValue: 20000, Order = 2, RequireRestart = false, HintText = "Natural growth capacity for towns. Trade, buildings and other bonus can boost prosperity beyond natural capacity. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "New Prosperity Model")]
        public int TownGrowthCap { get; set; } = 7000;

        [SettingPropertyInteger(displayName: "Castle Natural Growth Capacity", minValue: 0, maxValue: 5000, Order = 3, RequireRestart = false, HintText = "Natural growth capacity for castles. Buildings and other bonus can boost prosperity beyond natural capacity. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "New Prosperity Model")]
        public int CastleGrowthCap { get; set; } = 1500;


        [SettingPropertyInteger(displayName: "Town Minimum Prosperity for Recruit", minValue: 0, maxValue: 5000, Order = 0, RequireRestart = false, HintText = "If prosperity is below this value, a town will stop generating new recruits.")]
        [SettingPropertyGroup(groupName: "Town Settings", order: 3)]
        public int TownMinProsperityForRecruit { get; set; } = 1000;

        [SettingPropertyInteger(displayName: "Town Prosperity Threshold", minValue: 0, maxValue: 10000, Order = 1, RequireRestart = false, HintText = "The required prosperity for a town to generate more recruits.")]
        [SettingPropertyGroup(groupName: "Town Settings")]
        public int TownProsperityThreshold { get; set; } = 3000;

        [SettingPropertyInteger(displayName: "Town Prosperity Per Bonus Slot", minValue: 0, maxValue: 10000, Order = 2, RequireRestart = false, HintText = "Every how much prosperity past the threshold gives one extra recruitment slot.")]
        [SettingPropertyGroup(groupName: "Town Settings")]
        public int TownProsperityPerBonusSlot { get; set; } = 2000;

        [SettingPropertyFloatingInteger(displayName: "Town Recruit Prosperity Cost", minValue: 0f, maxValue: 10f, Order = 3, RequireRestart = false, HintText = "The prosperity cost for one recuit.")]
        [SettingPropertyGroup(groupName: "Town Settings")]
        public float TownRecruitProsperityCost { get; set; } = 2.0f;


        [SettingPropertyInteger(displayName: "Village Minimum Hearth for Recruit", minValue: 0, maxValue: 500, Order = 0, RequireRestart = false, HintText = "If hearth is below this value, a village will stop generating new recruits.")]
        [SettingPropertyGroup(groupName: "Village Settings", order: 4)]
        public int VillageMinProsperityForRecruit { get; set; } = 100;

        [SettingPropertyInteger(displayName: "Village Hearth Threshold", minValue: 0, maxValue: 1000, Order = 1, RequireRestart = false, HintText = "The required hearth for a village to generate more recruits.")]
        [SettingPropertyGroup(groupName: "Village Settings")]
        public int VillageProsperityThreshold { get; set; } = 300;

        [SettingPropertyInteger(displayName: "Village Hearth Per Bonus Slot", minValue: 0, maxValue: 1000, Order = 2, RequireRestart = false, HintText = "Every how much hearth past the threshold gives one extra recruitment slot.")]
        [SettingPropertyGroup(groupName: "Village Settings")]
        public int VillageProsperityPerBonusSlot { get; set; } = 200;

        [SettingPropertyFloatingInteger(displayName: "Village Recruit Hearth Cost", minValue: 0f, maxValue: 10f, Order = 3, RequireRestart = false, HintText = "The hearth cost for one recuit.")]
        [SettingPropertyGroup(groupName: "Village Settings")]
        public float VillageRecruitProsperityCost { get; set; } = 1.0f;


        [SettingPropertyInteger(displayName: "Castle Minimum Prosperity for Recruit", minValue: 0, maxValue: 2000, Order = 0, RequireRestart = false, HintText = "If prosperity is below this value, a castle will stop generating new noble recruits.")]
        [SettingPropertyGroup(groupName: "Castle Settings", order: 5)]
        public int CastleMinProsperityForRecruit { get; set; } = 500;

        [SettingPropertyInteger(displayName: "Castle Prosperity Threshold", minValue: 0, maxValue: 20000, Order = 1, RequireRestart = false, HintText = "Controls the chance of castles getting noble recruits. If prosperity reaches the threshold, the castle is guaranteed to get one noble recruit every day.")]
        [SettingPropertyGroup(groupName: "Castle Settings")]
        public int CastleProsperityThreshold { get; set; } = 7500;

        [SettingPropertyFloatingInteger(displayName: "Castle Recruit Prosperity Cost", minValue: 0f, maxValue: 10f, Order = 2, RequireRestart = false, HintText = "The prosperity cost for one recuit.")]
        [SettingPropertyGroup(groupName: "Castle Settings")]
        public float CastleRecruitProsperityCost { get; set; } = 2.0f;


        [SettingPropertyInteger(displayName: "Notable Power Threshold For Noble Recruit", minValue: 0, maxValue: 1000, Order = 0, RequireRestart = false, HintText = "Controls the chance of rural notables getting noble recruits. If power reaches the threshold, the notable is guaranteed to get notable recruits at every chance.")]
        [SettingPropertyGroup(groupName: "Notable Settings", order: 6)]
        public int NotablePowerThresholdForNobleRecruit { get; set; } = 600;

        [SettingPropertyFloatingInteger(displayName: "Notable Noble Recruit Power Cost", minValue: 0f, maxValue: 10f, Order = 1, RequireRestart = false, HintText = "The power cost for one noble recuit.")]
        [SettingPropertyGroup(groupName: "Notable Settings")]
        public float NotableNobleRecruitPowerCost { get; set; } = 1.0f;
    }
}
