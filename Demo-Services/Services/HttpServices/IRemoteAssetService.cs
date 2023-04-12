using Demo_Models;

namespace Demo_Services.Services.RemoteAssetServices
{
    public interface IRemoteAssetService
    {
        Task<List<Asset>> GetRemoteAssetListAsync();
    }
}