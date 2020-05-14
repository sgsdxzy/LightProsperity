using HarmonyLib;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LightProsperity
{
    public class SubModule : MBSubModuleBase
    {
        private bool Patched = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (!Patched)
            {
                var harmony = new Harmony("mod.bannerlord.lightprosperity");
                harmony.PatchAll();
                Patched = true;
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            AddModels(gameStarterObject as CampaignGameStarter);
        }

        private void AddModels(CampaignGameStarter gameStarter)
        {
            if (Settings.Instance.ModifyGarrisonConsumption)
            {
                gameStarter?.AddModel(new LightSettlementGarrisonModel());
            }
            if (Settings.Instance.NewProsperityModel)
            {
                gameStarter?.AddModel(new LightSettlementProsperityModel());
            }
        }
    }
}
