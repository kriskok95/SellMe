namespace SellMe.Tests
{
    using System.Threading.Tasks;
    using Moq;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using SellMe.Tests.Common;
    using Xunit;
    using System.Collections.Generic;
    using System.Linq;

    public class StatisticsServiceTests
    {
        private IStatisticsService statisticsService;

        public StatisticsServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetAdministrationIndexStatisticViewModel_WithValidData_ShouldReturnCorrectResult()
        {
            //Assert
            var expectedActiveAdsCount = 10;
            var expectedAllUsersCount = 5;

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetAllActiveAdsCountAsync())
                .ReturnsAsync(10);
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCountOfAllUsersAsync())
                .ReturnsAsync(5);

            var context = InitializeContext.CreateContextForInMemory();
            this.statisticsService = new StatisticsService(context, moqAdsService.Object, moqUsersService.Object);

            //Act
            var actual = await this.statisticsService.GetAdministrationIndexStatisticViewModel();

            //Assert
            Assert.Equal(expectedActiveAdsCount, actual.AllAdsCount);
            Assert.Equal(expectedAllUsersCount, actual.AllUsersCount);
        }

        [Fact]
        public async Task GetPointsForCreatedAds_WithValidData_ShouldReturnCorrectCountOfDataPoints()
        {
            //Assert
            var expected = 10;

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync())
                .ReturnsAsync(new List<int> { 1, 0, 1, 0, 2, 0, 0, 0, 1, 1 });

            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();

            this.statisticsService = new StatisticsService(context, moqAdsService.Object, moqUsersService.Object);

            //Act
            var actual = await this.statisticsService.GetPointsForCreatedAds();

            //Assert
            Assert.Equal(expected, actual.Count());
        }
    }
}
