using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LightProsperity
{
    public class SubModule : MBSubModuleBase
    {
        private bool Patched = false;
        public static Settings Settings = Settings.Instance;

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
            if (SubModule.Settings.ModifyGarrisonConsumption)
            {
                gameStarter?.AddModel(new LightSettlementGarrisonModel());
            }
            if (SubModule.Settings.NewProsperityModel)
            {
                gameStarter?.AddModel(new LightSettlementProsperityModel());
            }
        }
    }
}
