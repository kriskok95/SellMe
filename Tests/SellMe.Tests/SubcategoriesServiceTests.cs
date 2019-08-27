namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels.ViewModels.Subcategories;
    using Xunit;

    public class SubcategoriesServiceTests
    {
        private ISubCategoriesService subcategoriesService;

        public SubcategoriesServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetSubcategoriesByCategoryIdAsync_WithInvalidCategoryId_ShouldThrowAndInvalidArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Category with the given id doesn't exist!";

            var context = InitializeContext.CreateContextForInMemory();
            subcategoriesService = new SubCategoriesService(context);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                subcategoriesService.GetSubcategoriesByCategoryIdAsync(1));
            
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetSubcategoriesByCategoryIdAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new List<CreateAdSubcategoryViewModel>
            {
                new CreateAdSubcategoryViewModel
                {
                    Id = 1,
                    Name = "Phones"
                },
                new CreateAdSubcategoryViewModel
                {
                    Id = 2,
                    Name = "Displays"
                }
            };

            var context = InitializeContext.CreateContextForInMemory();
            subcategoriesService = new SubCategoriesService(context);

            var testSubcategories = new List<SubCategory>
            {
                new SubCategory
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Phones"
                },
                new SubCategory
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Displays"
                }
            };

            var testCategory = new Category
            {
                Id = 1,
                Name = "Electronics" 
            };

            await context.Categories.AddAsync(testCategory);
            await context.SubCategories.AddRangeAsync(testSubcategories);
            await context.SaveChangesAsync();

            //Act
            var actual = await subcategoriesService.GetSubcategoriesByCategoryIdAsync(1);

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
        public async Task GetAdsByCategorySubcategoryViewModelsAsync_WithInvalidCategoryId_ShouldThrowAndInvalidArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Category with the given id doesn't exist!";

            var context = InitializeContext.CreateContextForInMemory();
            subcategoriesService = new SubCategoriesService(context);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                subcategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(1));

            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdsByCategorySubcategoryViewModelsAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new List<AdsByCategorySubcategoryViewModel>
            {
                new AdsByCategorySubcategoryViewModel
                {
                    Id = 1,
                    Name = "Phones"
                },
                new AdsByCategorySubcategoryViewModel
                {
                    Id = 2,
                    Name = "Displays"
                }
            };

            var context = InitializeContext.CreateContextForInMemory();
            subcategoriesService = new SubCategoriesService(context);

            var testSubcategories = new List<SubCategory>
            {
                new SubCategory
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Phones"
                },
                new SubCategory
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Displays"
                }
            };

            var testCategory = new Category
            {
                Id = 1,
                Name = "Electronics"
            };

            await context.Categories.AddAsync(testCategory);
            await context.SubCategories.AddRangeAsync(testSubcategories);
            await context.SaveChangesAsync();

            //Act
            var actual = await subcategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(1);

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
    }
}
