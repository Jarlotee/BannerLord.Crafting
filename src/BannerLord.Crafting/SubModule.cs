using BannerLord.Crafting.Behaviors;
using BannerLord.Crafting.Models;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerLord.Crafting
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            if (game.GameType is Campaign)
            {
                var campaignStarter = gameStarter as CampaignGameStarter;
                if (Configuration.EnableTravelingCraftingStaminaRecovery)
                {
                    campaignStarter.AddBehavior(new TravelingCraftingStaminaRecoveryCampaignBehavior());
                }
                if (Configuration.EnableCraftingResearchBehavior)
                {
                    campaignStarter.AddBehavior(new CraftingResearchCampaignBehavior());
                }
                if (Configuration.EnableImprovedCraftingXpMultipliers)
                {
                    campaignStarter.AddModel(new ImprovedBaseSmithingModel());
                }
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("BannerLord.Crafting").PatchAll();
        }

    }
}