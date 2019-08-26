namespace SellMe.Tests
{
    using SellMe.Services;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using Moq;
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using SellMe.Tests.Common;
    using Xunit;

    public class HomeServiceTests
    {
        private IHomeService homeService;

        public HomeServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetIndexViewModelAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedCategoriesCount = 4;
            var expectedPromotedAdsCount = 5;
            var expectedLatestAddedAdsCount = 5;

            var moqCategoriesService = new Mock<ICategoriesService>();
            moqCategoriesService.Setup(x => x.GetAllCategoryViewModelsAsync())
                .ReturnsAsync(new List<CategoryViewModel>
                {
                    new CategoryViewModel {Id = 1, Name = "Electronics"},
                    new CategoryViewModel {Id = 2, Name = "Sport"},
                    new CategoryViewModel {Id = 3, Name = "Baby"},
                    new CategoryViewModel {Id = 4, Name = "Fashion"},
                });
            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetPromotedAdViewModelsAsync())
                .ReturnsAsync(new List<PromotedAdViewModel>
                {
                    new PromotedAdViewModel {Title = "Iphone 6s", Price = 200},
                    new PromotedAdViewModel {Title = "Samsung TV", Price = 500},
                    new PromotedAdViewModel {Title = "Monitor LG", Price = 100},
                    new PromotedAdViewModel {Title = "Acer laptop", Price = 1200},
                    new PromotedAdViewModel {Title = "Backpack", Price = 50},
                });
            moqAdsService.Setup(x => x.GetLatestAddedAdViewModelsAsync())
                .ReturnsAsync(new List<LatestAddedAdViewModel>
                {
                    new LatestAddedAdViewModel {Title = "Inpa cable", Price = 50},
                    new LatestAddedAdViewModel {Title = "Huawei p8", Price = 200},
                    new LatestAddedAdViewModel {Title = "Xiaomi mi 9", Price = 850},
                    new LatestAddedAdViewModel {Title = "Logitech mx master 2s", Price = 150},
                    new LatestAddedAdViewModel {Title = "Keyboard", Price = 50},
                });

            this.homeService = new HomeService(moqCategoriesService.Object, moqAdsService.Object); 

            //Act 
            var actual = await this.homeService.GetIndexViewModelAsync();

            //Assert
            Assert.Equal(expectedCategoriesCount, actual.CategoryViewModels.Count);
            Assert.Equal(expectedLatestAddedAdsCount, actual.LatestAddedAdViewModels.Count);
            Assert.Equal(expectedPromotedAdsCount, actual.PromotedAdViewModels.Count);
        }
    }
}
