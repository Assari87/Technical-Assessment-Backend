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
        public AssetController(IAssetService assetService)
        {
            this.assetService = assetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TargetAsset>>> Get()
        {
            try
            {
                var result = await assetService.AnalyzeTargetAssetAsync();
                return Ok(result);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }
    }
}
