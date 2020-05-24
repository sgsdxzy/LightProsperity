using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace LightProsperity
{
    public class LightSettlementProsperityModel : SettlementProsperityModel
    {
        private readonly TextObject _loyaltyText = GameTexts.FindText("str_loyalty", (string)null);
        private readonly TextObject _foodShortageText = new TextObject("{=qTFKvGSg}Food Shortage", (Dictionary<string, TextObject>)null);
        private readonly TextObject _prosperityFromMarketText = new TextObject("{=RNT5hMVb}Goods From Market", (Dictionary<string, TextObject>)null);
        private readonly TextObject _surplusFoodText = GameTexts.FindText("str_surplus_food", (string)null);
        private readonly TextObject _issueText = GameTexts.FindText("str_issues", (string)null);
        private readonly TextObject _empireProsperityBonus = new TextObject("{=3Ditaq1M}Empire Prosperity Bonus", (Dictionary<string, TextObject>)null);
        private readonly TextObject _governor = new TextObject("{=Fa2nKXxI}Governor", (Dictionary<string, TextObject>)null);
        private readonly TextObject _issues = new TextObject("{=D7KllIPI}Issues", (Dictionary<string, TextObject>)null);
        private readonly TextObject _newBornText = new TextObject("{=RVas571P}New Born", (Dictionary<string, TextObject>)null);
        private readonly TextObject _raidedText = new TextObject("{=RVas572P}Raided", (Dictionary<string, TextObject>)null);
        private readonly TextObject _populationLossText = new TextObject("{=iz6aMY}Population Loss", (Dictionary<string, TextObject>)null);
        private readonly TextObject _enemyText = new TextObject("{=YpDrMf}Enemy Around", (Dictionary<string, TextObject>)null);
        private readonly TextObject _foodWorriesText = new TextObject("{=PBMzr8}Food Running Out", (Dictionary<string, TextObject>)null);

        private static readonly TextObject _textGovernor = new TextObject("{=Fa2nKXxI}Governor", (Dictionary<string, TextObject>)null);

        private static readonly float _hearthMultiplier = 0.01f;
        private static readonly float _townProsperityMultiplier = 0.0075f;
        private static readonly float _castleProsperityMultiplier = 0.0075f;
        private static readonly float _vanillaToRatio = 0.05f;

        private readonly float _hearthCoeff = SubModule.Settings.VillageGrowthCap == 0 ? 0f : 1f / SubModule.Settings.VillageGrowthCap;
        private readonly float _townCoeff = SubModule.Settings.TownGrowthCap == 0 ? 0f : 1f / SubModule.Settings.TownGrowthCap;
        private readonly float _castleCoeff = SubModule.Settings.CastleGrowthCap == 0 ? 0f : 1f / SubModule.Settings.CastleGrowthCap;

        internal static void AddDefaultDailyBonus(Town fortification, ref ExplainedNumber result)
        {
            float num = (float)((double)fortification.Construction * (double)fortification.CurrentBuilding.BuildingType.Effects[0].Level1Effect * 0.00999999977648258);
            result.Add(num * SubModule.Settings.ProsperityGrowthMultiplier, fortification.CurrentBuilding.BuildingType.Name);
        }

        public override float CalculateProsperityChange(Town fortification, StatExplainer explanation = null)
        {
            return this.CalculateProsperityChangeInternal(fortification, explanation);
        }

        public override float CalculateHearthChange(Village village, StatExplainer explanation = null)
        {
            return this.CalculateHearthChangeInternal(village, explanation);
        }

        private float CalculateHearthChangeInternal(Village village, StatExplainer explanation = null)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(0.0f, explanation, (TextObject)null);
            if (village.VillageState == Village.VillageStates.Normal)
            {
                float newBorn = Math.Max(village.Hearth * _hearthMultiplier, 1);
                float loss = _hearthCoeff * village.Hearth * village.Hearth * _hearthMultiplier;
                float population = newBorn - loss;
                if (population > 0)
                {
                    explainedNumber.Add(population * SubModule.Settings.ProsperityGrowthMultiplier, _newBornText);
                }
                else
                {
                    explainedNumber.Add(population * SubModule.Settings.ProsperityGrowthMultiplier, _populationLossText);
                }

                float enemyRatio = Math.Min(0.8f * village.Settlement.NumberOfEnemiesSpottedAround, 1f);
                float enemyAround = -enemyRatio * village.Hearth * _hearthMultiplier;
                explainedNumber.Add(enemyAround * SubModule.Settings.ProsperityGrowthMultiplier, _enemyText);

                if (village.Bound != null)
                {
                    if (village.Bound.Town.CurrentBuilding != null && village.Bound.Town.CurrentBuilding.BuildingType == DefaultBuildingTypes.IrrigationDaily)
                        AddDefaultDailyBonus(village.Bound.Town, ref explainedNumber);
                    AddPerkBonusForTown(DefaultPerks.Medicine.BushDoctor, village.Bound.Town, ref explainedNumber, newBorn);
                }
            }
            else if (village.VillageState == Village.VillageStates.Looted)
                explainedNumber.Add(-village.Hearth * 0.01f * SubModule.Settings.ProsperityGrowthMultiplier, this._raidedText);

            return explainedNumber.ResultNumber;
        }

        private float CalculateProsperityChangeInternal(Town fortification, StatExplainer explanation = null)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(0.0f, explanation, (TextObject)null);

            float newBorn = 0, loss = 0;
            float enemyRatio = 0, enemyAround = 0;
            float foodStocksChange = Campaign.Current.Models.SettlementFoodModel.CalculateTownFoodStocksChange(fortification, (StatExplainer)null);
            float daysLast = fortification.FoodStocks / -foodStocksChange;
            float foodWorries = 0;
            if (fortification.IsTown)
            {
                newBorn = fortification.Prosperity * _townProsperityMultiplier;
                loss = _townCoeff * fortification.Prosperity * fortification.Prosperity * _townProsperityMultiplier;
                enemyRatio = Math.Min(0.6f * fortification.Settlement.NumberOfEnemiesSpottedAround, 1f);
                foodWorries = daysLast < 0 ? 0 : Math.Max(1 - daysLast / 21, 0) * newBorn;

            }
            if (fortification.IsCastle)
            {
                newBorn = fortification.Prosperity * _castleProsperityMultiplier;
                loss = _castleCoeff * fortification.Prosperity * fortification.Prosperity * _castleProsperityMultiplier;
                enemyRatio = Math.Min(0.4f * fortification.Settlement.NumberOfEnemiesSpottedAround, 1f);
                foodWorries = daysLast < 0 ? 0 : Math.Max(1 - daysLast / 42, 0) * newBorn;
            }
            float population = newBorn - loss;
            if (population > 0)
            {
                explainedNumber.Add(population * SubModule.Settings.ProsperityGrowthMultiplier, _newBornText);
            }
            else
            {
                explainedNumber.Add(population * SubModule.Settings.ProsperityGrowthMultiplier, _populationLossText);
            }
            enemyAround = enemyRatio * newBorn;
            explainedNumber.Add(-enemyAround * SubModule.Settings.ProsperityGrowthMultiplier, _enemyText);
            float starving = !fortification.Owner.IsStarving || foodStocksChange >= 0.0f ? 0f : foodStocksChange;
            if (starving < 0)
            {
                explainedNumber.Add(starving * SubModule.Settings.ProsperityGrowthMultiplier, _foodShortageText);
            }
            if (foodWorries > 0)
            {
                explainedNumber.Add(-foodWorries * SubModule.Settings.ProsperityGrowthMultiplier, _foodWorriesText);
            }

            if (fortification.Settlement.Culture.ProsperityBonus > 0)
            {
                float bonus = (float)fortification.Settlement.Culture.ProsperityBonus * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, this._empireProsperityBonus);
            }

            if (fortification.IsTown)
            {
                int num4 = fortification.SoldItems.Sum<Town.SellLog>((Func<Town.SellLog, int>)(x => x.Category.Properties != ItemCategory.Property.BonusToProsperity ? 0 : x.Number));
                if (num4 > 0)
                {
                    float bonus = (float)num4 * 2.0f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, this._prosperityFromMarketText);
                }
            }

            AddPerkBonusForTown(DefaultPerks.Medicine.PristineStreets, fortification, ref explainedNumber, newBorn);
            if (fortification.CurrentBuilding.BuildingType == DefaultBuildingTypes.BuildHouseDaily)
                AddDefaultDailyBonus(fortification, ref explainedNumber);
            foreach (Building building in fortification.Buildings)
            {
                float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.Prosperity);
                if (!building.BuildingType.IsDefaultProject && buildingEffectAmount > 0)
                {
                    float bonus = (float)buildingEffectAmount * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, building.Name);
                }

                if (building.BuildingType == DefaultBuildingTypes.SettlementAquaducts)
                {
                    AddPerkBonusForTown(DefaultPerks.Medicine.CleanInfrastructure, fortification, ref explainedNumber, newBorn);
                }

            }
            if ((double)fortification.Loyalty > 75.0)
            {
                float bonus = Campaign.Current.Models.SettlementLoyaltyModel.HighLoyaltyProsperityEffect * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, this._loyaltyText);
            }

            else if ((double)fortification.Loyalty <= 50.0)
            {
                float bonus = Campaign.Current.Models.SettlementLoyaltyModel.LowLoyaltyProsperityEffect * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, this._loyaltyText);
            }

            if (fortification.Settlement.OwnerClan.Kingdom != null)
            {
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, DefaultPolicies.Serfdom.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoadTolls))
                {
                    float bonus = -0.2f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, DefaultPolicies.RoadTolls.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.RulingClan == fortification.Settlement.OwnerClan && fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.ImperialTowns))
                {
                    float bonus = 1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, DefaultPolicies.ImperialTowns.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, DefaultPolicies.CrownDuty.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, DefaultPolicies.WarTax.Name);
                }
            }
            this.GetSettlementProsperityChangeDueToIssues(fortification.Settlement, ref explainedNumber, newBorn);
            return explainedNumber.ResultNumber;
        }

        private void GetSettlementProsperityChangeDueToIssues(
          Settlement settlement,
          ref ExplainedNumber result,
          float newBorn)
        {
            float totalChange;
            if (!IssueManager.DoesSettlementHasIssueEffect(DefaultIssueEffects.SettlementProsperity, settlement, out totalChange))
                return;
            float bonus = totalChange * _vanillaToRatio * newBorn;
            result.Add(bonus * SubModule.Settings.ProsperityGrowthMultiplier, this._issueText);
        }

        private static void AddPerkBonusForTown(PerkObject perk, Town town, ref ExplainedNumber bonuses, float newBorn)
        {
            if (perk.PrimaryRole != SkillEffect.PerkRole.Governor && perk.SecondaryRole != SkillEffect.PerkRole.Governor)
                return;
            Hero governor = town.Governor;
            if (governor == null || !governor.GetPerkValue(perk) || (governor.CurrentSettlement == null || governor.CurrentSettlement != town.Settlement))
                return;
            float number = perk.PrimaryRole == SkillEffect.PerkRole.Governor ? perk.PrimaryBonus : perk.SecondaryBonus;
            AddToStat(ref bonuses, perk.IncrementType, number, _textGovernor, newBorn);
        }

        private static void AddToStat(
            ref ExplainedNumber stat,
            SkillEffect.EffectIncrementType effectIncrementType,
            float number,
            TextObject text,
            float newBorn)
        {
            if (effectIncrementType == SkillEffect.EffectIncrementType.Add)
            {
                stat.Add(number * _vanillaToRatio * newBorn * SubModule.Settings.ProsperityGrowthMultiplier, text);
            }
            else
            {
                if (effectIncrementType != SkillEffect.EffectIncrementType.AddFactor)
                    return;
                stat.AddFactor(number * 0.01f, text);
            }
        }
    }
}
