namespace Demo_Tests;
using Demo_Services.Services.AssetServices;
using Demo_Services.Services.DateTimeServices;
using Demo_Services.Services.RemoteAssetServices;
using Moq;
using Demo_Models;

public class AnalyzeTargetAssetTest
{
    public static readonly List<Asset> AssetList =
        new List<Asset>
                        {
                            new Asset(1, 2, "Running"),
                            new Asset(2, null, "Running"),
                            new Asset(3, 1, "Stopped"),
                            new Asset(4, 6, "MigrationFailed"),
                            new Asset(5, 4, "Unknown"),
                            null,
                            new Asset(6, 5, "MigrationFailed")
                        };

    AssetService assetService = new AssetService(null, null, null);

    [Fact]
    public async Task TestTargetAsset()
    {
        var dateTimeService = new Mock<IDateTimeService>();
        var remoteAssetService = new Mock<IRemoteAssetService>();
        var assetService = new AssetService(remoteAssetService.Object, dateTimeService.Object, null);

        dateTimeService.Setup(p => p.GetNow()).Returns(new DateTime(2023, 02, 03));
        remoteAssetService.Setup(p => p.GetRemoteAssetListAsync()).Returns(Task.FromResult(AssetList));
        
        var result = await assetService.AnalyzeTargetAssetAsync();

        Assert.Equal(AssetList.Count, result.Count);
        Assert.Equal(2, AssetList[0].ParentTargetAssetCount);
        Assert.Equal(1, AssetList[1].ParentTargetAssetCount);
        Assert.Equal(3, AssetList[2].ParentTargetAssetCount);
        Assert.Equal(3, AssetList[3].ParentTargetAssetCount);
        Assert.Equal(3, AssetList[4].ParentTargetAssetCount);
        Assert.Null(AssetList[5]);
        Assert.Equal(3, AssetList[4].ParentTargetAssetCount);
    }
}