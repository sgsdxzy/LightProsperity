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
                if (settlement.Prosperity <= SubModule.Settings.townMinProsperityForRecruit)
                {
                    __result = 0;
                    return;
                }
                __result = __result * settlement.Prosperity / SubModule.Settings.townProsperityThreshould;
            }
            if (settlement.IsVillage)
            {
                if (settlement.Village.Hearth <= SubModule.Settings.villageMinProsperityForRecruit)
                {
                    __result = 0;
                    return;
                }
                __result = __result * settlement.Village.Hearth / SubModule.Settings.villageProsperityThreshould;
            }
        }
    }
}
