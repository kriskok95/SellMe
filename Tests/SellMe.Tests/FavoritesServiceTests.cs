namespace SellMe.Tests
{
    using System;
    using Moq;
    using System.Collections.Generic;
    using Xunit;
    using System.Threading.Tasks;

    using SellMe.Data;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using SellMe.Tests.Common;
    using SellMe.Data.Models;
    

    public class FavoritesServiceTests
    {
        private IFavoritesService favoritesService;

        public FavoritesServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithCurrentUserEqualsToNull_ShouldThrowAndInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "Current user can't be null";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();

            this.favoritesService = new FavoritesService(context, moqUserService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this.favoritesService.AddToFavoritesAsync(1));

            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.favoritesService.AddToFavoritesAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithValidData_ShouldReturnTrue()
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);
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

            //Act
            var actual = await favoritesService.AddToFavoritesAsync(1);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithAdAlreadyInFavorites_ShouldThrowAnInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "The given ad is already added to favorites!";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                        }
                    }
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);
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
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this.favoritesService.AddToFavoritesAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task RemoveFromFavoritesAsync_WithCurrentUserEqualsToNull_ShouldThrowAndInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "Current user can't be null";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();

            this.favoritesService = new FavoritesService(context, moqUserService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this.favoritesService.RemoveFromFavoritesAsync(1));

            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task RemoveFromFavoritesAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.favoritesService.RemoveFromFavoritesAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }


        [Fact]
        public async Task RemoveFromFavoritesAsync_WithExistingAdInFavorites_ShouldReturnTrue()
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                        }
                    }
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);
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
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com",
                },
            };

            var sellMeUserFavoriteProduct = new SellMeUserFavoriteProduct
            {
                AdId = 1,
                SellMeUserId = moqUserService.Object.GetCurrentUserAsync().Result.Id
            };

            await context.SellMeUserFavoriteProducts.AddAsync(sellMeUserFavoriteProduct);
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.favoritesService.RemoveFromFavoritesAsync(1);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task RemoveFromFavoritesAsync_WithoutAdInTheFavoritesList_ShouldThrowAndInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "The given ad isn't in the favorites list!";
            var context = InitializeContext.CreateContextForInMemory();

            var moqUserService = new Mock<IUsersService>();
            moqUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ivan",
                });

            this.favoritesService = new FavoritesService(context, moqUserService.Object);
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
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this.favoritesService.RemoveFromFavoritesAsync(1));

            Assert.Equal(expectedErrorMessage, ex.Message);
        }
    }
}
