using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerLord.Crafting
{
    public class OutOfSettlementSmithingRecoveryCampaignBehavior : CampaignBehaviorBase
    {
        private const float OUT_OF_SETTLEMENT_SMITHING_RECOVERY_MULTIPLIER = 0.5f;

        public override void SyncData(IDataStore dataStore)
        {
            // no-op
        }

        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener((object)this, new Action(this.HourlyTick));
        }

        private void HourlyTick()
        {
            HandleStaminaRecoveryOutsideOfSettlement(PartyBase.MainParty.LeaderHero);

            foreach (var companion in PartyBase.MainParty.LeaderHero.CompanionsInParty)
            {
                HandleStaminaRecoveryOutsideOfSettlement(companion);
            }
        }

        private void HandleStaminaRecoveryOutsideOfSettlement(Hero hero)
        {
            var baseCraftingBehavior = Campaign.Current.GetCampaignBehavior<CraftingCampaignBehavior>();

            var currentStamina = baseCraftingBehavior.GetHeroCraftingStamina(hero);
            var maxStamina = baseCraftingBehavior.GetMaxHeroCraftingStamina(hero);

            if (currentStamina < maxStamina && hero.CurrentSettlement == null)
            {
                var recovery = (int)Math.Round(this.GetStaminaHourlyRecoveryRate(hero) * OUT_OF_SETTLEMENT_SMITHING_RECOVERY_MULTIPLIER);
                baseCraftingBehavior.SetHeroCraftingStamina(
                    hero,
                    MathF.Min(maxStamina, currentStamina + recovery)
                );
            }
        }

        // De-compiled TaleWorlds.CampaignSystem 1.1.0
        private int GetStaminaHourlyRecoveryRate(Hero hero)
        {
            int hourlyRecoveryRate = 5 + MathF.Round((float)hero.GetSkillValue(DefaultSkills.Crafting) * 0.025f);
            if (hero.GetPerkValue(DefaultPerks.Athletics.Stamina))
                hourlyRecoveryRate += MathF.Round((float)hourlyRecoveryRate * DefaultPerks.Athletics.Stamina.PrimaryBonus);
            return hourlyRecoveryRate;
        }
    }
}