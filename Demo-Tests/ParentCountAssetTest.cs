namespace Demo_Tests;
using Demo_Services.Services.AssetServices;
using Demo_Models;

public class ParentCountAssetTest
{
    AssetService assetService = new AssetService(null, null, null);

    Dictionary<int, int?> GetDictionary(List<Asset> assetList)
    {
        return assetList?.Where(p=>p != null).ToDictionary(p => p.Id, p => p.ParentId);
    }

    [Fact]
    public void TestTargetAssetCount()
    {
        var assetList = new List<Asset>
                        {
                            new Asset(1, null, "Running"),
                            new Asset(2, 1, "Stopped")
                        };
        var count = assetService.ParentTargetAssetCount(assetList[0], GetDictionary(assetList));
        Assert.Equal(1, count);
    }

    [Fact]
    public void Test_Asset_Without_Parent()
    {
        var assetList = new List<Asset>
                        {
                            new Asset(1, null),
                        };
        var count = assetService.ParentTargetAssetCount(assetList[0], GetDictionary(assetList));
        Assert.Equal(1, count);
    }

    [Fact]
    public void Test_Handle_Null_In_List()
    {
        var assetList = new List<Asset>
                        {
                            new Asset(1, null),
                            null
                        };
        var count = assetService.ParentTargetAssetCount(assetList[0], GetDictionary(assetList));
        Assert.Equal(1, count);
    }

    [Fact]
    public void Test_Handle_Null_Parameter()
    {
        var assetList = new List<Asset>
                        {
                            new Asset(1, null),
                            null
                        };
        var count = assetService.ParentTargetAssetCount(null, GetDictionary(assetList));
        Assert.Equal(0, count);
    }

    [Fact]
    public void Test_Handle_Null_List()
    {
        var count = assetService.ParentTargetAssetCount(new Asset(1, null), GetDictionary(null));
        Assert.Equal(0, count);
    }
}