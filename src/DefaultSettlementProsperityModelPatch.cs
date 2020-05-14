using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateProsperityChange")]
    public class CalculateProsperityChangePatch
    {
        public static void Postfix(ref float __result,
            Town fortification, StatExplainer explanation)
        {
            if (explanation != null)
            {
                foreach (StatExplainer.ExplanationLine line in explanation.Lines)
                {
                    line.Number *= Settings.Instance.ProsperityGrowthMultiplier;
                }
            }

            __result *= Settings.Instance.ProsperityGrowthMultiplier;
        }

        public static bool Prepare()
        {
            return !Settings.Instance.NewProsperityModel && Settings.Instance.ProsperityGrowthMultiplier != 1;
        }
    }


    [HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateHearthChange")]
    public class CalculateHearthChangePatch
    {
        public static void Postfix(ref float __result,
            Village village, StatExplainer explanation)
        {
            if (explanation != null)
            {
                foreach (StatExplainer.ExplanationLine line in explanation.Lines)
                {
                    line.Number *= Settings.Instance.ProsperityGrowthMultiplier;
                }
            }

            __result *= Settings.Instance.ProsperityGrowthMultiplier;
        }

        public static bool Prepare()
        {
            return !Settings.Instance.NewProsperityModel && Settings.Instance.ProsperityGrowthMultiplier != 1;
        }
    }
}
