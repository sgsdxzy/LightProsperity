using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementMilitiaModel), "CalculateMilitiaChange")]
    public class CalculateMilitiaChangePatch
    {
        public static void Postfix(ref float __result,
            Settlement settlement,
            StatExplainer explanation)
        {
            if (explanation != null)
            {
                explanation.AddLine("Prosperity Bonus", Settings.Instance.MilitiaGrowthBonus);
            }

            __result += Settings.Instance.MilitiaGrowthBonus;
        }

        public static bool Prepare()
        {
            return Settings.Instance.MilitiaGrowthBonus != 0;
        }
    }
}
