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
                foreach (StatExplainer.ExplanationLine line in explanation.Lines)
                {
                    line.Number *= SubModule.Settings.militiaGrowthMultiplier;
                }
            }

            __result *= SubModule.Settings.militiaGrowthMultiplier;
        }

        static bool Prepare()
        {
            return SubModule.Settings.militiaGrowthMultiplier != 1;
        }
    }
}
