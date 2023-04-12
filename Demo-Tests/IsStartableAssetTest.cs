namespace Demo_Tests;
using Demo_Services.Services.AssetServices;
using Demo_Services.Services.DateTimeServices;
using Demo_Services.Services.RemoteAssetServices;
using Moq;
using Demo_Models;

public class IsStartableAssetTest
{
    public static readonly List<Asset> AssetList =
        new List<Asset>
                        {
                            new Asset(1, 2, "Running"),
                            new Asset(2, null, "Running"),
                            new Asset(3,1,"Stopped"),
                            new Asset(4, 6, "MigrationFailed"),
                            new Asset(5,4,"Unknown"),
                            null,
                            new Asset(6,5,"MigrationFailed")
                        };

    [Theory]
    [InlineData(3, "Running", true)]
    [InlineData(3, "running", true)]
    [InlineData(4, "Running", false)]
    [InlineData(3, "Stopped", false)]
    [InlineData(4, "Stopped", false)]
    [InlineData(3, "", false)]
    [InlineData(3, null, false)]
    public void TestIsStartableAsset(int day, string status, bool expected)
    {
        var dateTimeService = new Mock<IDateTimeService>();
        dateTimeService.Setup(p => p.GetNow()).Returns(new DateTime(2023, 02, day));

        var asset = new Asset(2, 1, status);
        var assetService = new AssetService(null, dateTimeService.Object, null);

        var result = assetService.IsStartable(asset);
        Assert.Equal(expected, result);
    }

}