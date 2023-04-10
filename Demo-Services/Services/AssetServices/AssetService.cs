using Demo_Models;
using Demo_Models.Models;
using Demo_Services.Services.HttpServices;
using Newtonsoft.Json;

namespace Demo_Services.Services.AssetServices
{
    public class AssetService : IAssetService
    {
        public const string STARTABLE_ASSET_STATUS = "Running";
        public const int STARTABLE_ASSET_DAY_NUMBER = 3;
        private readonly IHttpService httpService;
        private readonly SystemConfigs systemConfigs;

        public AssetService(IHttpService httpService, SystemConfigs systemConfigs)
        {
            this.httpService = httpService;
            this.systemConfigs = systemConfigs;
        }

        public async Task<List<TargetAsset>> AnalyzeTargetAssetAsync()
        {
            var response = await httpService.GetAsync(systemConfigs.TargetAssetApiUrl);
            var targetAssets = JsonConvert.DeserializeObject<List<TargetAsset>>(response);

            foreach (var targetAsset in targetAssets.Where(p => p != null))
            {
                targetAsset.IsStartable = IsStartable(targetAsset);
                targetAsset.ParentTargetAssetCount = ParentTargetAssetCount(targetAsset, targetAssets);
            }

            return targetAssets.ToList();
        }

        private static bool IsStartable(TargetAsset targetAsset)
        {
            return targetAsset.Status == STARTABLE_ASSET_STATUS && DateTime.Now.Day == STARTABLE_ASSET_DAY_NUMBER;
        }

        private static int ParentTargetAssetCount(TargetAsset targetAsset, IEnumerable<TargetAsset> targetAssets)
        {
            int count = 0;

            if (targetAsset.ParentId == null) return count;

            while (targetAsset.ParentId != null)
            {
                count++;
                targetAsset = targetAssets.FirstOrDefault(ta => ta.Id == targetAsset.ParentId);
            }

            return count + 1;
        }
    }
}