namespace SellMe.Tests
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Services;
    using Services.Interfaces;
    using Xunit;

    public class AddressesServiceTests
    {
        private IAddressesService addressesService;

        [Fact]
        public async Task GetAddressByIdAsync_WithValidId_ShouldReturnCorrectAddress()
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();

            addressesService = new AddressesService(context);

            var addressId = 1;

            var testAddress = new Address
            {
                Id = 1,
                City = "Sofia",
                Country = "Bulgaria",
                CreatedOn = DateTime.UtcNow,
                EmailAddress = "kristian.slavchev91@gmail.com",
                District = "Student City",
                ZipCode = 1000,
                PhoneNumber = "08552332",
                Street = "Ivan Vazov"
            };

            await context.Addresses.AddAsync(testAddress);
            await context.SaveChangesAsync();

            //Act
            var result = await addressesService.GetAddressByIdAsync(addressId);

            //Assert 
            Assert.Equal(result, testAddress);

        }

        [Theory]
        [InlineData(10)]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task GetAddressByIdAsync_WithInvalidId_ShouldThrowArgumentException(int addressId)
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();

            addressesService = new AddressesService(context);
            var testAddress = new Address
            {
                Id = 1,
                City = "Sofia",
                Country = "Bulgaria",
                CreatedOn = DateTime.UtcNow,
                EmailAddress = "kristian.slavchev91@gmail.com",
                District = "Student City",
                ZipCode = 1000,
                PhoneNumber = "08552332",
                Street = "Ivan Vazov"
            };

            await context.Addresses.AddAsync(testAddress);
            await context.SaveChangesAsync();
            var expectErrorMessage = "Address with the given ID doesn't exist!";

            //Act

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => addressesService.GetAddressByIdAsync(addressId));
            Assert.Equal(expectErrorMessage, ex.Message);

        }

        [Fact]
        public void GetAllCountries_ShouldReturnTheCorrectCount()
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();
            addressesService = new AddressesService(context);
            var expectedCount = 142;

            //Act
            var countriesCount = addressesService.GetAllCountries().Count;

            //Assert
            Assert.Equal(expectedCount, countriesCount);
        }
    }
}
