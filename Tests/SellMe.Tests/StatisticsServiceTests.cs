﻿namespace SellMe.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Moq;
    using Services;
    using Services.Interfaces;
    using Xunit;

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
            var moqPromotionsService = new Mock<IPromotionsService>();

            var context = InitializeContext.CreateContextForInMemory();
            statisticsService = new StatisticsService(context, moqAdsService.Object, moqUsersService.Object, moqPromotionsService.Object);

            //Act
            var actual = await statisticsService.GetAdministrationIndexStatisticViewModel();

            //Assert
            Assert.Equal(expectedActiveAdsCount, actual.AllAdsCount);
            Assert.Equal(expectedAllUsersCount, actual.AllUsersCount);
        }

        [Fact]
        public async Task GetPointsForCreatedAdsAsync_WithValidData_ShouldReturnCorrectCountOfDataPoints()
        {
            //Assert
            var expected = 10;

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync())
                .ReturnsAsync(new List<int> { 1, 0, 1, 0, 2, 0, 0, 0, 1, 1 });

            var moqUsersService = new Mock<IUsersService>();
            var moqPromotionsService = new Mock<IPromotionsService>();
            var context = InitializeContext.CreateContextForInMemory();

            statisticsService = new StatisticsService(context, moqAdsService.Object, moqUsersService.Object, moqPromotionsService.Object);

            //Act
            var actual = await statisticsService.GetPointsForCreatedAdsAsync();

            //Assert
            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public async Task GetPointsForPromotionsAsync_WithValidData_ShouldReturnCorrectCountOfDataPoints()
        {
            //Assert
            var expected = 10;

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqPromotionsService = new Mock<IPromotionsService>();
            moqPromotionsService.Setup(x => x.GetTheCountOfPromotionsForTheLastTenDaysAsync())
                .ReturnsAsync(new List<int> { 1, 0, 1, 0, 2, 0, 0, 0, 1, 1 });
            var context = InitializeContext.CreateContextForInMemory();

            statisticsService = new StatisticsService(context, moqAdsService.Object, moqUsersService.Object, moqPromotionsService.Object);

            //Act
            var actual = await statisticsService.GetPointsForPromotionsAsync();

            //Assert
            Assert.Equal(expected, actual.Count());
        }
    }
}
