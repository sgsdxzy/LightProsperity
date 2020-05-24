using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultSettlementFoodModel), "CalculateTownFoodStocksChange")]
    public class CalculateTownFoodStocksChangePatch
    {
        public static void Postfix(ref float __result,
            Town town, StatExplainer explanation)
        {
            MobileParty garrisonParty = town.GarrisonParty;
            int num2_old = -(garrisonParty != null ? garrisonParty.Party.NumberOfAllMembers : 0) / 20;
            int num2_new = (int)(num2_old * SubModule.Settings.GarrisonFoodConsumpetionMultiplier);
            __result = __result - num2_old + num2_new;

            if (explanation != null && explanation.Lines.Count > 1) explanation.Lines[1].Number = num2_new;
        }

        public static bool Prepare()
        {
            return SubModule.Settings.ModifyGarrisonConsumption;
        }
    }
}