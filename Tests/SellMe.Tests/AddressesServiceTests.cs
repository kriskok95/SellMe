namespace SellMe.Tests
{
    using System;
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using Xunit;

    public class AddressesServiceTests
    {
        private IAddressesService addressesService;

        [Fact]
        public async Task GetAddressByIdAsync_WithValidId_ShouldReturnCorrectAddress()
        {
            //Arrange
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForInMemory();
            this.addressesService = new AddressesService(context);

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
                Street = "Ivan Vazov",
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
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForInMemory();
            IAddressesService addressesService = new AddressesService(context);
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
                Street = "Ivan Vazov",
            };

            await context.Addresses.AddAsync(testAddress);
            await context.SaveChangesAsync();
            var expectErrorMessage = "Address with the given ID doesn't exist!";

            //Act
            try
            {
                var result = await addressesService.GetAddressByIdAsync(addressId);

            }
            catch (ArgumentException e)
            {
                Assert.Contains(e.Message, expectErrorMessage);
                return;
            }

            Assert.True(false, "The method had to throw an argument exception");
        }

       [Fact]
        public void GetAllCountries_ShouldReturnTheCorrectCount()
        {
            //Arrange
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForInMemory();
            this.addressesService = new AddressesService(context);
            var expectedCount = 142;

            //Act
            var countriesCount = this.addressesService.GetAllCountries().Count;

            //Assert
            Assert.Equal(expectedCount, countriesCount);
        }
    }
}
