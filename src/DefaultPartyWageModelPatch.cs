using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultPartyWageModel), "GetTotalWage")]
    public class GetTotalWagePatch
    {
        static void Postfix(ref int __result, MobileParty mobileParty, StatExplainer explanation = null)
        {
            if (mobileParty.IsGarrison)
            {
                __result = __result / 2;
            }
        }
    }
}
