using Demo_Models;
using Demo_Services;
using Demo_Services.Services.AssetServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService assetService;
        private readonly ILogger logger;
        public AssetController(
            IAssetService assetService,
            ILogger logger)
        {
            this.assetService = assetService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> TargetAsset()
        {
            try
            {
                var result = await assetService.AnalyzeTargetAssetAsync();

                logger.LogInformation("Asset Get service has been completed successfully");
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.LogError("Asset Get service failed",ex);
                return BadRequest("Asset Get service failed");
            }

        }
    }
}
