using HarmonyLib;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;


namespace LightProsperity
{
    [HarmonyPatch(typeof(RecruitCampaignBehavior), "OnTroopRecruited")]
    public class OnTroopRecruitedPatch
    {
        static void Postfix(Hero arg1,
            Settlement arg2,
            Hero individual,
            CharacterObject troop,
            int count)
        {           
            if (individual != null)
            {
                Settlement settlement = individual.CurrentSettlement;
                if (settlement != null)
                {
                    if (settlement.IsTown)
                    {
                        settlement.Prosperity -= SubModule.Settings.townRecruitProsperityCost * count;
                        if (settlement.Prosperity < 0)
                        {
                            settlement.Prosperity = 0;
                        }
                    }
                    if (settlement.IsVillage)
                    {
                        settlement.Village.Hearth -= SubModule.Settings.villageRecruitProsperityCost * count;
                        if (settlement.Village.Hearth < 0)
                        {
                            settlement.Village.Hearth = 0;
                        }
                    }
                }
            }        
        }
    }

    [HarmonyPatch(typeof(RecruitCampaignBehavior), "OnUnitRecruited")]
    public class OnUnitRecruitedPatch
    {
        static void Postfix(CharacterObject arg1, int count)
        {
            Settlement settlement = Hero.MainHero.CurrentSettlement;
            if (settlement != null)
            {
                if (settlement.IsTown)
                {
                    settlement.Prosperity -= SubModule.Settings.townRecruitProsperityCost * count;
                }
                if (settlement.IsVillage)
                {
                    settlement.Village.Hearth -= SubModule.Settings.villageRecruitProsperityCost * count;
                    if (settlement.Village.Hearth < 0)
                    {
                        settlement.Village.Hearth = 0;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(RecruitCampaignBehavior), "UpdateVolunteersOfNotables")]
    public class UpdateVolunteersOfNotablesPatch
    {
        private static bool IsBitSet(Hero hero, int bit)
        {
            return hero.VolunteerTypes[bit] != null;
        }

        static bool Prefix(ref bool initialRunning)
        {
            foreach (Settlement settlement in Campaign.Current.Settlements)
            {
                if (settlement.IsTown && !settlement.Town.IsRebeling || settlement.IsVillage && !settlement.Village.Bound.Town.IsRebeling)
                {
                    foreach (Hero notable in settlement.Notables)
                    {
                        if (notable.CanHaveRecruits)
                        {
                            bool flag = false;
                            CultureObject cultureObject = notable.CurrentSettlement != null ? notable.CurrentSettlement.Culture : notable.Clan.Culture;
                            CharacterObject basicTroop = cultureObject.BasicTroop;
                            //double num1 = !notable.IsRuralNotable || notable.Power < 200 ? 0.5 : 1.5;
                            float num2 = 200f;
                            for (int index = 0; index < 6; ++index)
                            {
                                if ((double)MBRandom.RandomFloat < (double)Campaign.Current.Models.VolunteerProductionModel.GetDailyVolunteerProductionProbability(notable, index, settlement))
                                {
                                    if (!IsBitSet(notable, index))
                                    {             
                                        if (HeroHelper.HeroShouldGiveEliteTroop(notable) && ((double)MBRandom.RandomFloat < ((double)notable.Power - 200d) / 200d))
                                        {
                                            notable.VolunteerTypes[index] = cultureObject.EliteBasicTroop;
                                            notable.AddPower(-SubModule.Settings.notableNobleRecruitPowerCost);
                                        } else
                                        {
                                            notable.VolunteerTypes[index] = basicTroop;
                                        }                                   
                                        flag = true;                                                                           
                                    }
                                    else
                                    {
                                        float num3 = (float) ((double)num2 * (double)num2 / ((double)notable.Power * (double)notable.Power));
                                        if (MBRandom.RandomInt((int) ((double)notable.VolunteerTypes[index].Level * num3)) == 0 && notable.VolunteerTypes[index].UpgradeTargets != null)
                                        {
                                            notable.VolunteerTypes[index] = notable.VolunteerTypes[index].UpgradeTargets[MBRandom.RandomInt(notable.VolunteerTypes[index].UpgradeTargets.Length)];
                                            flag = true;
                                        }
                                    }
                                }
                            }
                            if (flag)
                            {
                                for (int index1 = 0; index1 < 6; ++index1)
                                {
                                    for (int index2 = 0; index2 < 6; ++index2)
                                    {
                                        if (notable.VolunteerTypes[index2] != null)
                                        {
                                            for (int index3 = index2 + 1; index3 < 6; ++index3)
                                            {
                                                if (notable.VolunteerTypes[index3] != null)
                                                {
                                                    if ((double)notable.VolunteerTypes[index2].Level + (notable.VolunteerTypes[index2].IsMounted ? 0.5 : 0.0) > (double)notable.VolunteerTypes[index3].Level + (notable.VolunteerTypes[index3].IsMounted ? 0.5 : 0.0))
                                                    {
                                                        CharacterObject volunteerType = notable.VolunteerTypes[index2];
                                                        notable.VolunteerTypes[index2] = notable.VolunteerTypes[index3];
                                                        notable.VolunteerTypes[index3] = volunteerType;
                                                        break;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
