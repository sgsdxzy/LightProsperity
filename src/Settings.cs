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


        [SettingPropertyDropdown(displayName: "{=st3hNa}Bonus Slots For", Order = 0, RequireRestart = false, HintText = "{=FsrLvU}Who can receive extra recruitment slots from notables.")]
        [SettingPropertyGroup(groupName: "{=nY8kyK}General Settings", order: 0)]
        public DefaultDropdown<string> BonusSlotsFor { get; set; } = new DefaultDropdown<string>(new string[]
        {
            "Everyone",
            "Not at war",
            "Same faction",
            "Same clan"
        }, 2);

        [SettingPropertyFloatingInteger(displayName: "{=Dt7T5Q}Prisoner Prosperity Value", minValue: 0f, maxValue: 10f, Order = 1, RequireRestart = false, HintText = "{=FZTOih}Prosperity increase for selling one prisoner.")]
        [SettingPropertyGroup(groupName: "{=nY8kyK}General Settings")]
        public float PrisonerProsperityValue { get; set; } = 1.0f;

        [SettingPropertyFloatingInteger(displayName: "{=wDiy1T}Militia Growth Bonus", minValue: 0f, maxValue: 10f, Order = 2, RequireRestart = true, HintText = "{=qbKYdD}Number of extra militia every day.")]
        [SettingPropertyGroup(groupName: "{=nY8kyK}General Settings")]
        public float MilitiaGrowthBonus { get; set; } = 0;

        [SettingPropertyFloatingInteger(displayName: "{=r7PmHe}Prosperity Growth Multiplier", minValue: 0f, maxValue: 20f, Order = 3, RequireRestart = true, HintText = "{=WWmXeh}The multiplier to overall prosperity and hearth growth rate.")]
        [SettingPropertyGroup(groupName: "{=nY8kyK}General Settings")]
        public float ProsperityGrowthMultiplier { get; set; } = 1.0f;


        [SettingPropertyBool(displayName: "{=2w9Irb}Garrison Settings", Order = 0, RequireRestart = true, HintText = "{=iYJ2Aw}Modify food consumption and wages of garrison.")]
        [SettingPropertyGroup(groupName: "{=2w9Irb}Garrison Settings", order: 1, IsMainToggle = true)]
        public bool ModifyGarrisonConsumption { get; set; } = true;

        [SettingPropertyFloatingInteger(displayName: "{=bYdOtk}Garrison Wages Multiplier", minValue: 0f, maxValue: 1f, Order = 1, RequireRestart = false, HintText = "{=zAi4b4}Multiplier for garrison wages.")]
        [SettingPropertyGroup(groupName: "{=2w9Irb}Garrison Settings")]
        public float GarrisonWagesMultiplier { get; set; } = 0.5f;

        [SettingPropertyFloatingInteger(displayName: "{=XFGXdV}Garrison Food Consumption Multiplier", minValue: 0f, maxValue: 1f, Order = 2, RequireRestart = false, HintText = "{=S75wJJ}Multplier for garrison food consumption, and how many troops at garrison are lost because of food shortage.")]
        [SettingPropertyGroup(groupName: "{=2w9Irb}Garrison Settings")]
        public float GarrisonFoodConsumpetionMultiplier { get; set; } = 0.0f;


        [SettingPropertyBool(displayName: "{=NDmBnT}New Prosperity Model", Order = 0, RequireRestart = true, HintText = "{=cboszz}Enable the new prosperity model. The new model gives settlements a natural new born growth rate and is affected by settlement capacity, trade, food storage and enemies around.")]
        [SettingPropertyGroup(groupName: "{=NDmBnT}New Prosperity Model", order: 2, IsMainToggle = true)]
        public bool NewProsperityModel { get; set; } = false;

        [SettingPropertyInteger(displayName: "{=1DP3ee}Village Natural Growth Capacity", minValue: 0, maxValue: 3000, Order = 1, RequireRestart = false, HintText = "{=udES1n}Natural growth capacity for village. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "{=NDmBnT}New Prosperity Model")]
        public int VillageGrowthCap { get; set; } = 800;

        [SettingPropertyInteger(displayName: "{=8uSaMW}Town Natural Growth Capacity", minValue: 0, maxValue: 20000, Order = 2, RequireRestart = false, HintText = "{=taQ8PZ}Natural growth capacity for towns. Trade, buildings and other bonus can boost prosperity beyond natural capacity. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "{=NDmBnT}New Prosperity Model")]
        public int TownGrowthCap { get; set; } = 7000;

        [SettingPropertyInteger(displayName: "{=hkVfKG}Castle Natural Growth Capacity", minValue: 0, maxValue: 5000, Order = 3, RequireRestart = false, HintText = "{=hjvM5R}Natural growth capacity for castles. Buildings and other bonus can boost prosperity beyond natural capacity. 0 for unlimited.")]
        [SettingPropertyGroup(groupName: "{=NDmBnT}New Prosperity Model")]
        public int CastleGrowthCap { get; set; } = 1500;


        [SettingPropertyInteger(displayName: "{=vmWeg5}Town Minimum Prosperity for Recruit", minValue: 0, maxValue: 5000, Order = 0, RequireRestart = false, HintText = "{=5lAFED}If prosperity is below this value, a town will stop generating new recruits.")]
        [SettingPropertyGroup(groupName: "{=rRxcGb}Town Settings", order: 3)]
        public int TownMinProsperityForRecruit { get; set; } = 1000;

        [SettingPropertyInteger(displayName: "{=zfnNmH}Town Prosperity Threshold", minValue: 0, maxValue: 10000, Order = 1, RequireRestart = false, HintText = "{=DCIx2W}The required prosperity for a town to generate more recruits.")]
        [SettingPropertyGroup(groupName: "{=rRxcGb}Town Settings")]
        public int TownProsperityThreshold { get; set; } = 3000;

        [SettingPropertyInteger(displayName: "{=SSS3Uk}Town Prosperity Per Bonus Slot", minValue: 0, maxValue: 10000, Order = 2, RequireRestart = false, HintText = "{=hdv7FT}Amount of prosperity past the threshold required for one extra recruitment slot.")]
        [SettingPropertyGroup(groupName: "{=rRxcGb}Town Settings")]
        public int TownProsperityPerBonusSlot { get; set; } = 2000;

        [SettingPropertyFloatingInteger(displayName: "{=VvQ1N7}Town Recruit Prosperity Cost", minValue: 0f, maxValue: 10f, Order = 3, RequireRestart = false, HintText = "{=Fua09c}The prosperity cost for one recuit.")]
        [SettingPropertyGroup(groupName: "{=rRxcGb}Town Settings")]
        public float TownRecruitProsperityCost { get; set; } = 2.0f;


        [SettingPropertyInteger(displayName: "{=f0PBIT}Village Minimum Hearth for Recruit", minValue: 0, maxValue: 500, Order = 0, RequireRestart = false, HintText = "{=6J40z3}If hearth is below this value, a village will stop generating new recruits.")]
        [SettingPropertyGroup(groupName: "{=Sbl08S}Village Settings", order: 4)]
        public int VillageMinProsperityForRecruit { get; set; } = 100;

        [SettingPropertyInteger(displayName: "{=5gY7RW}Village Hearth Threshold", minValue: 0, maxValue: 1000, Order = 1, RequireRestart = false, HintText = "{=XRf31f}The required hearth for a village to generate more recruits.")]
        [SettingPropertyGroup(groupName: "{=Sbl08S}Village Settings")]
        public int VillageProsperityThreshold { get; set; } = 300;

        [SettingPropertyInteger(displayName: "{=TdEny1}Village Hearth Per Bonus Slot", minValue: 0, maxValue: 1000, Order = 2, RequireRestart = false, HintText = "{=Pa373a}Amount of hearth past the threshold required for one extra recruitment slot.")]
        [SettingPropertyGroup(groupName: "{=Sbl08S}Village Settings")]
        public int VillageProsperityPerBonusSlot { get; set; } = 200;

        [SettingPropertyFloatingInteger(displayName: "{=zkkSzM}Village Recruit Hearth Cost", minValue: 0f, maxValue: 10f, Order = 3, RequireRestart = false, HintText = "{=GnGQnu}The hearth cost for one recuit.")]
        [SettingPropertyGroup(groupName: "{=Sbl08S}Village Settings")]
        public float VillageRecruitProsperityCost { get; set; } = 1.0f;


        [SettingPropertyInteger(displayName: "{=0A9Xwm}Castle Minimum Prosperity for Recruit", minValue: 0, maxValue: 2000, Order = 0, RequireRestart = false, HintText = "{=QH4Z4u}If prosperity is below this value, a castle will stop generating new noble recruits.")]
        [SettingPropertyGroup(groupName: "{=DACgrG}Castle Settings", order: 5)]
        public int CastleMinProsperityForRecruit { get; set; } = 500;

        [SettingPropertyInteger(displayName: "{=2Qgisr}Castle Prosperity Threshold", minValue: 0, maxValue: 20000, Order = 1, RequireRestart = false, HintText = "{=5mN2sb}Controls the chance of castles getting noble recruits. If prosperity reaches the threshold, the castle is guaranteed to get one noble recruit every day.")]
        [SettingPropertyGroup(groupName: "{=DACgrG}Castle Settings")]
        public int CastleProsperityThreshold { get; set; } = 7500;

        [SettingPropertyFloatingInteger(displayName: "{=3eTGOO}Castle Recruit Prosperity Cost", minValue: 0f, maxValue: 10f, Order = 2, RequireRestart = false, HintText = "{=Fua09c}The prosperity cost for one recuit.")]
        [SettingPropertyGroup(groupName: "{=DACgrG}Castle Settings")]
        public float CastleRecruitProsperityCost { get; set; } = 2.0f;


        [SettingPropertyInteger(displayName: "{=izsqza}Notable Power Threshold For Noble Recruit", minValue: 0, maxValue: 1000, Order = 0, RequireRestart = false, HintText = "{=bh2GwT}Controls the chance of rural notables getting noble recruits. If power reaches the threshold, the notable is guaranteed to get notable recruits at every chance.")]
        [SettingPropertyGroup(groupName: "{=JZ32iR}Notable Settings", order: 6)]
        public int NotablePowerThresholdForNobleRecruit { get; set; } = 600;

        [SettingPropertyFloatingInteger(displayName: "{=CpOhvD}Notable Noble Recruit Power Cost", minValue: 0f, maxValue: 10f, Order = 1, RequireRestart = false, HintText = "{=45urKB}The power cost for one noble recuit.")]
        [SettingPropertyGroup(groupName: "{=JZ32iR}Notable Settings")]
        public float NotableNobleRecruitPowerCost { get; set; } = 1.0f;
    }
}