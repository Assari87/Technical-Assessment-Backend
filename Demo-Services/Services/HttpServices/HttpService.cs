using Demo_Models;
using System.Net.Http;
using System.Text.Json;

namespace Demo_Services.Services.HttpServices
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetAsync(string url)
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
            catch
            {
                throw new HttpRequestException();
            }
        }
    }
}