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
            double notableMinPowerForNobleRecruit = 200;
            double chance = ((double)notable.Power - notableMinPowerForNobleRecruit) / (SubModule.Settings.notablePowerThresholdForNobleRecruit - notableMinPowerForNobleRecruit);
            // chance = Math.Max(chance, 0);
            // chance = Math.Sqrt(chance);
            if (HeroHelper.HeroShouldGiveEliteTroop(notable) && ((double)MBRandom.RandomFloat < chance))
            {
                notable.VolunteerTypes[index] = cultureObject.EliteBasicTroop;
                int powerMinus = Math.Min(notable.Power - 1, (int)SubModule.Settings.notableNobleRecruitPowerCost);
                notable.AddPower(-powerMinus);
            }
            else
            {
                notable.VolunteerTypes[index] = cultureObject.BasicTroop;
            }
        }

        private static double GetTroopUpgradeChance(Hero notable, int index)
        {
            double difficulty = 1.0;
            double num2 = 200;
            double num3 = num2 * num2 / (notable.Power * notable.Power);
            double level_diff = Math.Pow(notable.VolunteerTypes[index].Level, 2) / 11;
            double chance = 1 / (level_diff * num3 * difficulty);

            return chance;
        }

        private static void UpgradeTroop(Hero notable, int index)
        {  
            if (notable.VolunteerTypes[index].UpgradeTargets != null)
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

        static int GetDailyCastleNobleRecruitCount(Settlement settlement)
        {
            double chance = (settlement.Prosperity - SubModule.Settings.castleMinProsperityForRecruit) / 
                (SubModule.Settings.castleProsperityThreshold - SubModule.Settings.castleMinProsperityForRecruit);
            int num = (int)Math.Floor(chance);
            num += (double)MBRandom.RandomFloat < (chance - num) ? 1 : 0;
            return num;
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
                                        for (int i = 0; i < 4; i++)
                                        {
                                            if ((double)MBRandom.RandomFloat < GetTroopUpgradeChance(notable, index))
                                            {
                                                UpgradeTroop(notable, index);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((double)MBRandom.RandomFloat < GetTroopUpgradeChance(notable, index))
                                        {
                                            UpgradeTroop(notable, index);
                                        }
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
                if (settlement.IsCastle)
                {
                    int num = GetDailyCastleNobleRecruitCount(settlement);
                    if (num > 0)
                    {
                        CharacterObject troop = settlement.Culture.EliteBasicTroop;
                        if (settlement.Town.GarrisonParty == null)
                        {
                            settlement.AddGarrisonParty(false);
                        }
                        int max_num = settlement.Town.GarrisonParty.Party.PartySizeLimit - settlement.Town.GarrisonParty.Party.NumberOfAllMembers;
                        int count = Math.Min(num, max_num);
                        settlement.Town.GarrisonParty.MemberRoster.AddToCounts(troop, count, false, 0, 0, true, -1);
                        settlement.Prosperity -= SubModule.Settings.castleRecruitProsperityCost * count;
                    }
                }
            }
            return false;
        }
    }
}
