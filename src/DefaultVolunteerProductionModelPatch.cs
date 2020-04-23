using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultVolunteerProductionModel), "GetDailyVolunteerProductionProbability")]
    public class GetDailyVolunteerProductionProbabilityPatch
    {
        static void Postfix(ref float __result,
            Hero hero,
            int index,
            Settlement settlement)
        {
            if (settlement.IsTown)
            {
                float multiplier = (settlement.Prosperity - SubModule.Settings.townMinProsperityForRecruit) /
                    (SubModule.Settings.townProsperityThreshould - SubModule.Settings.townMinProsperityForRecruit);
                __result *= multiplier;
            }
            if (settlement.IsVillage)
            {
                float multiplier = (settlement.Prosperity - SubModule.Settings.villageMinProsperityForRecruit) /
                    (SubModule.Settings.villageProsperityThreshould - SubModule.Settings.villageMinProsperityForRecruit);
                __result *= multiplier;
            }
        }
    }
}
