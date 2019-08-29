namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Services;
    using Services.Interfaces;
    using Xunit;

    public class UsersServiceTests
    {
        private IUsersService usersService;

        public UsersServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetAllUserViewModelsAsync_WithValidDAta_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 3;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = false,EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new SellMeUser{Id = "Id4", UserName = "Seller4", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-18)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.GetAllUserViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public async Task GetAllUserViewModelsAsync_WithValidDAta_ShouldReturnCorrectOrder()
        {
            //Arrange
            var expected = new List<string> { "Id3", "Id2", "Id1" };

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = false,EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = this.usersService.GetAllUserViewModelsAsync(1, 10).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.Equal(expected[0], actual[0].Id);
            Assert.Equal(expected[1], actual[1].Id);
            Assert.Equal(expected[2], actual[2].Id);
        }

        [Fact]
        public async Task BlockUserByIdAsync_WithInvalidUserId_ShouldThrowAndArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User with the given id doesn't exist!";

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            //Act and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.usersService.BlockUserByIdAsync("UserId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task BlockUserByIdAsync_WithValidDAta_ShouldReturnTrue()
        {
            //Arrange
            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUser = new SellMeUser
            {
                Id = "UserId",
                UserName = "Seller1",
                IsDeleted = false,
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.AddDays(-25),
                Ads = new List<Ad> { new Ad(), new Ad() }
            };

            await context.SellMeUsers.AddAsync(testingUser);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.BlockUserByIdAsync("UserId");

            //Assert
            Assert.True(actual);
            Assert.True(testingUser.IsDeleted);
        }

        [Fact]
        public async Task GetRatingByUser_WithInvalidUserId_ShouldThrowAndArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User with the given id doesn't exist!";

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            //Act and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.usersService.GetRatingByUserAsync("UserId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetRatingByUserAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = 2;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(x => x.FindByIdAsync("UserId"))
                .ReturnsAsync(new SellMeUser
                {
                    Id = "UserId",
                    UserName = "Username",
                    OwnedReviews = new List<Review>
                    {
                        new Review{Comment = "Comment1", Rating = 5},
                        new Review{Comment = "Comment2", Rating = 2},
                        new Review{Comment = "Comment3", Rating = 1},
                        new Review{Comment = "Comment4", Rating = 1},
                        new Review{Comment = "Comment5", Rating = 1},
                    }
                });
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUser = new SellMeUser{Id = "UserId", UserName = "Username"};

            await context.SellMeUsers.AddAsync(testingUser);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.GetRatingByUserAsync("UserId");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetCountOfAllUsersAsync_WithoutAnyUsers_ShouldReturnZero()
        {
            //Arrange
            var expectedResult = 0;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            //Act
            var actual = await this.usersService.GetCountOfAllUsersAsync();

            //Assert
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public async Task GetCountOfAllUsersAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedResult = 3;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = false,EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new SellMeUser{Id = "Id4", UserName = "Seller3", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.GetCountOfAllUsersAsync();

            //Assert
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public async Task GetAllBlockedUserViewModels_WithValidDAta_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 3;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new SellMeUser{Id = "Id4", UserName = "Seller4", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-18)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.GetAllBlockedUserViewModels(1, 10);

            //Assert
            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public async Task GetAllBlockedUserViewModels_WithValidDAta_ShouldReturnCorrectOrder()
        {
            //Arrange
            var expected = new List<string> { "Id3", "Id2", "Id1" };

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = true,EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = true, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = this.usersService.GetAllBlockedUserViewModels(1, 10).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.Equal(expected[0], actual[0].Id);
            Assert.Equal(expected[1], actual[1].Id);
            Assert.Equal(expected[2], actual[2].Id);
        }

        [Fact]
        public async Task UnblockUserByIdAsync_WithInvalidUserId_ShouldThrowAndArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User with the given id doesn't exist!";

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            //Act and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.usersService.UnblockUserByIdAsync("UserId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task UnblockUserByIdAsync_WithValidDAta_ShouldReturnTrue()
        {
            //Arrange
            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUser = new SellMeUser
            {
                Id = "UserId",
                UserName = "Seller1",
                IsDeleted = true,
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.AddDays(-25),
                Ads = new List<Ad> { new Ad(), new Ad() }
            };

            await context.SellMeUsers.AddAsync(testingUser);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.UnblockUserByIdAsync("UserId");

            //Assert
            Assert.True(actual);
            Assert.False(testingUser.IsDeleted);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidUserId_ShouldThrowAndArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User with the given id doesn't exist!";

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            //Act and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.usersService.GetUserByIdAsync("UserId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidDAta_ShouldReturnCorrectUser()
        {
            //Arrange
            var expectedUserId = "UserId";
            var expectedUsername = "Username";

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new Mock<UserManager<SellMeUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(x => x.FindByIdAsync("UserId"))
                .ReturnsAsync(new SellMeUser {Id = "UserId", UserName = "Username"});

            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager.Object);

            var testingUser = new SellMeUser {Id = "UserId", UserName = "User"};

            await context.SellMeUsers.AddAsync(testingUser);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.usersService.GetUserByIdAsync("UserId");

            //Assert
            Assert.Equal(expectedUserId, actual.Id);
            Assert.Equal(expectedUsername, actual.UserName);
        }
    }
}
