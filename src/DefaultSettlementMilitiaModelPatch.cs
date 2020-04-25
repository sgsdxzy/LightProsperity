using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementMilitiaModel), "CalculateMilitiaChange")]
    public class CalculateMilitiaChangePatch
    {
        static void Postfix(ref float __result,
            Settlement settlement,
            StatExplainer explanation)
        {
            if (explanation != null)
            {
                explanation.AddLine("Prosperity Bonus", SubModule.Settings.militiaGrowthBonus);
            }

            __result += SubModule.Settings.militiaGrowthBonus;
        }

        static bool Prepare()
        {
            return SubModule.Settings.militiaGrowthBonus != 0;
        }
    }
}
