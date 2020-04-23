using HarmonyLib;
using Helpers;
using System;
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

        private static void GenerateBasicTroop(Hero notable, int index)
        {
            CultureObject cultureObject = notable.CurrentSettlement != null ? notable.CurrentSettlement.Culture : notable.Clan.Culture;
            if (HeroHelper.HeroShouldGiveEliteTroop(notable) && ((double)MBRandom.RandomFloat < ((double)notable.Power - 200d) / 200d))
            {
                notable.VolunteerTypes[index] = cultureObject.EliteBasicTroop;
                int powerMinus = Math.Min(notable.Power - 1, SubModule.Settings.notableNobleRecruitPowerCost);
                notable.AddPower(-powerMinus);
            }
            else
            {
                notable.VolunteerTypes[index] = cultureObject.BasicTroop;
            }
        }

        private static void UpgradeTroop(Hero notable, int index)
        {
            double num2 = 200;
            float num3 = (float)(num2 * num2 / (notable.Power * notable.Power));
            if ((double)MBRandom.RandomFloat < 1 / ((double)notable.VolunteerTypes[index].Level * num3) && notable.VolunteerTypes[index].UpgradeTargets != null)
            {
                notable.VolunteerTypes[index] = notable.VolunteerTypes[index].UpgradeTargets[MBRandom.RandomInt(notable.VolunteerTypes[index].UpgradeTargets.Length)];
            }
        }

        private static void SortNotableVolunteers(Hero notable)
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
                            //double num1 = !notable.IsRuralNotable || notable.Power < 200 ? 0.5 : 1.5;
                            for (int index = 0; index < 6; ++index)
                            {
                                if ((double)MBRandom.RandomFloat < (double)Campaign.Current.Models.VolunteerProductionModel.GetDailyVolunteerProductionProbability(notable, index, settlement))
                                {
                                    flag = true;
                                    if (!IsBitSet(notable, index))
                                    {
                                        GenerateBasicTroop(notable, index);                                                                                 
                                    }
                                    else
                                    {
                                        UpgradeTroop(notable, index);
                                    }
                                }
                            }
                            if (flag)
                            {
                                SortNotableVolunteers(notable);
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
