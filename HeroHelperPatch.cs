using System;
using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;


namespace LightProsperity
{
    [HarmonyPatch(typeof(HeroHelper), "MaximumIndexHeroCanRecruitFromHero")]
    public class MaximumIndexHeroCanRecruitFromHeroPatch
    {
        static bool Prefix(ref int __result,
            Hero buyerHero,
            Hero sellerHero,
            int useValueAsRelation)
        {
            int num1 = 1;
            int num2 = buyerHero == Hero.MainHero ? Campaign.Current.Models.DifficultyModel.GetPlayerRecruitSlotBonus() : 0;
            int num3 = sellerHero.CurrentSettlement == null || buyerHero.MapFaction != sellerHero.CurrentSettlement.MapFaction ? 0 : 1;
            int num4 = sellerHero.CurrentSettlement == null || !buyerHero.MapFaction.IsAtWarWith(sellerHero.CurrentSettlement.MapFaction) ? 0 : -1;
            int num5 = useValueAsRelation < -100 ? buyerHero.GetRelation(sellerHero) : useValueAsRelation;
            int num6 = num5 >= 100 ? 7 : (num5 >= 80 ? 6 : (num5 >= 60 ? 5 : (num5 >= 40 ? 4 : (num5 >= 20 ? 3 : (num5 >= 10 ? 2 : (num5 >= 5 ? 1 : (num5 >= 0 ? 0 : -1)))))));

            // +1 for owned settlement
            int num7 = sellerHero.CurrentSettlement == null || buyerHero.Clan != sellerHero.CurrentSettlement.OwnerClan ? 0 : 1;

            // Prosperity bonus, only if not at war
            int num8 = 0;
            Settlement settlement = sellerHero.CurrentSettlement;
            if (settlement != null && !buyerHero.MapFaction.IsAtWarWith(sellerHero.CurrentSettlement.MapFaction))
            {
                if (settlement.IsTown)
                {
                    float prosperity = settlement.Prosperity;
                    num8 = (int)Math.Floor((prosperity - SubModule.Settings.townProsperityThreshould) / SubModule.Settings.townProsperityPerBonusSlot);
                }
                if (settlement.IsVillage)
                {
                    float prosperity = settlement.Village.Hearth;
                    num8 = (int)Math.Floor((prosperity - SubModule.Settings.villageProsperityThreshould) / SubModule.Settings.villageProsperityPerBonusSlot);
                }
            }

            __result = Math.Max(0, num1 + num6 + num2 + num3 + num4 + num7 + num8);
            return false;
        }
    }
}
