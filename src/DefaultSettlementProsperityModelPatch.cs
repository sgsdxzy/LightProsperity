using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateProsperityChange")]
    public class CalculateProsperityChangePatch
    {
        static void Postfix(ref float __result,
            Town fortification, StatExplainer explanation)
        {    
            if (explanation != null)
            {
                foreach (StatExplainer.ExplanationLine line in explanation.Lines)
                {
                    line.Number *= SubModule.Settings.prosperityGrowthMultiplier;
                }
            }

            __result *= SubModule.Settings.prosperityGrowthMultiplier;
        }

        static bool Prepare()
        {
            return !SubModule.Settings.newProsperityModel && SubModule.Settings.prosperityGrowthMultiplier != 1;
        }
    }


    [HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateHearthChange")]
    public class CalculateHearthChangePatch
    {
        static void Postfix(ref float __result,
            Village village, StatExplainer explanation)
        {
            if (explanation != null)
            {
                foreach (StatExplainer.ExplanationLine line in explanation.Lines)
                {
                    line.Number *= SubModule.Settings.prosperityGrowthMultiplier;
                }
            }

            __result *= SubModule.Settings.prosperityGrowthMultiplier;
        }

        static bool Prepare()
        {
            return !SubModule.Settings.newProsperityModel && SubModule.Settings.prosperityGrowthMultiplier != 1;
        }
    }
}
