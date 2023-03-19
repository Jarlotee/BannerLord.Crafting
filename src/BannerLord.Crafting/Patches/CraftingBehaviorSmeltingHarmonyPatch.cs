using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BannerLord.Crafting.Behaviors;
using BannerLord.Crafting.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;

namespace BannerLord.Crafting.Patches
{
    [HarmonyPatch(typeof(CraftingCampaignBehavior))]
    [HarmonyPatch("DoSmelting")]
    public static class CraftingBehaviorSmeltingHarmonyPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if(!Configuration.EnableCraftingResearchBehavior) return instructions;
            
            var instructionsList = instructions.ToList();

            if (AccessTools.DeclaredMethod(typeof(CraftingCampaignBehavior), "SetHeroCraftingStamina") is not { } setHeroCraftingStamina)
                return instructions.HandleTranspilerFailure("CraftingCampaignBehavior:SetHeroCraftingStamina not found");

            if (AccessTools.DeclaredMethod(typeof(CraftingCampaignBehavior), "AddResearchPoints") is not { } addResearchPoints)
                return instructions.HandleTranspilerFailure("CraftingCampaignBehavior:AddResearchPoints not found");

            if (AccessTools.DeclaredMethod(typeof(CraftingCampaignBehavior), "IsOpened") is not { } isOpen)
                return instructions.HandleTranspilerFailure("CraftingCampaignBehavior:IsOpened not found");

            if (AccessTools.DeclaredMethod(typeof(CraftingCampaignBehavior), "OpenPart") is not { } OpenPart)
                return instructions.HandleTranspilerFailure("CraftingCampaignBehavior:OpenPart not found");

            if (AccessTools.DeclaredMethod(typeof(CraftingResearchCampaignBehavior), nameof(CraftingResearchCampaignBehavior.HandleResearchForSmelting)) is not { } handleResearchForSmelting)
                return instructions.HandleTranspilerFailure("CraftingBehaviorPatch:HandleResearchForSmelting not found");

            var instructionsByIndex = instructionsList.Select((instruction, index) => new { instruction, index }).ToList();

            try
            {
                var lastInstructionIndex = instructionsByIndex
                    .Where(item => Equals(item.instruction.operand, setHeroCraftingStamina))
                    .First().index;

                var researchInstructionIndex = instructionsByIndex
                    .Where(item => Equals(item.instruction.operand, addResearchPoints))
                    .First().index;

                var newInstructions = new List<CodeInstruction>
                {
                    new(OpCodes.Ldarg_1),
                    new(OpCodes.Ldarg_2),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldftn, isOpen),
                    new(OpCodes.Newobj, typeof(Func<CraftingPiece, CraftingTemplate, bool>).GetConstructors().First()),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldftn, OpenPart),
                    new(OpCodes.Newobj, typeof(Action<CraftingPiece, CraftingTemplate>).GetConstructors().First()),
                    new(OpCodes.Call, handleResearchForSmelting)
                };

                instructionsList.RemoveRange(lastInstructionIndex + 1, researchInstructionIndex - lastInstructionIndex);
                instructionsList.InsertRange(lastInstructionIndex + 1, newInstructions);

            }
            catch (System.Exception ex)
            {
                return instructions.HandleTranspilerFailure($"Failed to patch instructions {ex.ToString()}");
            }

            return instructionsList;
        }
    }
}