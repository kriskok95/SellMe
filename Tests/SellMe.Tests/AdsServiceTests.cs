using SellMe.Web.ViewModels.ViewModels.Subcategories;

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
                }
            };

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

        //[Fact]
        //public async Task GetAdsByCategoryViewModelAsync_WithValidData_ShouldReturnCorrectResult()
        //{

        //    //Arrange
        //    var expectedAdsCount = 2;

        //    var moqAddressService = new Mock<IAddressesService>();
        //    var moqUsersService = new Mock<IUsersService>();
        //    var moqCategoriesService = new Mock<ICategoriesService>();
        //    moqCategoriesService.Setup(x => x.GetAllCategoryViewModelsAsync())
        //        .ReturnsAsync(new List<CategoryViewModel>
        //        {
        //            new CategoryViewModel
        //            {
        //                Id = 1,
        //                Name = "Electronics"
        //            },
        //            new CategoryViewModel
        //            {
        //                Id = 2,
        //                Name = "Animals"
        //            },
        //            new CategoryViewModel
        //            {
        //                Id = 3,
        //                Name = "Services"
        //            },
        //        });
        //    moqCategoriesService.Setup(x => x.GetCategoryNameByIdAsync(1))
        //        .ReturnsAsync("Electronics");

        //    var moqUpdatesService = new Mock<IUpdatesService>();
        //    var moqSubcategoriesService = new Mock<ISubCategoriesService>();
        //    moqSubcategoriesService.Setup(x => x.GetAdsByCategorySubcategoryViewModelsAsync(1))
        //        .ReturnsAsync(new List<AdsByCategorySubcategoryViewModel>
        //        {
        //            new AdsByCategorySubcategoryViewModel
        //            {
        //                Id = 1,
        //                Name = "Phones",
        //            },
        //            new AdsByCategorySubcategoryViewModel
        //            {
        //                Id = 2,
        //                Name = "Monitors"
        //            }
        //        });

        //    var moqMapper = new Mock<IMapper>();

        //    var context = InitializeContext.CreateContextForInMemory();
        //    this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

        //    var testingAds = new List<Ad>
        //    {
        //        new Ad
        //        {
        //            Id = 1,
        //            Title = "Iphone 6s",
        //            Description = "PerfectCondition",
        //            CategoryId = 1,
        //            IsApproved = true,
        //            ActiveFrom = DateTime.UtcNow,
        //            ActiveTo = DateTime.UtcNow.AddDays(30),
        //            AvailabilityCount = 1,
        //            Price = 120,
        //            Condition = new Condition { Name = "Brand New" },
        //            Address = new Address
        //            {
        //                Country = "Bulgaria",
        //                City = "Sofia",
        //                Street = "Ivan Vazov",
        //                District = "Student city",
        //                ZipCode = 1000,
        //                PhoneNumber = "0895335532",
        //                EmailAddress = "Ivan@gmail.com"
        //            }
        //        },
        //        new Ad
        //        {
        //            Id = 2,
        //            CategoryId = 1,
        //            Title = "Monitor LG",
        //            IsDeleted = false,
        //            IsApproved = true
        //        },
        //        new Ad
        //        {
        //            Id = 3,
        //            CategoryId = 4,
        //            Title = "German Shepherd",
        //            IsDeleted = false,
        //            IsApproved = true
        //        }
        //    };

        //    var testingCategories = new List<Category>
        //    {
        //        new Category
        //        {
        //            Id = 1,
        //            Name = "Electronics"
        //        },
        //        new Category
        //        {
        //            Id = 2,
        //            Name = "Animals"
        //        },
        //        new Category
        //        {
        //            Id = 3,
        //            Name = "Services"
        //        },
        //    };

        //    var testingSubcategories = new List<SubCategory>
        //    {
        //        new SubCategory
        //        {
        //            CategoryId = 1,
        //            Name = "Phones"
        //        },
        //        new SubCategory
        //        {
        //            CategoryId = 1,
        //            Name = "Monitors"
        //        },

        //    };

        //    await context.Categories.AddRangeAsync(testingCategories);
        //    await context.Ads.AddRangeAsync(testingAds);
        //    await context.SubCategories.AddRangeAsync(testingSubcategories);
        //    await context.SaveChangesAsync();

        //    //Act
        //    var actual = await this.adsService.GetAdsByCategoryViewModelAsync(1, 1, 10);

        //    //Asert
        //    Assert.Equal(expectedAdsCount, actual.AdsViewModels.Count);
        //}

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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
                moqMapper.Object);

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
                moqMapper.Object);

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
                moqMapper.Object);

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
                moqMapper.Object);

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
                moqMapper.Object);

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
                moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
            };

            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();
            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();

            var testingCategory = new Category
            {
                Id = 1, Name = "Electronics"
            };

            await context.Categories.AddAsync(testingCategory);
            await context.SaveChangesAsync();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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


            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

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

            var context = InitializeContext.CreateContextForInMemory();

            this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this.adsService.GetFavoriteAdsViewModelsAsync(userId, 1, 10));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        //[Fact]
        //public async Task GetFavoriteAdsViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        //{
        //    //Arrange
        //    var expectedCount = 2;

        //    var moqAddressService = new Mock<IAddressesService>();
        //    var moqUsersService = new Mock<IUsersService>();
        //    moqUsersService.Setup(x => x.GetCurrentUserId())
        //        .Returns("SellMeUserId");

        //    var moqCategoriesService = new Mock<ICategoriesService>();
        //    var moqUpdatesService = new Mock<IUpdatesService>();
        //    var moqSubcategoriesService = new Mock<ISubCategoriesService>();
        //    var moqMapper = new Mock<IMapper>();
        //    var context = InitializeContext.CreateContextForInMemory();

        //    var testingAds = new List<Ad>
        //    {
        //        new Ad
        //        {
        //            Id = 1,
        //            Title = "Iphone 6s",
        //            Price = 100,
        //            IsApproved = true,
        //            IsDeleted = false,
        //            SellerId = "SellMeUserId",
        //            Updates = 3,
        //            ActiveFrom = DateTime.UtcNow,
        //            ActiveTo = DateTime.UtcNow.AddDays(30),
        //            SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
        //            {
        //                new SellMeUserFavoriteProduct
        //                {
        //                    AdId = 1,
        //                    SellMeUserId = "SellMeUserId"
        //                }
        //            },
        //            Images = new List<Image>
        //            {
        //                new Image
        //                {
        //                    ImageUrl = "https://www.webpagetest.org1"
        //                }
        //            }
        //        },
        //        new Ad
        //        {
        //            Id = 2,
        //            Title = "Samsung TV",
        //            Price = 100,
        //            IsApproved = true,
        //            IsDeleted = false,
        //            SellerId = "SellMeUserId",
        //            Updates = 10,
        //            ActiveFrom = DateTime.UtcNow,
        //            ActiveTo = DateTime.UtcNow.AddDays(25),
        //            SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
        //            {
        //                new SellMeUserFavoriteProduct
        //                {
        //                    AdId = 1,
        //                    SellMeUserId = "SellMeUserId"
        //                }
        //            },
        //            Images = new List<Image>
        //            {
        //                new Image
        //                {
        //                    ImageUrl = "https://www.webpagetest.org2"
        //                }
        //            },
        //        },
        //        new Ad
        //        {
        //            Id = 4,
        //            Title = "Motorola phone",
        //            Price = 250,
        //            IsApproved = true,
        //            IsDeleted = true,
        //            SellerId = "FakeSellerId",
        //            Updates = 0,
        //            ActiveFrom = DateTime.UtcNow,
        //            ActiveTo = DateTime.UtcNow.AddDays(10),
        //            SellMeUserFavoriteProducts = new List<SellMeUserFavoriteProduct>
        //            {
        //                new SellMeUserFavoriteProduct
        //                {
        //                    AdId = 1,
        //                    SellMeUserId = "FakeSellerId"
        //                }
        //            },
        //            Images = new List<Image>
        //            {
        //                new Image
        //                {
        //                    ImageUrl = "https://www.webpagetest.org3"
        //                }
        //            },
        //        },
        //    };

        //    await context.Ads.AddRangeAsync(testingAds);
        //    await context.SaveChangesAsync();

        //    this.adsService = new AdsService(context, moqAddressService.Object, moqUsersService.Object, moqCategoriesService.Object, moqUpdatesService.Object, moqSubcategoriesService.Object, moqMapper.Object);

        //    //Act
        //    var actual = await this.adsService.GetFavoriteAdsViewModelsAsync("SellMeUserId", 1, 10);

        //    //Assert
        //    Assert.Equal(expectedCount, actual.Count);
        //}
    }
}
