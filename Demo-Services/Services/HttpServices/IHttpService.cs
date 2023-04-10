namespace Demo_Services.Services.HttpServices
{
    public interface IHttpService
    {
        public Task<string> GetAsync(string url);
    }
}