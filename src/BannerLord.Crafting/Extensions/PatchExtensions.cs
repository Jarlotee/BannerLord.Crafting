using System.Collections.Generic;
using System.Diagnostics;
using HarmonyLib;
using TaleWorlds.Library;

namespace BannerLord.Crafting.Extensions
{
    public static class PatchExtensions
    {
        public static IEnumerable<CodeInstruction> HandleTranspilerFailure(this IEnumerable<CodeInstruction> instructions, string message)
        {
            Trace.TraceWarning(message);
            InformationManager.DisplayMessage(new InformationMessage($"BannerLord.Crafting: {message}", Colors.Yellow));
            return instructions;
        }

    }
}