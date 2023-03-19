using Newtonsoft.Json;
using TaleWorlds.CampaignSystem;

namespace BannerLord.Crafting.Extensions
{
    public static class DataStoreExtensions
    {
        public static void SaveAsJson<T>(this IDataStore dataStore, string key, ref T source)
        {
            if (dataStore.IsLoading)
            {
                try
                {
                    var json = string.Empty;

                    dataStore.SyncData(key, ref json);
                    source = JsonConvert.DeserializeObject<T>(json);
                }
                catch (System.Exception) { }
            }

            if (dataStore.IsSaving)
            {
                var json = JsonConvert.SerializeObject(source);
                dataStore.SyncData(key, ref json);
            }
        }
    }
}