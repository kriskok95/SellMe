namespace SellMe.Tests
{
    using System;
    using Xunit;
    using SellMe.Tests.Common;
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using SellMe.Services;
    public class CategoriesServiceTests
    {
        private ICategoriesService categoriesService;

        [Fact]
        public async Task GetCategoryNameById_WithValidId_ShouldReturnTheCorrectCategoryName()
        {
            //Arrange
            var connectionFactory = new ConnectionFactory();

            using (var context = connectionFactory.CreateContextForInMemory())
            {
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
            var expectedErrorMessage = "Category with the given id doesn't exist!";
            var connectionFactory = new ConnectionFactory();

            using (var context = connectionFactory.CreateContextForInMemory())
            {
                this.categoriesService = new CategoriesService(context);
                var testCategory = new Category
                {
                    Id = 1,
                    Name = "Auto",
                };
                await context.Categories.AddAsync(testCategory);
                await context.SaveChangesAsync();

                //Act and assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.categoriesService.GetCategoryNameByIdAsync(categoryId));
                Assert.Equal(ex.Message, expectedErrorMessage);
            }
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithValidId_ShouldReturnCorrectCategory()
        {
            //Arrange
            string errorMessagePrefix = "CategoriesService GetCategoryByIdAsync() method does not work properly.";

            var connectionFactory = new ConnectionFactory();
            using (var context = connectionFactory.CreateContextForInMemory())
            {
                this.categoriesService = new CategoriesService(context);
                var testCategory = new Category
                {
                    Id = 1,
                    Name = "Auto",
                };
                await context.Categories.AddAsync(testCategory);
                await context.SaveChangesAsync();

                //Act
                var actual = await this.categoriesService.GetCategoryByIdAsync(1);
                var expected = testCategory;

                Assert.True(expected.Name == actual.Name, errorMessagePrefix);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task GetCategoryByIdAsync_WithInvalidId_ShouldThrowArgumentException(int categoryId)
        {
            //Arrange
            var expectedErrorMessage = "Category with the given Id doesn't exist!";

            var connectionFactory = new ConnectionFactory();
            using (var context = connectionFactory.CreateContextForInMemory())
            {
                this.categoriesService = new CategoriesService(context);
                var testCategory = new Category
                {
                    Id = 1,
                    Name = "Auto",
                };
                await context.Categories.AddAsync(testCategory);
                await context.SaveChangesAsync();


                //Act and assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.categoriesService.GetCategoryByIdAsync(categoryId));
                Assert.Equal(expectedErrorMessage, ex.Message);
            }
        }
    }
}
