using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerLord.Crafting
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            if(game.GameType is Campaign) 
            {
                var campaignStarter = gameStarter as CampaignGameStarter;
                campaignStarter.AddBehavior(new OutOfSettlementSmithingRecoveryCampaignBehavior());
            }
        }
    }
}