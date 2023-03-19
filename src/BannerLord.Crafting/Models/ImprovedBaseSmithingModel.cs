using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerLord.Crafting.Models
{
    public class ImprovedBaseSmithingModel : DefaultSmithingModel
    {
        public override int GetSkillXpForRefining(ref TaleWorlds.Core.Crafting.RefiningFormula refineFormula) => 
            MathF.Round(Configuration.RefiningXpMultiplier * (float) (this.GetCraftingMaterialItem(refineFormula.Output).Value * refineFormula.OutputCount));
        public override int GetSkillXpForSmelting(ItemObject item) => MathF.Round(Configuration.SmeltingXpMultiplier * (float)item.Value);
        public override int GetSkillXpForSmithingInFreeBuildMode(ItemObject item) => MathF.Round(Configuration.FreeBuildCraftingXpMultiplier * (float)item.Value);
    }
}