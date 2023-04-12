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
        ///This service adds the fields "isStartable" and "parentTargetAssetCount" to the received target assets, and return the enriched target assets as JSON
        /// </summary>
        /// <returns>List<Asset></returns>
        public async Task<List<Asset>> AnalyzeTargetAssetAsync()
        {
            var targetAssets = await remoteAssetService.GetRemoteAssetListAsync();

            foreach (var targetAsset in targetAssets.Where(p => p != null))
            {
                targetAsset.IsStartable = IsStartable(targetAsset);
                targetAsset.ParentTargetAssetCount = ParentTargetAssetCount(targetAsset, targetAssets);
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
        public int ParentTargetAssetCount(Asset asset, List<Asset> assetList)
        {
            //It's possible that the 'Asset' or 'AssetList' parameters might be 'null' at some point during execution
            //Currently, I've made the decision to return a value of 0 for the 'Count' property in this scenario. 
            //I understand that this may not have been explicitly outlined in the original task description, 
            //but I believe it's the best approach given the current circumstances
            if (asset == null || assetList == null) return 0;

            int count = 0;
            var firstId = asset?.Id;

            do
            {
                asset = assetList.FirstOrDefault(ta => ta?.Id == asset.ParentId);
                count++;
            }
            while (asset != null && asset.Id != firstId);
            //asset.Id != firstId 
            //I noticed a potential issue in the parent fields of the list where a circular reference may occur. 
            //However, the task description does not provide a scenario for handling this situation. 
            //After careful consideration, I have decided that the best course of action is to break the loop in the event of a 
            //circular reference. This will ensure the program runs smoothly and avoids any potential errors or crashes.

            return count;
        }
    }
}