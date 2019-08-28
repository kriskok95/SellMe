﻿namespace SellMe.Tests
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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

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
            var actual = await this.usersService.GetAllUserViewModelsAsync();

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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser{Id = "Id1", UserName = "Seller1", IsDeleted = false,EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-25)},
                new SellMeUser{Id = "Id2", UserName = "Seller2", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-20)},
                new SellMeUser{Id = "Id3", UserName = "Seller3", IsDeleted = false, EmailConfirmed = true, CreatedOn = DateTime.UtcNow.AddDays(-5)},
            };

            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = this.usersService.GetAllUserViewModelsAsync().GetAwaiter().GetResult().ToList();

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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

            var testingUser = new SellMeUser
            {
                Id = "UserId",
                UserName = "Seller1",
                IsDeleted = false,
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.AddDays(-25),
                Ads = new List<Ad> { new Ad(), new Ad()}
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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

            //Act and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.usersService.GetRatingByUser("UserId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetCountOfAllUsersAsync_WithoutAnyUsers_ShouldReturnZero()
        {
            //Arrange
            var expectedResult = 0;

            var moqHttpContext = new Mock<IHttpContextAccessor>();
            var userStore = new Mock<IUserStore<SellMeUser>>();
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

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
            var userManager = new UserManager<SellMeUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var context = InitializeContext.CreateContextForInMemory();
            this.usersService = new UsersService(context, moqHttpContext.Object, userManager);

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
    }
}
