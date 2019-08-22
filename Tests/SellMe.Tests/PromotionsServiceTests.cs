namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using SellMe.Data.Models;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.BindingModels.Promotions;
    using SellMe.Web.ViewModels.ViewModels.Promotions;
    using Xunit;
    using SellMe.Tests.Common;

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

            this.promotionsService = new PromotionsService(context, moqAdsService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.promotionsService.GetPromotionBindingModelByAdIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }


        [Fact]
        public async Task GetPromotionBindingModelByAdIdAsync_WithValidData_ShouldReturnCorectPromotionBindingModel()
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
                        Type = "silver",
                        Updates = 3
                    },
                    new PromotionViewModel
                    {
                        ActiveDays = 30,
                        Price = 8.00M,
                        Type = "gold",
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

            this.promotionsService = new PromotionsService(context, moqAdsService.Object);
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
                    EmailAddress = "Ivan@gmail.com",
                },
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
            var actual = await this.promotionsService.GetPromotionBindingModelByAdIdAsync(1);

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

            this.promotionsService = new PromotionsService(context, moqAdsService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.promotionsService.CreatePromotionOrderAsync(1, 1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreatePromotionOrderAsync_WithInvalidPromotionId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Promotion with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var context = InitializeContext.CreateContextForInMemory();

            this.promotionsService = new PromotionsService(context, moqAdsService.Object);

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
                    EmailAddress = "Ivan@gmail.com",
                },
            };
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.promotionsService.CreatePromotionOrderAsync(1, 1));
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
            this.promotionsService = new PromotionsService(context, moqAdsService.Object);

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
                    EmailAddress = "Ivan@gmail.com",
                },
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
            await this.promotionsService.CreatePromotionOrderAsync(1, 1);

            //Assert
            Assert.True(context.PromotionOrders.Count() == 1);
        }
    }
}
