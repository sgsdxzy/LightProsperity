﻿using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;


namespace LightProsperity
{
    [HarmonyPatch(typeof(DefaultVolunteerProductionModel), "GetDailyVolunteerProductionProbability")]
    public class GetDailyVolunteerProductionProbabilityPatch
    {
        public static void Postfix(ref float __result,
            Hero hero,
            int index,
            Settlement settlement)
        {
            if (settlement.IsTown)
            {
                double multiplier = (settlement.Prosperity - SubModule.Settings.TownMinProsperityForRecruit) /
                    (SubModule.Settings.TownProsperityThreshold - SubModule.Settings.TownMinProsperityForRecruit);
                multiplier = Math.Max(multiplier, 0);
                multiplier = Math.Pow(multiplier, 0.7);
                __result *= (float)multiplier;
            }
            if (settlement.IsVillage)
            {
                double multiplier = (settlement.Village.Hearth - SubModule.Settings.VillageMinProsperityForRecruit) /
                    (SubModule.Settings.VillageProsperityThreshold - SubModule.Settings.VillageMinProsperityForRecruit);
                multiplier = Math.Max(multiplier, 0);
                multiplier = Math.Pow(multiplier, 0.7);
                __result *= (float)multiplier;
            }
        }
    }
}
