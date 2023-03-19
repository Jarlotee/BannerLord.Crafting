using System;
using System.Collections.Generic;
using System.Linq;
using BannerLord.Crafting.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerLord.Crafting.Behaviors
{
    public class CraftingResearchCampaignBehavior : CampaignBehaviorBase
    {
        protected Dictionary<CraftingPiece, float> _openNewPartUnlockDictionary = new Dictionary<CraftingPiece, float>();
        protected Dictionary<CraftingTemplate, float> _openNewPartByTemplateUnlockDictionary = new Dictionary<CraftingTemplate, float>();

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SaveAsJson<Dictionary<CraftingPiece, float>>("_openNewPartUnlockDictionary", ref this._openNewPartUnlockDictionary);
            dataStore.SaveAsJson<Dictionary<CraftingTemplate, float>>("_openNewPartByTemplateUnlockDictionary", ref this._openNewPartByTemplateUnlockDictionary);
        }

        public float GetUnlockPercentForCraftingPiece(CraftingPiece piece)
        {
            return _openNewPartUnlockDictionary.ContainsKey(piece) ? _openNewPartUnlockDictionary[piece] : 0;
        }

        public void SetUnlockPercentForCraftingPiece(CraftingPiece piece, float percent)
        {
            if (!_openNewPartUnlockDictionary.ContainsKey(piece))
            {
                _openNewPartUnlockDictionary[piece] = 0;
            }

            _openNewPartUnlockDictionary[piece] = percent;
        }

        public float GetUnlockPercentForTemplate(CraftingTemplate template)
        {
            return _openNewPartByTemplateUnlockDictionary.ContainsKey(template) ? _openNewPartByTemplateUnlockDictionary[template] : 0;
        }

        public void SetUnlockPercentForTemplate(CraftingTemplate template, float percent)
        {
            if (!_openNewPartByTemplateUnlockDictionary.ContainsKey(template))
            {
                _openNewPartByTemplateUnlockDictionary[template] = 0;
            }

            _openNewPartByTemplateUnlockDictionary[template] = percent;
        }

        public override void RegisterEvents() { }

        public static void HandleResearchForSmelting(
            Hero hero,
            EquipmentElement equipmentElement,
            Func<CraftingPiece, CraftingTemplate, bool> IsPartUnlocked,
            Action<CraftingPiece, CraftingTemplate> UnlockPart)
        {
            var craftingResearchCampaignBehavior = Campaign.Current.GetCampaignBehavior<CraftingResearchCampaignBehavior>();
            var craftingSkill = hero.GetSkillValue(DefaultSkills.Crafting);

            if (hero.GetPerkValue(DefaultPerks.Crafting.CuriousSmelter))
            {
                craftingSkill += 50;
            }

            equipmentElement.Item.WeaponDesign.UsedPieces.Where(p => p.IsValid).ToMBList().ForEach(element =>
            {
                var partIsOpen = IsPartUnlocked(element.CraftingPiece, equipmentElement.Item.WeaponDesign.Template);

                if (!partIsOpen)
                {
                    var existingUnlockChange = craftingResearchCampaignBehavior.GetUnlockPercentForCraftingPiece(element.CraftingPiece);
                    var partDifficulty = Math.Max(Campaign.Current.Models.SmithingModel.GetCraftingPartDifficulty(element.CraftingPiece), 1);
                    var weightedUnlockModifier = craftingSkill / partDifficulty;
                    var weightedUnlockChange = Configuration.SmeltingBaseUnlockChance * weightedUnlockModifier + existingUnlockChange;
                    var partUnlocked = MBRandom.RandomInt(1, 100) <= weightedUnlockChange * 100;

                    if (partUnlocked)
                    {
                        UnlockPart(element.CraftingPiece, equipmentElement.Item.WeaponDesign.Template);
                    }
                    else
                    {
                        craftingResearchCampaignBehavior.SetUnlockPercentForCraftingPiece(element.CraftingPiece, weightedUnlockChange);
                    }
                }
            });

            CraftingResearchCampaignBehavior.HandleResearchForCrafting(hero, equipmentElement.Item.WeaponDesign, IsPartUnlocked, UnlockPart);
        }

        public static void HandleResearchForCrafting(
            Hero hero,
            WeaponDesign design,
            Func<CraftingPiece, CraftingTemplate, bool> IsPartUnlocked,
            Action<CraftingPiece, CraftingTemplate> UnlockPart)
        {
            var craftingResearchCampaignBehavior = Campaign.Current.GetCampaignBehavior<CraftingResearchCampaignBehavior>();
            var existingUnlockChange = craftingResearchCampaignBehavior.GetUnlockPercentForTemplate(design.Template);

            var lockedParts = design.Template.Pieces
                .Where(part => !part.IsHiddenOnDesigner)
                .Where(part => !IsPartUnlocked(part, design.Template))
                .OrderBy(part => part.PieceTier)
                .ToMBList();

            if (lockedParts.Count < 1) return;

            var nextLockedPart = lockedParts.First();

            var partDifficulty = Math.Max(Campaign.Current.Models.SmithingModel.GetCraftingPartDifficulty(nextLockedPart), 1);
            var craftingSkill = hero.GetSkillValue(DefaultSkills.Crafting);

            if (hero.GetPerkValue(DefaultPerks.Crafting.CuriousSmith))
            {
                craftingSkill += 50;
            }

            var weightedUnlockModifier = craftingSkill / partDifficulty;
            var weightedUnlockChange = Configuration.CraftingBaseUnlockChance * weightedUnlockModifier + existingUnlockChange;
            var partUnlocked = MBRandom.RandomInt(1, 100) <= weightedUnlockChange * 100;

            if (partUnlocked)
            {
                var partToUnlock = lockedParts.Where(part => part.PieceTier == nextLockedPart.PieceTier).ToMBList().GetRandomElement();
                UnlockPart(partToUnlock, design.Template);
            }

            craftingResearchCampaignBehavior.SetUnlockPercentForTemplate(design.Template, partUnlocked ? 0 : weightedUnlockChange);
        }
    }
}