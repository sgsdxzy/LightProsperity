using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultPartyWageModel), "GetTotalWage")]
    public class GetTotalWagePatch
    {
        public static void Postfix(ref int __result, MobileParty mobileParty, StatExplainer explanation = null)
        {
            if (mobileParty.IsGarrison)
            {
                __result = (int)(__result * Settings.Instance.GarrisonWagesMultiplier);
            }
        }

        public static bool Prepare()
        {
            return Settings.Instance.ModifyGarrisonConsumption;
        }
    }
}
