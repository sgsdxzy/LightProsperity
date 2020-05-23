using HarmonyLib;
using Helpers;
using System;
using TaleWorlds.CampaignSystem;


namespace LightProsperity
{
    [HarmonyPatch(typeof(HeroHelper), "MaximumIndexHeroCanRecruitFromHero")]
    public class MaximumIndexHeroCanRecruitFromHeroPatch
    {
        public static bool Prefix(ref int __result,
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

            // Prosperity bonus
            int num8_0 = 0;
            Settlement settlement = sellerHero.CurrentSettlement;
            if (settlement != null)
            {
                if (settlement.IsTown)
                {
                    float prosperity = settlement.Prosperity;
                    num8_0 = (int)Math.Floor((prosperity - Settings.Instance.TownProsperityThreshold) / Settings.Instance.TownProsperityPerBonusSlot);
                }
                if (settlement.IsVillage)
                {
                    float prosperity = settlement.Village.Hearth;
                    num8_0 = (int)Math.Floor((prosperity - Settings.Instance.VillageProsperityThreshold) / Settings.Instance.VillageProsperityPerBonusSlot);
                }
            }

            int num8;
            if (num8_0 > 0)
            {
                switch (Settings.Instance.BonusSlotsFor.SelectedIndex)
                {
                    case 0:
                        num8 = num8_0;
                        break;
                    case 1:
                        num8 = !buyerHero.MapFaction.IsAtWarWith(sellerHero.CurrentSettlement.MapFaction) ? num8_0 : 0;
                        break;
                    case 2:
                        num8 = buyerHero.MapFaction == sellerHero.CurrentSettlement.MapFaction ? num8_0 : 0;
                        break;
                    default:
                        num8 = buyerHero.Clan == sellerHero.CurrentSettlement.OwnerClan ? num8_0 : 0;
                        break;
                }
            }
            else
            {
                num8 = num8_0;
            }

            __result = Math.Max(0, num1 + num6 + num2 + num3 + num4 + num7 + num8);
            return false;
        }
    }
}
