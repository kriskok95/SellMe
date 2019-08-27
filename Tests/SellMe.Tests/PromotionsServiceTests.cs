namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Moq;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels.BindingModels.Promotions;
    using Web.ViewModels.ViewModels.Promotions;
    using Xunit;

    public class PromotionsServiceTests
    {
        private PromotionsService promotionsService;

        public PromotionsServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetPromotionBindingModelByAdIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                promotionsService.GetPromotionBindingModelByAdIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }


        [Fact]
        public async Task GetPromotionBindingModelByAdIdAsync_WithValidData_ShouldReturnCorrectPromotionBindingModel()
        {
            //Arrange
            var expected = new PromotionBindingModel
            {
                AdId = 1,
                AdTitle = "Iphone 6s",
                PromotionViewModels = new List<PromotionViewModel>
                {
                    new PromotionViewModel
                    {
                        ActiveDays = 10,
                        Price = 3.50M,
                        Type = "Silver",
                        Updates = 3
                    },
                    new PromotionViewModel
                    {
                        ActiveDays = 30,
                        Price = 8.00M,
                        Type = "Gold",
                        Updates = 10
                    }
                }
            };

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetAdByIdAsync(1))
                .ReturnsAsync(new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    AvailabilityCount = 1,
                    Price = 120,
                    Condition = new Condition { Name = "Brand New" },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com"
                    }
                });

            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);
            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Price = 120,
                Condition = new Condition { Name = "Brand New" },
                Address = new Address
                {
                    Id = 1,
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com"
                }
            };

            var testingPromotions = new List<Promotion>
            {
                new Promotion
                {
                    ActiveDays = 10,
                    Price = 3.50M,
                    Type = "silver",
                    Updates = 3
                },
                new Promotion
                {
                    ActiveDays = 30,
                    Price = 8.00M,
                    Type = "gold",
                    Updates = 10
                }
            };

            await context.Ads.AddAsync(testingAd);
            await context.Promotions.AddRangeAsync(testingPromotions);
            await context.SaveChangesAsync();

            //Act
            var actual = await promotionsService.GetPromotionBindingModelByAdIdAsync(1);

            Assert.Equal(expected.AdId, actual.AdId);
            Assert.Equal(expected.AdTitle, actual.AdTitle);
            Assert.Collection(actual.PromotionViewModels,
                elem1 =>
                {
                    Assert.Equal(expected.PromotionViewModels[0].ActiveDays, elem1.ActiveDays);
                    Assert.Equal(expected.PromotionViewModels[0].Price, elem1.Price);
                    Assert.Equal(expected.PromotionViewModels[0].Type, elem1.Type);
                    Assert.Equal(expected.PromotionViewModels[0].Updates, elem1.Updates);
                },
                elem2 =>
                {
                    Assert.Equal(expected.PromotionViewModels[1].ActiveDays, elem2.ActiveDays);
                    Assert.Equal(expected.PromotionViewModels[1].Price, elem2.Price);
                    Assert.Equal(expected.PromotionViewModels[1].Type, elem2.Type);
                    Assert.Equal(expected.PromotionViewModels[1].Updates, elem2.Updates);
                });

            Assert.Equal(expected.PromotionViewModels.Count, actual.PromotionViewModels.Count);
        }

        [Fact]
        public async Task CreatePromotionOrderAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                promotionsService.CreatePromotionOrderAsync(1, 1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreatePromotionOrderAsync_WithInvalidPromotionId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Promotion with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Price = 120,
                Condition = new Condition { Name = "Brand New" },
                Address = new Address
                {
                    Id = 1,
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com"
                }
            };
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                promotionsService.CreatePromotionOrderAsync(1, 1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreatePromotionOrderAsync_WithValidData_ShouldCreateAnPromotionOrder()
        {
            //Arrange
            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetAdByIdAsync(1))
                .ReturnsAsync(new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    AvailabilityCount = 1,
                    Price = 120,
                    Condition = new Condition { Name = "Brand New" },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com"
                    }
                });

            var context = InitializeContext.CreateContextForInMemory();
            promotionsService = new PromotionsService(context, moqAdsService.Object);

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Price = 120,
                Condition = new Condition { Name = "Brand New" },
                Address = new Address
                {
                    Id = 1,
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com"
                }
            };

            var testingPromotion = new Promotion
            {
                Id = 1,
                ActiveDays = 10,
                Price = 3.50M,
                Type = "silver",
                Updates = 3
            };

            await context.Ads.AddAsync(testingAd);
            await context.Promotions.AddAsync(testingPromotion);
            await context.SaveChangesAsync();

            //Act
            await promotionsService.CreatePromotionOrderAsync(1, 1);

            //Assert
            Assert.True(context.PromotionOrders.Count() == 1);
        }

        [Fact]
        public async Task GetTheCountOfPromotionsForTheLastTenDaysAsync_WithValidData_ShouldReturnCorrectCollectionLength()
        {
            //Arrange
            var expectedLength = 10;

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);

            //Act
            var actual = await promotionsService.GetTheCountOfPromotionsForTheLastTenDaysAsync();

            //Assert
            Assert.Equal(expectedLength, actual.Count);
        }

        [Fact]
        public async Task GetTheCountOfPromotionsForTheLastTenDaysAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new List<int> { 1, 0, 1, 0, 2, 0, 0, 0, 1, 1 };

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            promotionsService = new PromotionsService(context, moqAdsService.Object);

            var testingPromotions = new List<PromotionOrder>
            {
                new PromotionOrder {Id = 1, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new PromotionOrder {Id = 2, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new PromotionOrder {Id = 3, CreatedOn = DateTime.UtcNow.AddDays(-7)},
                new PromotionOrder {Id = 4, CreatedOn = DateTime.UtcNow.AddDays(-9)},
                new PromotionOrder {Id = 5, CreatedOn = DateTime.UtcNow.AddDays(-1)},
                new PromotionOrder {Id = 6, CreatedOn = DateTime.UtcNow.AddDays(-10)},
                new PromotionOrder {Id = 7, CreatedOn = DateTime.UtcNow.AddDays(-30)},
                new PromotionOrder {Id = 8, CreatedOn = DateTime.UtcNow}
            };

            await context.PromotionOrders.AddRangeAsync(testingPromotions);
            await context.SaveChangesAsync();

            //Act
            var actual = await promotionsService.GetTheCountOfPromotionsForTheLastTenDaysAsync();

            //Assert
            Assert.Equal(expected[0], actual[0]);
            Assert.Equal(expected[1], actual[1]);
            Assert.Equal(expected[2], actual[2]);
            Assert.Equal(expected[3], actual[3]);
            Assert.Equal(expected[4], actual[4]);
            Assert.Equal(expected[5], actual[5]);
            Assert.Equal(expected[6], actual[6]);
            Assert.Equal(expected[7], actual[7]);
            Assert.Equal(expected[8], actual[8]);
            Assert.Equal(expected[9], actual[9]);
        }
    }
}
