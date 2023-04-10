using Demo_Models;

namespace Demo_Services.Services.AssetServices
{
    public interface IAssetService
    {
        Task<List<TargetAsset>> AnalyzeTargetAssetAsync();
    }
}