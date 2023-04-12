using Demo_Models;
using Demo_Models.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo_Services.Services.RemoteAssetServices
{
    public class RemoteAssetService : IRemoteAssetService
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly SystemConfigs systemConfigs;

        public RemoteAssetService(
            HttpClient httpClient,
            SystemConfigs systemConfigs,
            ILogger logger)
        {
            this.httpClient = httpClient;
            this.systemConfigs = systemConfigs;
            this.logger = logger;
        }

        /// <summary>
        ///  Call an Api from url and return result as string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("GetAsync failed!", ex);
                throw new HttpRequestException(ex.Message);
            }
        }

        /// <summary>
        ///  Returns a list of asset
        /// </summary>
        /// <returns>List<Asset></returns>
        public async Task<List<Asset>> GetRemoteAssetListAsync()
        {
            try
            {
                var response = await GetAsync(systemConfigs.TargetAssetApiUrl);
                return JsonConvert.DeserializeObject<List<Asset>>(response);
            }
            catch
            {
                logger.LogError("GetRemoteAssetListAsync failed!");
                throw new HttpRequestException("GetRemoteAssetListAsync failed!");
            }
        }
    }
}