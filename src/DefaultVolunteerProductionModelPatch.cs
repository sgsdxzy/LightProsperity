using System;
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
                double multiplier = (settlement.Prosperity - SubModule.Settings.townMinProsperityForRecruit) /
                    (SubModule.Settings.townProsperityThreshold - SubModule.Settings.townMinProsperityForRecruit);
                multiplier = Math.Max(multiplier, 0);
                multiplier = Math.Pow(multiplier, 0.7);
                __result *= (float)multiplier;
            }
            if (settlement.IsVillage)
            {
                double multiplier = (settlement.Village.Hearth - SubModule.Settings.villageMinProsperityForRecruit) /
                    (SubModule.Settings.villageProsperityThreshold - SubModule.Settings.villageMinProsperityForRecruit);
                multiplier = Math.Max(multiplier, 0);
                multiplier = Math.Pow(multiplier, 0.7);
                __result *= (float)multiplier;
            }
        }
    }
}
