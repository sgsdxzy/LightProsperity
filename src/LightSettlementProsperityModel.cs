using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;


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
        private readonly TextObject _overcrowdedText = new TextObject("Overcrowded", (Dictionary<string, TextObject>)null);
        private readonly TextObject _enemyText = new TextObject("Enemy Around", (Dictionary<string, TextObject>)null);
        private readonly TextObject _foodWorriesText = new TextObject("Food Running Out", (Dictionary<string, TextObject>)null);

        private readonly float _hearthMultiplier = 0.02f;
        private readonly float _townProsperityMultiplier = 0.01f;
        private readonly float _castleProsperityMultiplier = 0.01f;

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
                float newBorn = village.Hearth;
                float overCrowded = 0.0016f * village.Hearth * village.Hearth;
                float population = (newBorn - overCrowded) * _hearthMultiplier;
                if (population > 0)
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _newBornText);
                }
                else
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _overcrowdedText);
                }

                float enemyRatio = Math.Min(0.8f * village.Settlement.NumberOfEnemiesSpottedAround, 1f);
                float enemyAround = -enemyRatio * village.Hearth * _hearthMultiplier;
                explainedNumber.Add(enemyAround * SubModule.Settings.prosperityGrowthMultiplier, _enemyText);
            }          
            else if (village.VillageState == Village.VillageStates.Looted)
                explainedNumber.Add(-village.Hearth * 0.02f, this._raidedText);
            if (village.Bound != null && (double)explainedNumber.ResultNumber > 0.0)
            {
                ExplainedNumber bonuses = new ExplainedNumber(explainedNumber.ResultNumber, (StringBuilder)null);
                PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.BushDoctor, village.Bound.Town, ref bonuses);
                explainedNumber.Add(bonuses.ResultNumber - explainedNumber.ResultNumber, this._governor);
            }
            return explainedNumber.ResultNumber;
        }

        private float CalculateProsperityChangeInternal(Town fortification, StatExplainer explanation = null)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(0.0f, explanation, (TextObject)null);
            if (fortification.Settlement.Culture.ProsperityBonus > 0)
                explainedNumber.Add((float)fortification.Settlement.Culture.ProsperityBonus * SubModule.Settings.prosperityGrowthMultiplier, this._empireProsperityBonus);

            if (fortification.IsTown)
            {
                float newBorn = fortification.Prosperity;
                float overCrowded = 0.00016f * fortification.Prosperity * fortification.Prosperity;
                float population = (newBorn - overCrowded) * _townProsperityMultiplier;
                if (population > 0)
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _newBornText);
                } else
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _overcrowdedText);
                }
                
                float enemyRatio = Math.Min(0.6f * fortification.Settlement.NumberOfEnemiesSpottedAround, 1f);
                float enemyAround = enemyRatio * fortification.Prosperity * _townProsperityMultiplier;
                explainedNumber.Add(-enemyAround * SubModule.Settings.prosperityGrowthMultiplier, _enemyText);

                float foodStocksChange = Campaign.Current.Models.SettlementFoodModel.CalculateTownFoodStocksChange(fortification, (StatExplainer)null);
                int num1 = !fortification.Owner.IsStarving || (double)foodStocksChange >= 0.0 ? 0 : (int)foodStocksChange;
                explainedNumber.Add((float)num1 * SubModule.Settings.prosperityGrowthMultiplier, _foodShortageText);

                if (foodStocksChange < 0)
                {
                    float daysLast = fortification.FoodStocks / -foodStocksChange;
                    float foodWorries = Math.Max(1 - daysLast / 21, 0) * fortification.Prosperity * _townProsperityMultiplier;
                    if (foodWorries > 0)
                    {
                        explainedNumber.Add(-foodWorries * SubModule.Settings.prosperityGrowthMultiplier, _foodWorriesText);
                    }
                }             
            }
            if (fortification.IsCastle)
            {
                float newBorn = fortification.Prosperity;
                float overCrowded = 0.0007f * fortification.Prosperity * fortification.Prosperity;
                float population = (newBorn - overCrowded) * _castleProsperityMultiplier;
                if (population > 0)
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _newBornText);
                }
                else
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _overcrowdedText);
                }

                float enemyRatio = Math.Min(0.4f * fortification.Settlement.NumberOfEnemiesSpottedAround, 1f);
                float enemyAround = enemyRatio * fortification.Prosperity * _castleProsperityMultiplier;
                explainedNumber.Add(-enemyAround * SubModule.Settings.prosperityGrowthMultiplier, _enemyText);

                float foodStocksChange = Campaign.Current.Models.SettlementFoodModel.CalculateTownFoodStocksChange(fortification, (StatExplainer)null);
                int num1 = !fortification.Owner.IsStarving || (double)foodStocksChange >= 0.0 ? 0 : (int)foodStocksChange;
                explainedNumber.Add((float)num1 * SubModule.Settings.prosperityGrowthMultiplier, _foodShortageText);

                if (foodStocksChange < 0)
                {
                    float daysLast = fortification.FoodStocks / -foodStocksChange;
                    float foodWorries = Math.Max(1 - daysLast / 42, 0) * fortification.Prosperity * _castleProsperityMultiplier;
                    if (foodWorries > 0)
                    {
                        explainedNumber.Add(-foodWorries * SubModule.Settings.prosperityGrowthMultiplier, _foodWorriesText);
                    }
                }
            }      

            if (fortification.IsTown)
            {
                int num4 = fortification.SoldItems.Sum<Town.SellLog>((Func<Town.SellLog, int>)(x => x.Category.Properties != ItemCategory.Property.BonusToProsperity ? 0 : x.Number));
                if (num4 > 0)
                    explainedNumber.Add((float)num4 * 3 * SubModule.Settings.prosperityGrowthMultiplier, this._prosperityFromMarketText);
            }
            PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PristineStreets, fortification, ref explainedNumber);
            foreach (Building building in fortification.Buildings)
            {
                int buildingEffectAmount = building.GetBuildingEffectAmount(DefaultBuildingEffects.Prosperity);
                if ((!building.BuildingType.IsDefaultProject || fortification.CurrentBuilding == building) && buildingEffectAmount > 0)
                    explainedNumber.Add((float)buildingEffectAmount * SubModule.Settings.prosperityGrowthMultiplier, building.Name);
                if (building.BuildingType == DefaultBuildingTypes.SettlementAquaducts)
                    PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.CleanInfrastructure, fortification, ref explainedNumber);
            }
            if ((double)fortification.Loyalty > 75.0)
                explainedNumber.Add(Campaign.Current.Models.SettlementLoyaltyModel.HighLoyaltyProsperityEffect * SubModule.Settings.prosperityGrowthMultiplier, this._loyaltyText);
            else if ((double)fortification.Loyalty <= 50.0)
                explainedNumber.Add(Campaign.Current.Models.SettlementLoyaltyModel.LowLoyaltyProsperityEffect * SubModule.Settings.prosperityGrowthMultiplier, this._loyaltyText);
            if (fortification.Settlement.OwnerClan.Kingdom != null)
            {
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom))
                    explainedNumber.Add(-1f * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.Serfdom.Name);
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoadTolls))
                    explainedNumber.Add(-0.2f * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.RoadTolls.Name);
                if (fortification.Settlement.OwnerClan.Kingdom.RulingClan == fortification.Settlement.OwnerClan && fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.ImperialTowns))
                    explainedNumber.Add(1f * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.ImperialTowns.Name);
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
                    explainedNumber.Add(-1f * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.CrownDuty.Name);
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax))
                    explainedNumber.Add(-1f * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.WarTax.Name);
            }
            this.GetSettlementProsperityChangeDueToIssues(fortification.Settlement, ref explainedNumber);
            return explainedNumber.ResultNumber;
        }

        private void GetSettlementProsperityChangeDueToIssues(
          Settlement settlement,
          ref ExplainedNumber result)
        {
            float totalChange;
            if (!IssueManager.DoesSettlementHasIssueEffect(DefaultIssueEffects.SettlementProsperity, settlement, out totalChange))
                return;
            result.Add(totalChange, this._issueText);
        }
    }
}
