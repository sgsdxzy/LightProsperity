using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;


namespace LightProsperity
{
    [HarmonyPatch(typeof(SellPrisonersAction), "ApplyInternal")]
    public class ApplyInternalPatch
    {
        static void Prefix(MobileParty sellerParty,
            TroopRoster prisoners,
            Settlement currentSettlement,
            bool applyGoldChange)
        {
            if (currentSettlement != null)
            {
                currentSettlement.Prosperity += prisoners.TotalRegulars * SubModule.Settings.prisonerProsperityValue;
            }
        }
    }
}
