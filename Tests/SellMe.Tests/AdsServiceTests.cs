namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Moq;
    using SellMe.Data.Models;
    using SellMe.Services;
    using SellMe.Services.Interfaces;
    using SellMe.Tests.Common;
    using SellMe.Web.ViewModels.BindingModels.Ads;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public class AdsServiceTests
    {
        private IAdsService adsService;

        public AdsServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateAdAsync_WithValidData_ShouldCreateAd()
        {
            //Arrange
            var expectedAdsCount = 1;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();
            moqCloudinaryService.Setup(x => x.UploadPictureAsync(moqIFormFile.Object, "FileName"))
                .ReturnsAsync("http://test.com");

            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Ad>(It.IsAny<CreateAdInputModel>()))
                .Returns(new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    AvailabilityCount = 1,
                    Price = 120,
                    Condition = new Condition { Id = 1, Name = "Brand New" },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com"
                    }
                });

            var createAdInputModel = new CreateAdInputModel
            {
                CreateAdDetailInputModel = new CreateAdDetailInputModel
                {
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Availability = 1,
                    Price = 120,
                    ConditionId = 1,
                    CategoryId = 1,
                    SubCategoryId = 2,
                    Images = new List<IFormFile>
                    {
                        moqIFormFile.Object
                    }
                }
            };

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            await this.adsService.CreateAdAsync(createAdInputModel);

            //Assert
            Assert.Equal(expectedAdsCount, context.Ads.Count());
        }

        [Fact]
        public async Task GetAdsByCategoryViewModelAsync_WithInvalidCategoryId_ShouldThrowAndArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Category with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            var testingCategories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    Id = 2,
                    Name = "Animals"
                },
                new Category
                {
                    Id = 3,
                    Name = "Services"
                },
            };

            await context.Categories.AddRangeAsync(testingCategories);
            await context.SaveChangesAsync();

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.adsService.GetAdsByCategoryViewModelAsync(15, 1, 10));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdsByCategoryViewModelAsync_WithValidData_ShouldReturnCorrectResult()
        {

            //Arrange 
            var expectedAdsCount = 2;
            var expectedCategoryName = "Electronics";
            var expectedCategoryId = 1;
            var expectedSubcategoriesCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqCloudinary = new Mock<ICloudinaryService>();
            moqCategoriesService.Setup(x => x.GetAllCategoryViewModelsAsync())
                .ReturnsAsync(new List<CategoryViewModel>
                {
                    new CategoryViewModel
                    {
                        Id = 1,
                        Name = "Electronics"
                    },
                    new CategoryViewModel
                    {
                        Id = 2,
                        Name = "Animals"
                    },
                    new CategoryViewModel
                    {
                        Id = 3,
                        Name = "Services"
                    },
                });
            moqCategoriesService.Setup(x => x.GetCategoryNameByIdAsync(1))
                .ReturnsAsync("Electronics");

            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            moqSubcategoriesService.Setup(x => x.GetAdsByCategorySubcategoryViewModelsAsync(1))
                .ReturnsAsync(new List<AdsByCategorySubcategoryViewModel>
                {
                    new AdsByCategorySubcategoryViewModel
                    {
                        Id = 1,
                        Name = "Phones",
                    },
                    new AdsByCategorySubcategoryViewModel
                    {
                        Id = 2,
                        Name = "Monitors"
                    }
                });

            var moqMapper = new Mock<IMapper>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinary.Object);

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    CategoryId = 1,
                    CreatedOn = DateTime.UtcNow,
                    SubCategoryId = 1,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org2"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 2,
                    Title = "Xiaomi mi 9",
                    Description = "Perfect condition",
                    Price = 800,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    CreatedOn = DateTime.UtcNow,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org3"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung tv",
                    Description = "Brand new",
                    Price = 100,
                    CategoryId = 2,
                    CreatedOn = DateTime.UtcNow,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org2"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
            };

            var testingCategories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    Id = 2,
                    Name = "Animals"
                },
                new Category
                {
                    Id = 3,
                    Name = "Services"
                },
            };

            var testingSubcategories = new List<SubCategory>
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
                    Name = "Monitors"
                },

            };

            await context.Categories.AddRangeAsync(testingCategories);
            await context.Ads.AddRangeAsync(testingAds);
            await context.SubCategories.AddRangeAsync(testingSubcategories);
            await context.SaveChangesAsync();

            //Act
            var actual = await this.adsService.GetAdsByCategoryViewModelAsync(1, 1, 10);

            //Asert
            Assert.Equal(expectedAdsCount, actual.AdsViewModels.Count);
            Assert.Equal(expectedCategoryId, actual.CategoryId);
            Assert.Equal(expectedCategoryName, actual.CategoryName);
            Assert.Equal(expectedSubcategoriesCount, actual.AdsByCategorySubcategoryViewModels.Count);
        }

        [Fact]
        public async Task GetAdByIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetAdByIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdByIdAsync_WithValidAdId_ShouldReturnCorrectAd()
        {
            //Arrange
            var expected = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                CategoryId = 1,
                IsApproved = true,
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
                    EmailAddress = "Ivan@gmail.com"
                }
            };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                CategoryId = 1,
                IsApproved = true,
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
                    EmailAddress = "Ivan@gmail.com"
                }
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdByIdAsync(1);

            //Assert
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.AvailabilityCount, actual.AvailabilityCount);
            Assert.Equal(expected.Price, actual.Price);
            Assert.Equal(expected.Address.Country, actual.Address.Country);
            Assert.Equal(expected.Address.City, actual.Address.City);
        }

        [Fact]
        public async Task GetAdDetailsViewModelAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetAdDetailsViewModelAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdDetailsViewModelAsync_WhenExecute_ShouldCreateAView()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            moqMapper.Setup(x => x.Map<AdDetailsViewModel>(It.IsAny<Ad>()))
                .Returns(new AdDetailsViewModel
                {
                    Id = 1,
                    SellerId = "SellerId",
                    Seller = "Martin",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 120,
                    Phone = "0895335532",
                    CategoryName = "Electronics",
                    SubcategoryName = "Phones",
                    ConditionName = "Brand New",
                    Rating = 0
                });

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
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
                    EmailAddress = "Ivan@gmail.com"
                },
                Seller = new SellMeUser
                {
                    Id = "SellerId",
                    UserName = "Martin"
                },
                SellerId = "SellerId"
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdDetailsViewModelAsync(1);

            Assert.True(context.AdViews.Count() == 1);
        }

        [Fact]
        public async Task GetAdDetailsViewModelAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedResult = new AdDetailsViewModel
            {
                Id = 1,
                SellerId = "SellerId",
                Seller = "Martin",
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 120,
                Phone = "0895335532",
                CategoryName = "Electronics",
                SubcategoryName = "Phones",
                ConditionName = "Brand New",
                Rating = 0
            };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<AdDetailsViewModel>(It.IsAny<Ad>()))
                .Returns(new AdDetailsViewModel
                {
                    Id = 1,
                    SellerId = "SellerId",
                    Seller = "Martin",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 120,
                    Phone = "0895335532",
                    CategoryName = "Electronics",
                    SubcategoryName = "Phones",
                    ConditionName = "Brand New",
                    Rating = 0
                });
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
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
                    EmailAddress = "Ivan@gmail.com"
                },
                Seller = new SellMeUser
                {
                    Id = "SellerId",
                    UserName = "Martin"
                },
                SellerId = "SellerId"
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdDetailsViewModelAsync(1);

            //Assert
            Assert.Equal(expectedResult.Id, actual.Id);
            Assert.Equal(expectedResult.Title, actual.Title);
            Assert.Equal(expectedResult.CategoryName, actual.CategoryName);
            Assert.Equal(expectedResult.SubcategoryName, actual.SubcategoryName);
            Assert.Equal(expectedResult.Price, actual.Price);
            Assert.Equal(expectedResult.Description, actual.Description);
            Assert.Equal(expectedResult.Rating, actual.Rating);
        }

        [Fact]
        public async Task ArchiveAdByIdAsync_WithValidData_ShouldChangeIsDeletedToTrue()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = false
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ArchiveAdByIdAsync(1);

            //Assert
            Assert.True(testingAd.IsDeleted);
        }

        [Fact]
        public async Task ArchiveAdByIdAsync_WithArchivedAd_ShouldReturnFalse()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = true
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ArchiveAdByIdAsync(1);

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task ArchiveAdByIdAsync_WithValidData_ShouldReturnTrue()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = false
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ArchiveAdByIdAsync(1);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task ActivateAdById_WithActiveAd_ShouldReturnFalse()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = false
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ActivateAdByIdAsync(1);

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task ActivateAdById_WithValidData_ShouldChangeIsDeletedToFalse()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = true
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ActivateAdByIdAsync(1);

            //Assert
            Assert.False(testingAd.IsDeleted);
        }

        [Fact]
        public async Task ActivateAdById_WithValidData_ShouldReturnTrue()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                IsDeleted = true
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object,
                moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ActivateAdByIdAsync(1);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task GetEditAdBindingModelById_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetEditAdBindingModelById(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetEditAdBindingModelById_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedResult = new EditAdBindingModel
            {
                EditAdViewModel = new EditAdViewModel
                {
                    EditAdDetailsViewModel = new EditAdDetailsViewModel
                    {
                        Id = 1,
                        Title = "Iphone 6s",
                        Description = "PerfectCondition",
                        Availability = 1,
                        Price = 120,
                        ConditionName = "Brand New",
                        CategoryName = "Electronics",
                        SubcategoryName = "Phones"
                    },
                    EditAdAddressViewModel = new EditAdAddressViewModel
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com",
                    }
                }
            };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<EditAdDetailsViewModel>(It.IsAny<Ad>()))
                .Returns(new EditAdDetailsViewModel
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Availability = 1,
                    Price = 120,
                    ConditionName = "Brand New",
                    CategoryName = "Electronics",
                    SubcategoryName = "Phones"
                });

            moqMapper.Setup(x => x.Map<EditAdAddressViewModel>(It.IsAny<Ad>()))
                .Returns(new EditAdAddressViewModel
                {
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com",
                });
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
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
                    EmailAddress = "Ivan@gmail.com"
                },
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetEditAdBindingModelById(1);

            //Assert
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.Id, actual.EditAdViewModel.EditAdDetailsViewModel.Id);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.Title, actual.EditAdViewModel.EditAdDetailsViewModel.Title);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.Description, actual.EditAdViewModel.EditAdDetailsViewModel.Description);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.Availability, actual.EditAdViewModel.EditAdDetailsViewModel.Availability);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.Price, actual.EditAdViewModel.EditAdDetailsViewModel.Price);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.ConditionName, actual.EditAdViewModel.EditAdDetailsViewModel.ConditionName);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.CategoryName, actual.EditAdViewModel.EditAdDetailsViewModel.CategoryName);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdDetailsViewModel.SubcategoryName, actual.EditAdViewModel.EditAdDetailsViewModel.SubcategoryName);

            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.Country, actual.EditAdViewModel.EditAdAddressViewModel.Country);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.City, actual.EditAdViewModel.EditAdAddressViewModel.City);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.Street, actual.EditAdViewModel.EditAdAddressViewModel.Street);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.District, actual.EditAdViewModel.EditAdAddressViewModel.District);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.PhoneNumber, actual.EditAdViewModel.EditAdAddressViewModel.PhoneNumber);
            Assert.Equal(expectedResult.EditAdViewModel.EditAdAddressViewModel.EmailAddress, actual.EditAdViewModel.EditAdAddressViewModel.EmailAddress);
        }

        [Fact]
        public async Task GetAdTitleByIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetAdTitleByIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdTitleByIdAsync_WithValidData_ShouldReturnCorrectTitle()
        {
            //Arrange
            var expectedResult = "Iphone 6s";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdTitleByIdAsync(1);

            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public async Task UpdateAdByIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.UpdateAdByIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateAdByIdAsync_WithValidData_ShouldUpdateProperlyActiveFromAndActiveTo()
        {
            //Arrange
            var expectedActiveFrom = DateTime.UtcNow.Day;
            var expectedActiveTo = DateTime.UtcNow.AddDays(30).Day;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
                ActiveFrom = DateTime.UtcNow.AddDays(-15),
                ActiveTo = DateTime.UtcNow.AddDays(15),
                AvailabilityCount = 1,
                Updates = 3,
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
                    EmailAddress = "Ivan@gmail.com"
                },
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            await this.adsService.UpdateAdByIdAsync(1);

            //Assert
            Assert.Equal(expectedActiveFrom, testingAd.ActiveFrom.Day);
            Assert.Equal(expectedActiveTo, testingAd.ActiveTo.Day);
        }

        [Fact]
        public async Task UpdateAdByIdAsync_WithValidData_ShouldDecrementUpdates()
        {
            //Arrange
            var expected = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
                ActiveFrom = DateTime.UtcNow.AddDays(-15),
                ActiveTo = DateTime.UtcNow.AddDays(15),
                AvailabilityCount = 1,
                Updates = 3,
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
                    EmailAddress = "Ivan@gmail.com"
                },
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            await this.adsService.UpdateAdByIdAsync(1);

            //Assert
            Assert.Equal(expected, testingAd.Updates);
        }

        [Fact]
        public async Task UpdateAdByIdAsync_WithoutUpdates_ShouldNotChangeActiveFromAndActiveto()
        {
            //Arrange
            var expectedActiveFrom = DateTime.UtcNow.Day;
            var expectedActiveTo = DateTime.UtcNow.AddDays(30).Day;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Updates = 0,
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
                    EmailAddress = "Ivan@gmail.com"
                },
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            await this.adsService.UpdateAdByIdAsync(1);

            //Assert
            Assert.Equal(expectedActiveFrom, testingAd.ActiveFrom.Day);
            Assert.Equal(expectedActiveTo, testingAd.ActiveTo.Day);
        }

        [Fact]
        public async Task GetPromotedAdViewModels_WithValidData_ShouldReturnExpectedAdsCount()
        {

            //Arrange
            var expected = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    PromotionOrders = new List<PromotionOrder>
                    {
                        new PromotionOrder
                        {
                            ActiveTo = DateTime.UtcNow.AddDays(30)
                        }
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                    PromotionOrders = new List<PromotionOrder>
                    {
                        new PromotionOrder
                        {
                            ActiveTo = DateTime.UtcNow.AddDays(20)
                        }
                    }
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Price = 250,
                    IsApproved = false,
                    IsDeleted = false,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                    PromotionOrders = new List<PromotionOrder>
                    {
                        new PromotionOrder
                        {
                            ActiveTo = DateTime.UtcNow.AddDays(15)
                        }
                    }
                },
                new Ad
                {
                    Id = 3,
                    Title = "Audi A8",
                    PromotionOrders = new List<PromotionOrder>
                    {
                        new PromotionOrder
                        {
                            ActiveTo = DateTime.UtcNow.AddDays(-20)
                        }
                    }
                },

            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetPromotedAdViewModels();

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetPromotedAdViewModels_WithoutAds_ShouldReturnAnEmptyCollection()
        {
            //Arrange
            var expected = 0;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetPromotedAdViewModels();

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetLatestAddedAdViewModels_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = true,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetLatestAddedAdViewModels();

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetLatestAddedAdViewModels_WithoutAds_ShouldReturnAnEmptyCollection()
        {
            //Arrange
            var expected = 0;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetLatestAddedAdViewModels();

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetAdsBySubcategoryViewModelAsync_WithInvalidCategoryId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Category with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetAdsBySubcategoryViewModelAsync(1, 2, 1, 10));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdsBySubcategoryViewModelAsync_WithInvalidSubcategoryId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Subcategory with the give id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingCategory = new Category
            {
                Id = 1,
                Name = "Electronics"
            };

            await context.Categories.AddAsync(testingCategory);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetAdsBySubcategoryViewModelAsync(1, 1, 1, 10));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetAdsBySubcategoryViewModelAsync_WithValidDate_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedAdsCount = 2;
            var expectedSubcategoriesCount = 3;
            var expectedSubcategoryId = 1;
            var expectedCategoryId = 1;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            moqSubcategoriesService.Setup(x => x.GetAdsByCategorySubcategoryViewModelsAsync(1))
                .ReturnsAsync(new List<AdsByCategorySubcategoryViewModel>
                {
                    new AdsByCategorySubcategoryViewModel
                    {
                        Id = 1,
                        Name = "Phones",
                    },
                    new AdsByCategorySubcategoryViewModel
                    {
                        Id = 2,
                        Name = "Monitors",
                    },
                    new AdsByCategorySubcategoryViewModel
                    {
                        Id = 3,
                        Name = "Tvs",
                    },
                });
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    CreatedOn = DateTime.UtcNow,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org2"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 2,
                    Title = "Xiaomi mi 9",
                    Description = "Perfect condition",
                    Price = 800,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    CreatedOn = DateTime.UtcNow,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org3"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung tv",
                    Description = "Brand new",
                    Price = 100,
                    CategoryId = 1,
                    SubCategoryId = 2,
                    CreatedOn = DateTime.UtcNow,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl =  "https://www.webpagetest.org2"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    IsApproved = true,
                    IsDeleted = false,
                },
            };

            var testingCategory = new Category
            {
                Id = 1,
                Name = "Electronics"
            };

            var testingSubcategory = new SubCategory
            {
                Id = 1,
                CategoryId = 1,
                Name = "Phones"
            };

            await context.SubCategories.AddAsync(testingSubcategory);
            await context.AddRangeAsync(testingAds);
            await context.Categories.AddAsync(testingCategory);
            await context.SaveChangesAsync();


            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdsBySubcategoryViewModelAsync(1, 1, 1, 10);

            //Assert
            Assert.Equal(expectedSubcategoriesCount, actual.AdsByCategorySubcategoryViewModels.Count);
            Assert.Equal(expectedAdsCount, actual.AdsBySubcategoryViewModels.Count);
            Assert.Equal(expectedCategoryId, actual.CategoryId);
            Assert.Equal(expectedSubcategoryId, actual.SubcategoryId);
        }

        [Fact]
        public async Task GetMyActiveAdsViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 3,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 10,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(25),
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "FakeSellerId",
                    Updates = 0,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(10),
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetMyActiveAdsViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetFavoriteAdsViewModelsAsync_WithNullOrEmptyUserId_ShouldThrowAnArgumentException(string userId)
        {
            //Arrange
            var expectedErrorMessage = "User id can't be null or empty!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetFavoriteAdsViewModelsAsync(userId, 1, 10));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetFavoriteAdsViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new SellMeUser
                {
                    Id = "CurrentUserId",
                    UserName = "CurrentUser"
                });

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 3,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Phones",
                    },
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Description = "Perfect Condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 10,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(25),
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Description = "Description",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "FakeSellerId",
                    Updates = 0,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(10),
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "FakeSellerId"
                        }
                    },
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetFavoriteAdsViewModelsAsync("SellMeUserId", 1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetArchivedAdsViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "CurrentUserId",
                    Updates = 3,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Phones",
                    },
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Description = "Perfect Condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "CurrentUserId",
                    Updates = 10,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(25),
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Description = "Description",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 0,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(10),
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetArchivedAdsViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetAdsBySearchViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "CurrentUserId",
                    Updates = 3,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Phones",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Description = "Perfect Condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = true,
                    SellerId = "CurrentUserId",
                    Updates = 10,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(25),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Description = "Description",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "CurrentUserId",
                    Updates = 0,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(10),
                    SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
                    {
                        new SellMeUserFavoriteProduct
                        {
                            AdId = 1,
                            SellMeUserId = "CurrentUserId"
                        }
                    },
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdsBySearchViewModelsAsync("phone", 1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetAdsByUserBindingModelAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedCount = 2;
            var expectedSellerId = "SellerId";
            var expectedSellerUsername = "Seller";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetUserByIdAsync("SellerId"))
                .ReturnsAsync(new SellMeUser
                {
                    Id = "SellerId",
                    UserName = "Seller"
                });

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "Perfect condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "SellerId",
                    Updates = 3,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Phones",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org1"
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    Title = "Samsung TV",
                    Description = "Perfect Condition",
                    Price = 100,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "SellerId",
                    Updates = 10,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(25),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org2"
                        }
                    },
                },
                new Ad
                {
                    Id = 4,
                    Title = "Motorola phone",
                    Description = "Description",
                    Price = 250,
                    IsApproved = true,
                    IsDeleted = false,
                    SellerId = "FakeSellerId",
                    Updates = 0,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(10),
                    Category = new Category
                    {
                        Name = "Electronics",
                    },
                    SubCategory = new SubCategory
                    {
                        Name = "Tvs",
                    },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = "https://www.webpagetest.org3"
                        }
                    },
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdsByUserBindingModelAsync("SellerId", 1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.AdViewModels.Count);
            Assert.Equal(expectedSellerId, actual.UserId);
            Assert.Equal(expectedSellerUsername, actual.Username);
        }

        [Fact]
        public async Task EditAdById_WithInvalidAd_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.EditAd(new EditAdInputModel()));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task EditAdById_WithValidData_ShouldEditAd()
        {
            //Arrange
            var expectedAdId = 1;
            var expectedTitle = "Iphone 6s new";
            var expectedDescription = "Perfect Condition new";
            var expectedAvailability = 2;
            var expectedPrice = 110;
            var expectedCountry = "Bulgaria New";
            var expectedCity = "Sofia New";
            var expectedStreet = "Ivan Vazov New";
            var expectedDistinct = "Student city New";
            var expectedZipCode = 1200;
            var expectedPhoneNumber = "085253235";
            var expectedEmailAddress = "kristian-new@gmail.com";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();
            moqCloudinaryService.Setup(x => x.UploadPictureAsync(moqIFormFile.Object, "ImageName"))
                .ReturnsAsync("test");

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Category = new Category { Id = 1, Name = "Electronics" },
                SubCategory = new SubCategory { Id = 2, CategoryId = 1, Name = "Phones" },
                CategoryId = 1,
                IsApproved = true,
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Updates = 0,
                Price = 120,
                Condition = new Condition { Name = "Brand New" },
                Images = new List<Image>(),
                Address = new Address
                {
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com"
                },
            };

            await context.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            var editAdInputModel = new EditAdInputModel
            {
                AdId = 1,
                EditAdDetailsInputModel = new EditAdDetailsInputModel
                {
                    Title = "Iphone 6s new",
                    Description = "Perfect Condition new",
                    Availability = 2,
                    Price = 110,
                    Images = new List<IFormFile>
                    {
                        moqIFormFile.Object
                    }
                },
                EditAdAddressInputModel = new EditAdAddressInputModel
                {
                    Country = "Bulgaria New",
                    City = "Sofia New",
                    Street = "Ivan Vazov New",
                    District = "Student city New",
                    ZipCode = 1200,
                    PhoneNumber = "085253235",
                    EmailAddress = "kristian-new@gmail.com"
                }
            };


            //Act
            await this.adsService.EditAd(editAdInputModel);

            //Assert
            Assert.Equal(expectedAdId, testingAd.Id);
            Assert.Equal(expectedTitle, testingAd.Title);
            Assert.Equal(expectedDescription, testingAd.Description);
            Assert.Equal(expectedAvailability, testingAd.AvailabilityCount);
            Assert.Equal(expectedPrice, testingAd.Price);
            Assert.Equal(expectedCountry, testingAd.Address.Country);
            Assert.Equal(expectedCity, testingAd.Address.City);
            Assert.Equal(expectedStreet, testingAd.Address.Street);
            Assert.Equal(expectedDistinct, testingAd.Address.District);
            Assert.Equal(expectedZipCode, testingAd.Address.ZipCode);
            Assert.Equal(expectedPhoneNumber, testingAd.Address.PhoneNumber);
            Assert.Equal(expectedEmailAddress, testingAd.Address.EmailAddress);

        }

        [Fact]
        public async Task GetAdsForApprovalViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                },
                new Ad
                {
                    Id = 2,
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = true,
                }
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdsForApprovalViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetAdsForApprovalViewModelsAsync_WithValidData_ShouldReturnCorrectOrder()
        {
            //Arrange
            var expectedAdsIds = new List<int> { 2, 1 };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                },
                new Ad
                {
                    Id = 2,
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = true,
                }
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAdsForApprovalViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedAdsIds[0], actual[0].Id);
            Assert.Equal(expectedAdsIds[1], actual[1].Id);
        }

        [Fact]
        public async Task ApproveAdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.ApproveAdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task ApproveAdAsync_WithAlreadyApprovedAd_ShouldThrowAnInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "The given ad is already approved!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
                IsApproved = true,
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this.adsService.ApproveAdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task ApproveAdAsync_WithValidData_ShouldReturnTrue()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
                IsApproved = false,
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.ApproveAdAsync(1);

            //Assert
            Assert.True(actual);
            Assert.True(testingAd.IsApproved);
        }

        [Fact]
        public async Task GetRejectAdBindingModelAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetRejectAdBindingModelAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetRejectAdBindingModelAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new RejectAdViewModel
            {
                AdId = 1,
                AdTitle = "Iphone 6s"
            };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<RejectAdViewModel>(It.IsAny<Ad>()))
                .Returns(new RejectAdViewModel { AdId = 1, AdTitle = "Iphone 6s" });

            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
                IsApproved = false,
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetRejectAdBindingModelAsync(1);

            //Assert
            Assert.Equal(expected.AdId, actual.ViewModel.AdId);
            Assert.Equal(expected.AdTitle, actual.ViewModel.AdTitle);
        }

        [Fact]
        public async Task CreateAdRejectionAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.CreateAdRejectionAsync(1, "Comment"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateAdRejectionAsync_WithNullOrEmptyComment_ShouldThrowAnArgumentException(string comment)
        {
            //Arrange
            var expectedErrorMessage = "The comment can't be null or empty string!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.CreateAdRejectionAsync(1, comment));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateAdRejectionAsync_WithValidData_ShouldCreateRejection()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
                IsDeclined = false,
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            await this.adsService.CreateAdRejectionAsync(1, "Comment");

            //Assert
            Assert.Single(context.AdRejections);
            Assert.True(testingAd.IsDeclined);
        }

        [Fact]
        public async Task GetWaitingForApprovalByCurrentUserViewModels_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = true,
                },
                new Ad
                {
                    Id = 4,
                    Title = "Fake Ad",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 5435,
                    IsApproved = false,
                    IsDeclined = true,
                }
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetWaitingForApprovalByCurrentUserViewModels(1, 10);

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task GetRejectedAdByUserViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                    IsDeclined = true,
                    AdRejections = new List<AdRejection>
                    {
                        new AdRejection
                        {
                            Id = 1,
                            AdId = 1,
                            Comment = "Comment",
                        }
                    }
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                    IsDeclined = true,
                    AdRejections = new List<AdRejection>
                    {
                        new AdRejection
                        {
                            Id = 2,
                            AdId = 2,
                            Comment = "Comment",
                        }
                    }
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = false,
                    IsDeclined = true,
                    IsDeleted = true,
                    AdRejections = new List<AdRejection>
                    {
                        new AdRejection
                        {
                            Id = 3,
                            AdId = 3,
                            Comment = "Comment",
                        }
                    }
                },
                new Ad
                {
                    Id = 4,
                    Title = "Fake Ad",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 5435,
                    IsApproved = false,
                    IsDeclined = false,
                    AdRejections = new List<AdRejection>
                    {
                        new AdRejection
                        {
                            Id = 4,
                            AdId = 4,
                            Comment = "Comment",
                        }
                    }
                }
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetRejectedAdByUserViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public async Task SubmitRejectedAdAsync_WithInvalidRejectionId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad Rejection with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.SubmitRejectedAdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task SubmitRejectedAdAsync_WithNonExistingAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAdRejection = new AdRejection
            {
                Id = 1,
                AdId = 1,
                Comment = "Comment",
            };
            await context.AdRejections.AddAsync(testingAdRejection);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.SubmitRejectedAdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task SubmitRejectedAdAsync_WithValidData_ShouldReturnTrue()
        {
            //Arrange
            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                Price = 200,
                IsDeclined = true,
            };

            var testingAdRejection = new AdRejection
            {
                AdId = 1,
                Comment = "Comment",
            };
            await context.Ads.AddAsync(testingAd);
            await context.AdRejections.AddAsync(testingAdRejection);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object, moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.SubmitRejectedAdAsync(1);

            Assert.True(actual);
            Assert.False(testingAd.IsDeclined);
        }

        [Fact]
        public async Task GetRejectedAdAllViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                    IsDeclined = true,
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                    IsDeclined = true,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = false,
                    IsDeclined = false,
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetRejectedAdAllViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetRejectedAdAllViewModelsAsync_WithValidData_ShouldReturnCorrectOrder()
        {
            //Arrange
            var expectedIds = new List<int> {2, 3, 1};

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = false,
                    IsDeclined = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-24),
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = false,
                    IsDeclined = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = false,
                    IsDeclined = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetRejectedAdAllViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedIds[0], actual[0].Id);
            Assert.Equal(expectedIds[1], actual[1].Id);
            Assert.Equal(expectedIds[2], actual[2].Id);
        }

        [Fact]
        public async Task GetAllActiveAdViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expectedCount = 2;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = true,
                    IsDeleted = false,
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = true,
                    IsDeleted = true,
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAllActiveAdViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedCount, actual.Count);
        }

        [Fact]
        public async Task GetAllActiveAdViewModelsAsync_WithValidData_ShouldReturnCorrectOrder()
        {
            //Arrange
            var expectedIds = new List<int> { 2, 3, 1 };

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            var testingAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    SellerId = "CurrentUserId",
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    Price = 200,
                    IsApproved = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-24),
                },
                new Ad
                {
                    Id = 2,
                    SellerId = "CurrentUserId",
                    Title = "Motorola phone",
                    Description = "PerfectCondition",
                    Price = 500,
                    IsApproved = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                },
                new Ad
                {
                    Id = 3,
                    Title = "Samsung TVs",
                    SellerId = "CurrentUserId",
                    Description = "brand new",
                    Price = 2000,
                    IsApproved = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAllActiveAdViewModelsAsync(1, 10);

            //Assert
            Assert.Equal(expectedIds[0], actual[0].Id);
            Assert.Equal(expectedIds[1], actual[1].Id);
            Assert.Equal(expectedIds[2], actual[2].Id);
        }

        [Fact]
        public async Task GetTheCountForTheCreatedAdsForTheLastTenDays_WithValidData_ShouldReturnCorrectCollectionLength()
        {
            //Arrange
            var expectedLength = 10;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync();

            //Assert
            Assert.Equal(expectedLength, actual.Count);
        }

        [Fact]
        public async Task GetTheCountForTheCreatedAdsForTheLastTenDays_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new List<int> { 1, 0, 1, 0, 2, 0, 0, 0, 1, 1};

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad {Id = 1, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new Ad {Id = 2, CreatedOn = DateTime.UtcNow.AddDays(-5)},
                new Ad {Id = 3, CreatedOn = DateTime.UtcNow.AddDays(-7)},
                new Ad {Id = 4, CreatedOn = DateTime.UtcNow.AddDays(-9)},
                new Ad {Id = 5, CreatedOn = DateTime.UtcNow.AddDays(-1)},
                new Ad {Id = 6, CreatedOn = DateTime.UtcNow.AddDays(-10)},
                new Ad {Id = 7, CreatedOn = DateTime.UtcNow.AddDays(-30)},
                new Ad {Id = 8, CreatedOn = DateTime.UtcNow},
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync();

            //Assert
            Assert.Equal(expected[0], actual[0]);
            Assert.Equal(expected[1], actual[1]);
            Assert.Equal(expected[2], actual[2]);
            Assert.Equal(expected[3], actual[3]);
            Assert.Equal(expected[4], actual[4]);
            Assert.Equal(expected[5], actual[5]);
            Assert.Equal(expected[6], actual[6]);
            Assert.Equal(expected[7], actual[7]);
            Assert.Equal(expected[8], actual[8]);
            Assert.Equal(expected[9], actual[9]);
        }

        [Fact]
        public async Task GetAllActiveAdsCountAsync_WithValidData_ShouldReturnCorrectCount()
        {
            //Arrange
            var expected = 5;

            var moqAddressService = new Mock<IAddressesService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqCategoriesService = new Mock<ICategoriesService>();
            var moqUpdatesService = new Mock<IUpdatesService>();
            var moqSubcategoriesService = new Mock<ISubCategoriesService>();
            var moqMapper = new Mock<IMapper>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAds = new List<Ad>
            {
                new Ad {Id = 1, IsDeleted = false, IsApproved = true },
                new Ad {Id = 2, IsDeleted = false, IsApproved = true },
                new Ad {Id = 3, IsDeleted = false, IsApproved = true },
                new Ad {Id = 4, IsDeleted = false, IsApproved = true },
                new Ad {Id = 5, IsDeleted = false, IsApproved = true },
                new Ad {Id = 6, IsDeleted = true, IsApproved = true },
                new Ad {Id = 7, IsDeleted = false, IsApproved = false },
            };

            await context.Ads.AddRangeAsync(testingAds);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object,
                moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object,
                moqCloudinaryService.Object);

            //Act
            var actual = await this.adsService.GetAllActiveAdsCountAsync();

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
