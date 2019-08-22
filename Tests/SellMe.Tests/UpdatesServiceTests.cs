using System.Linq;
using Microsoft.EntityFrameworkCore;
using SellMe.Data.Models;

namespace SellMe.Tests
{
    using System;
    using System.Threading.Tasks;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using SellMe.Tests.Common;
    using Xunit;

    public class UpdatesServiceTests
    {
        private IUpdatesService updatesService;

        [Fact]
        public async Task CreateUpdateAdAsync_WithInvalidAdId_ShouldThrowAndInvalidArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var context = InitializeContext.CreateContextForInMemory();
            this.updatesService = new UpdatesService(context);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.updatesService.CreateUpdateAdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateUpdateAdAsync_WithValidData_ShouldCreateUpdateAd()
        {
            //Arrange
            var expectedUpdateAdCount = 1;

            var context = InitializeContext.CreateContextForInMemory();
            this.updatesService = new UpdatesService(context);

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

            //Act
            await this.updatesService.CreateUpdateAdAsync(1);

            //Assert
            Assert.Equal(expectedUpdateAdCount, context.UpdateAds.Count());
        }
    }
}
