using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BannerLord.Crafting
{
    public static class Configuration
    {
        private static readonly string _modBasePath =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
        private static readonly string _configFilePath =
            Path.Combine(_modBasePath, "ModuleData", "Config.json");

        static Configuration()
        {
            try
            {
                var document = JObject.Parse(File.ReadAllText(_configFilePath));

                EnableTravelingCraftingStaminaRecovery = document.GetValue("enableTravelingCraftingStaminaRecovery").Value<bool>();
                EnableCraftingResearchBehavior = document.GetValue("enableCraftingResearchBehavior").Value<bool>();
                EnableImprovedCraftingXpMultipliers = document.GetValue("enableImprovedCraftingXpMultipliers").Value<bool>();

                TravelingCraftingStaminaRecoveryMultiplier = document.GetValue("travelingCraftingStaminaRecoveryMultiplier").Value<float>();
                SmeltingBaseUnlockChance = document.GetValue("smeltingBaseUnlockChance").Value<float>();
                CraftingBaseUnlockChance = document.GetValue("craftingBaseUnlockChance").Value<float>();
                RefiningXpMultiplier = document.GetValue("refiningXpMultiplier").Value<float>();
                SmeltingXpMultiplier = document.GetValue("smeltingXpMultiplier").Value<float>();
                FreeBuildCraftingXpMultiplier = document.GetValue("freeBuildCraftingXpMultiplier").Value<float>();
            }
            catch (System.Exception ex)
            {
                Trace.TraceWarning($"Failed to load BannerLord.Crafting from {_configFilePath} {ex.Message}");
            }

        }

        public static bool EnableTravelingCraftingStaminaRecovery { get; private set; } = true;
        public static bool EnableCraftingResearchBehavior { get; private set; } = true;
        public static bool EnableImprovedCraftingXpMultipliers { get; private set; } = true;
        public static float TravelingCraftingStaminaRecoveryMultiplier { get; private set; } = 0.5f;
        public static float SmeltingBaseUnlockChance { get; private set; } = 0.2f;
        public static float CraftingBaseUnlockChance { get; private set; } = 0.05f;
        public static float RefiningXpMultiplier { get; private set; } = 0.1f;
        public static float SmeltingXpMultiplier { get; private set; } = 0.05f;
        public static float FreeBuildCraftingXpMultiplier { get; private set; } = 0.05f;
    }
}