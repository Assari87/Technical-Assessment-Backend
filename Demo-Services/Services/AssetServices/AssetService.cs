using Microsoft.Extensions.Logging;
using Demo_Models;
using Demo_Services.Services.DateTimeServices;
using Demo_Services.Services.RemoteAssetServices;

namespace Demo_Services.Services.AssetServices
{
    public class AssetService : IAssetService
    {
        public const string STARTABLE_ASSET_STATUS = "Running";
        public const int STARTABLE_ASSET_DAY_NUMBER = 3;

        private readonly IRemoteAssetService remoteAssetService;
        private readonly IDateTimeService dateTimeService;
        private readonly ILogger logger;

        public AssetService(
            IRemoteAssetService remoteAssetService,
            IDateTimeService dateTimeService,
            ILogger logger)
        {
            this.remoteAssetService = remoteAssetService;
            this.dateTimeService = dateTimeService;
            this.logger = logger;
        }

        /// <summary>
        ///  Analyze Target Asset
        ///  This service adds the fields "isStartable" and "parentTargetAssetCount" to the received target assets, and return the enriched target assets as JSON
        /// </summary>
        /// <returns>List<Asset></returns>
        public async Task<List<Asset>> AnalyzeTargetAssetAsync()
        {
            var targetAssets = await remoteAssetService.GetRemoteAssetListAsync();

            var nodeDic = targetAssets.Where(p => p != null).ToDictionary(p => p.Id, p => p.ParentId);

            foreach (var targetAsset in targetAssets.Where(p => p != null))
            {
                targetAsset.IsStartable = IsStartable(targetAsset);
                targetAsset.ParentTargetAssetCount = ParentTargetAssetCount(targetAsset, nodeDic);
            }

            return targetAssets.ToList();
        }

        /// <summary>
        /// Check IsStartable
        /// The ```isStartable``` field should be ```true```&nbsp;if, according to the current date (local system time of the Demo-API), 
        /// it is the third day of the month and the ```status``` field of the target asset is ```Running```. Otherwise, ```isStartable``` 
        /// should be ```false```
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <returns>bool</returns>
        public bool IsStartable(Asset asset)
        {
            return asset.Status?.ToLower() == STARTABLE_ASSET_STATUS.ToLower() && dateTimeService.GetNow().Day == STARTABLE_ASSET_DAY_NUMBER;
        }

        /// <summary>
        /// Calculate Parent Target Asset Count
        /// The ```parentTargetAssetCount``` field should contain the number of parent target assets that can be determined by 
        /// the ```parentId``` field of each target asset. For example, if a target asset named "TestTargetAsset" has 
        /// a ```parentId``` of ```5```, the target asset with ID 5 has a ```parentId``` of ```1```, and the target asset with ID ```1``` 
        /// has a ```parentId``` of ```null```, then the ```parentTargetAssetCount``` field of "TestTargetAsset" should be ```3```. 
        /// The code for determining this should be written by yourself and should not come from a library.
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="assetList">List<Asset></param>
        /// <returns>bool</returns>
        public int ParentTargetAssetCount(Asset asset, Dictionary<int, int?> nodeDic)
        {
            if (asset == null || nodeDic == null) return 0;

            int count = 1;
            var firstId = asset?.Id;
            int? parentId = asset.ParentId;
            if (parentId == null) return count;
            do
            {
                parentId = nodeDic.TryGetValue(parentId.Value, out var _parentId) ? _parentId : null;
                count++;
            }
            while (parentId != null && parentId != firstId);

            return count;
        }
    }
}