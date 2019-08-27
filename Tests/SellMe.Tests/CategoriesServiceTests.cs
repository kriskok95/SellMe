namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Data.Models;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels.ViewModels.Categories;
    using Xunit;

    public class CategoriesServiceTests
    {
        private ICategoriesService categoriesService;

        [Fact]
        public async Task GetCategoryNameById_WithValidId_ShouldReturnTheCorrectCategoryName()
        {
            //Arrange
            var context = InitializeContext.CreateContextForInMemory();

            categoriesService = InitializeCategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto"
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();

            var expectedResult = "Auto";

            //Act
            var actualResult = await categoriesService.GetCategoryNameByIdAsync(1);

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
            var expectedErrorMessage = "Category with the given id doesn't exist!";

            var context = InitializeContext.CreateContextForInMemory();

            categoriesService = InitializeCategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto"
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => categoriesService.GetCategoryNameByIdAsync(categoryId));
            Assert.Equal(ex.Message, expectedErrorMessage);

        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithValidId_ShouldReturnCorrectCategory()
        {
            //Arrange
            string errorMessagePrefix = "CategoriesService GetCategoryByIdAsync() method does not work properly.";

            var context = InitializeContext.CreateContextForInMemory();

            categoriesService = InitializeCategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto"
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();

            //Act
            var actual = await categoriesService.GetCategoryByIdAsync(1);
            var expected = testCategory;

            Assert.True(expected.Name == actual.Name, errorMessagePrefix);

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

            var context = InitializeContext.CreateContextForInMemory();

            categoriesService = InitializeCategoriesService(context);
            var testCategory = new Category
            {
                Id = 1,
                Name = "Auto"
            };
            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();


            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => categoriesService.GetCategoryByIdAsync(categoryId));
            Assert.Equal(expectedErrorMessage, ex.Message);

        }

        [Fact]
        public async Task GetCategoryViewModelsAsync_WithValidData_ShouldReturnProperCollectionOfCreateAdCategoryViewModels()
        {
            //Arrange
            var expected = new List<CreateAdCategoryViewModel>
            {
                new CreateAdCategoryViewModel
                {
                    Id = 1,
                    Name = "Auto"
                },
                new CreateAdCategoryViewModel
                {
                    Id = 2,
                    Name = "Electronics"
                }
            };

            var context = InitializeContext.CreateContextForInMemory();
            categoriesService = InitializeCategoriesService(context);

            var categories = CreateTestingCategories();
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            //Act
            var actual = await categoriesService.GetCategoryViewModelsAsync();

            //Assert
            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Name, elem1.Name);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Name, elem2.Name);
                });

            Assert.Equal(expected.Count, actual.Count);

        }

        [Fact]
        public async Task GetCategoryViewModelsAsync_WithoutData_ShouldReturnAnEmptyCollection()
        {
            //Arrange
            var expected = 0;

            var context = InitializeContext.CreateContextForInMemory();
            categoriesService = InitializeCategoriesService(context);

            var actual = await categoriesService.GetCategoryViewModelsAsync();

            //Act and assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetAllCategoryViewModelsAsync_WithValidData_ShouldReturnCorrectCollection()
        {
            //Arrange
            var expected = new List<CategoryViewModel>
            {
                new CategoryViewModel
                {
                    Id = 1,
                    Name = "Auto"
                },
                new CategoryViewModel
                {
                    Id = 2,
                    Name = "Electronics"
                }
            };

            var context = InitializeContext.CreateContextForInMemory();
            categoriesService = InitializeCategoriesService(context);

            var categories = CreateTestingCategories();
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            //Act
            var actual = await categoriesService.GetAllCategoryViewModelsAsync();

            //Assert
            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Name, elem1.Name);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Name, elem2.Name);
                });

            Assert.Equal(expected.Count, actual.Count);
        }

        [Fact]
        public async Task GetAllCategoryViewModelsAsync_WithoutData_ShouldReturnAnEmptyCollection()
        {
            //Arrange
            var expected = 0;

            var context = InitializeContext.CreateContextForInMemory();
            categoriesService = InitializeCategoriesService(context);

            var actual = await categoriesService.GetAllCategoryViewModelsAsync();

            //Act and assert
            Assert.Equal(expected, actual.Count);
        }

        private List<Category> CreateTestingCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Auto"
                },
                new Category
                {
                    Id = 2,
                    Name = "Electronics"
                }
            };

            return categories;
        }

        private static CategoriesService InitializeCategoriesService(SellMeDbContext context)
        {
            MapperInitializer.InitializeMapper();

            CategoriesService service = new CategoriesService(context);

            return service;
        }
    }
}
