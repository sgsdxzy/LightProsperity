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
                if (settlement.Prosperity <= 1000)
                {
                    // A recruit cost 20 prosperity
                    __result = 0;
                    return;
                }
                __result = __result * settlement.Prosperity / 3000;
            }
            if (settlement.IsVillage)
            {
                if (settlement.Village.Hearth <= 100)
                {
                    // A recruit cost 10 hearth
                    __result = 0;
                    return;
                }
                __result = __result * settlement.Village.Hearth / 300;
            }
        }
    }
}
