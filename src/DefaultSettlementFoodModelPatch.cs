using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementFoodModel), "CalculateTownFoodStocksChange")]
    public class CalculateTownFoodStocksChangePatch
    {
        static void Postfix(ref float __result,
            Town town, StatExplainer explanation)
        {
            MobileParty garrisonParty = town.GarrisonParty;
            int num2_old = -(garrisonParty != null ? garrisonParty.Party.NumberOfAllMembers : 0) / 20;
            int num2_new = -(int)(garrisonParty != null ? garrisonParty.Party.NumberOfAllMembers * SubModule.Settings.garrisonFoodConsumpetionMultiplier : 0) / 20;
            __result = __result - num2_old + num2_new;

            if (explanation != null && explanation.Lines.Count > 1) explanation.Lines[1].Number = num2_new;
        }

        static bool Prepare()
        {
            return SubModule.Settings.garrisonFoodConsumpetionMultiplier != 1;
        }
    }
}