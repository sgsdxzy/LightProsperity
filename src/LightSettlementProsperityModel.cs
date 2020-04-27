using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;

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
        private readonly TextObject _populationLossText = new TextObject("Population Loss", (Dictionary<string, TextObject>)null);
        private readonly TextObject _enemyText = new TextObject("Enemy Around", (Dictionary<string, TextObject>)null);
        private readonly TextObject _foodWorriesText = new TextObject("Food Running Out", (Dictionary<string, TextObject>)null);

        private readonly float _hearthMultiplier = 0.01f;
        private readonly float _townProsperityMultiplier = 0.01f;
        private readonly float _castleProsperityMultiplier = 0.01f;
        private readonly float _vanillaToRatio = 0.05f;

        private readonly float _hearthCoeff = SubModule.Settings.villageGrowthCap == 0f ? 0f : 1 / SubModule.Settings.villageGrowthCap;
        private readonly float _townCoeff = SubModule.Settings.villageGrowthCap == 0f ? 0f : 1 / SubModule.Settings.townGrowthCap;
        private readonly float _castleCoeff = SubModule.Settings.villageGrowthCap == 0f ? 0f : 1 / SubModule.Settings.castleGrowthCap;

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
                float loss = _hearthCoeff * village.Hearth * village.Hearth;
                float population = (newBorn - loss) * _hearthMultiplier;
                if (population > 0)
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _newBornText);
                }
                else
                {
                    explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _populationLossText);
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
                explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _newBornText);
            }
            else
            {
                explainedNumber.Add(population * SubModule.Settings.prosperityGrowthMultiplier, _populationLossText);
            }
            enemyAround = enemyRatio * newBorn;
            explainedNumber.Add(-enemyAround * SubModule.Settings.prosperityGrowthMultiplier, _enemyText);
            float starving = !fortification.Owner.IsStarving || foodStocksChange >= 0.0f ? 0f : foodStocksChange;
            if (starving < 0)
            {
                explainedNumber.Add(starving * SubModule.Settings.prosperityGrowthMultiplier, _foodShortageText);
            }          
            if (foodWorries > 0)
            {
                explainedNumber.Add(-foodWorries * SubModule.Settings.prosperityGrowthMultiplier, _foodWorriesText);
            }

            if (fortification.Settlement.Culture.ProsperityBonus > 0)
            {
                float bonus = (float)fortification.Settlement.Culture.ProsperityBonus * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, this._empireProsperityBonus);
            }              

            if (fortification.IsTown)
            {
                int num4 = fortification.SoldItems.Sum<Town.SellLog>((Func<Town.SellLog, int>)(x => x.Category.Properties != ItemCategory.Property.BonusToProsperity ? 0 : x.Number));
                if (num4 > 0)
                {
                    float bonus = (float)num4 * 2.0f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, this._prosperityFromMarketText);
                }             
            }

            float oldNumber = explainedNumber.ResultNumber;
            PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PristineStreets, fortification, ref explainedNumber);
            float newNumber = explainedNumber.ResultNumber;
            float diff = newNumber - oldNumber;
            if (diff !=0 && explainedNumber.Explainer != null)
            {
                explainedNumber.Explainer.Lines.Last().Number *= _vanillaToRatio * newBorn;
            }
            float resultDiff = diff * _vanillaToRatio * newBorn + oldNumber - newNumber;
            
            foreach (Building building in fortification.Buildings)
            {
                int buildingEffectAmount = building.GetBuildingEffectAmount(DefaultBuildingEffects.Prosperity);
                if ((!building.BuildingType.IsDefaultProject || fortification.CurrentBuilding == building) && buildingEffectAmount > 0)
                {
                    float bonus = (float)buildingEffectAmount * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, building.Name);
                }
                    
                if (building.BuildingType == DefaultBuildingTypes.SettlementAquaducts)
                {
                    oldNumber = explainedNumber.ResultNumber;
                    PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.CleanInfrastructure, fortification, ref explainedNumber);
                    newNumber = explainedNumber.ResultNumber;
                    diff = newNumber - oldNumber;
                    if (diff != 0 && explainedNumber.Explainer != null)
                    {
                        explainedNumber.Explainer.Lines.Last().Number *= _vanillaToRatio * newBorn;
                    }
                    resultDiff += diff * _vanillaToRatio * newBorn + oldNumber - newNumber;
                }
                    
            }
            if ((double)fortification.Loyalty > 75.0)
            {
                float bonus = Campaign.Current.Models.SettlementLoyaltyModel.HighLoyaltyProsperityEffect * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, this._loyaltyText);
            }
                
            else if ((double)fortification.Loyalty <= 50.0)
            {
                float bonus = Campaign.Current.Models.SettlementLoyaltyModel.LowLoyaltyProsperityEffect * _vanillaToRatio * newBorn;
                explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, this._loyaltyText);
            }
                
            if (fortification.Settlement.OwnerClan.Kingdom != null)
            {
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.Serfdom.Name);
                }                   
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoadTolls))
                {
                    float bonus = -0.2f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.RoadTolls.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.RulingClan == fortification.Settlement.OwnerClan && fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.ImperialTowns))
                {
                    float bonus = 1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.ImperialTowns.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.CrownDuty.Name);
                }
                if (fortification.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax))
                {
                    float bonus = -1f * _vanillaToRatio * newBorn;
                    explainedNumber.Add(bonus * SubModule.Settings.prosperityGrowthMultiplier, DefaultPolicies.WarTax.Name);
                }
            }

            oldNumber = explainedNumber.ResultNumber;
            this.GetSettlementProsperityChangeDueToIssues(fortification.Settlement, ref explainedNumber);
            newNumber = explainedNumber.ResultNumber;
            diff = newNumber - oldNumber;
            if (diff != 0 && explainedNumber.Explainer != null)
            {
                explainedNumber.Explainer.Lines.Last().Number *= _vanillaToRatio * newBorn;
            }
            resultDiff += diff * _vanillaToRatio * newBorn + oldNumber - newNumber;

            return explainedNumber.ResultNumber + resultDiff;
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
