using System;

namespace SellMe.Tests
{
    using Xunit;
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using SellMe.Services;
    public  class CategoriesServiceTests
    {
        private ICategoriesService categoriesService;

        [Fact]
        public async Task GetCategoryNameById_WithValidId_ShouldReturnTheCorrectCategoryName()
        {
            //Arrange
            var connectionFactory = new ConnectionFactory();
            var context = connectionFactory.CreateContextForInMemory();
            this.categoriesService = new CategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto",
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();

            var expectedResult = "Auto";

            //Act
            var actualResult = await this.categoriesService.GetCategoryNameByIdAsync(1);

            //Assert
            Assert.Equal(expectedResult, actualResult);

        }

        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(999)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task GetCategoryNameById_WithInvalidId_ShouldThrowArgumentException(int categoryId)
        {
            //Arrange
            var connectionFactory = new ConnectionFactory();
            var context = connectionFactory.CreateContextForInMemory();
            this.categoriesService = new CategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto",
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();

            var expectedErrorMessage = "Category with the given id doesn't exist!";

            try
            {
                //Act
                var result = await this.categoriesService.GetCategoryNameByIdAsync(categoryId);
            }
            catch (ArgumentException e)
            {
                //Assert
                Assert.Contains(expectedErrorMessage, e.Message);
                return;
            }

            Assert.True(false, "The method had to throw an argument exception!");
        }
    }
}
