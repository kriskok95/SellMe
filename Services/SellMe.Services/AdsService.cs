namespace SellMe.Services
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using SellMe.Data.Models;
    using SellMe.Services.Utilities;
    using System.Threading.Tasks;
    using SellMe.Services.Paging;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using SellMe.Web.ViewModels.ViewModels.Addresses;
    using SellMe.Web.ViewModels.BindingModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;
    using SellMe.Web.ViewModels.BindingModels.Favorites;
    using System;
    using SellMe.Common;

    public class AdsService : IAdsService
    {
        private readonly SellMeDbContext context;
        private readonly IAddressService addressService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly IUpdatesService updatesService;
        private readonly UserManager<SellMeUser> userManager;
        private readonly ISubCategoriesService subCategoriesService;

        public AdsService(SellMeDbContext context, IAddressService addressService, IMapper mapper, IUsersService usersService, ICategoriesService categoriesService, IUpdatesService updatesService, UserManager<SellMeUser> userManager, IHttpContextAccessor contextAccessor, ISubCategoriesService subCategoriesService)
        {
            this.context = context;
            this.addressService = addressService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.updatesService = updatesService;
            this.userManager = userManager;
            this.subCategoriesService = subCategoriesService;
        }

        public async Task CreateAdAsync(CreateAdInputModel inputModel)
        {

            //TODO: Export this into separate method
            var imageUrls = inputModel.CreateAdDetailInputModel.Images
                .Select(x => this.UploadImages(x, inputModel.CreateAdDetailInputModel.Title))
                .ToList();

            var ad = this.mapper.Map<Ad>(inputModel);
            ad.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
            ad.Images = imageUrls.Select(x => new Image { ImageUrl = x.Result })
                .ToList();
            ad.SellerId = this.usersService.GetCurrentUserId();

            await this.context.Ads.AddAsync(ad);
            await this.context.SaveChangesAsync();
        }

        public async Task<AdsAllViewModel> GetAllAdViewModelsAsync(int pageNumber, int pageSize)
        {
            var adsViewModels = this.GetAllAdsViewModel();
            var allCategoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();

            var paginatedAds = await PaginatedList<AdViewModel>
                .CreateAsync(adsViewModels, pageNumber, pageSize);

            var adsAllViewModel = this.CreateAdsAllViewModel(paginatedAds, allCategoriesViewModel);

            return adsAllViewModel;
        }

        public async Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId, int pageNumber, int pageSize)
        {
            var adsViewModel = this.GetAllAdsByCategoryAsync(categoryId);
            var paginatedAdsViewModel =
                await PaginatedList<AdViewModel>.CreateAsync(adsViewModel, pageNumber, pageSize);

            var allCategoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();
            var subcategoryViewModels = await this.subCategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(categoryId);
            string categoryName = this.categoriesService.GetCategoryNameById(categoryId);

            var adsByCategoryViewModel = this.CreateAdsByCategoryViewModel(paginatedAdsViewModel, allCategoriesViewModel, categoryName, subcategoryViewModels, categoryId);

            return adsByCategoryViewModel;
        }

        public async Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId)
        {
            var adFromDb = await this.GetAdByIdAsync(adId);
            await CreateViewForAdAsync(adFromDb);
            var addressForGivenAd = await this.addressService.GetAddressByAdIdAsync(adFromDb.AddressId);

            var adDetailsViewModel = mapper.Map<AdDetailsViewModel>(adFromDb);
            var addressViewModel = mapper.Map<AddressViewModel>(addressForGivenAd);
            adDetailsViewModel.Observed = await this.GetObservedAdsByAdIdAsync(adId);

            adDetailsViewModel.AddressViewModel = addressViewModel;

            return adDetailsViewModel;
        }

        private Task<int> GetObservedAdsByAdIdAsync(int adId)
        {
            var observed = this.context
                .SellMeUserFavoriteProducts
                .CountAsync(x => x.AdId == adId);

            return observed;
        }

        private async Task CreateViewForAdAsync(Ad ad)
        {
            var adView = new AdView()
            {
                AdId = ad.Id
            };
            await this.context.AdViews.AddAsync(adView);
            await this.context.SaveChangesAsync();
        }

        public async Task<Ad> GetAdByIdAsync(int adId)
        {
            var ad = await this.context.Ads
                .FirstOrDefaultAsync(x => x.Id == adId);

            return ad;
        }

        private IQueryable<MyActiveAdsViewModel> GetMyAdsViewModels()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var adsForCurrentUser = this.GetActiveAdsByUserId(currentUserId);

            var adsForCurrentUserViewModels = adsForCurrentUser
                .OrderBy(x => x.ActiveTo)
                .To<MyActiveAdsViewModel>();

            return adsForCurrentUserViewModels;
        }

        public async Task<bool> ArchiveAdByIdAsync(int adId)
        {
            var ad = await this.GetAdByIdAsync(adId);

            if (ad.IsDeleted == true)
            {
                return false;
            }
            ad.IsDeleted = true;

            this.context.Update(ad);
            await this.context.SaveChangesAsync();

            return true;
        }

        private IQueryable<MyArchivedAdsViewModel> GetMyArchivedAdsViewModels()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var archivedAds = this.GetArchivedAdsByUserId(currentUserId);

            var archivedAdsForCurrentUserViewModels = archivedAds
                .To<MyArchivedAdsViewModel>();

            return archivedAdsForCurrentUserViewModels;
        }

        public async Task<bool> ActivateAdById(int adId)
        {
            var ad = await this.GetAdByIdAsync(adId);

            if (!ad.IsDeleted)
            {
                return false;
            }
            ad.IsDeleted = false;
            ad.CreatedOn = DateTime.UtcNow;
            ad.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);

            this.context.Update(ad);
            await this.context.SaveChangesAsync();

            return true;
        }

        private async Task<EditAdDetailsViewModel> GetEditAdViewModelByIdAsync(int adId)
        {
            var adFromDb = await this.GetAdByIdAsync(adId);

            var editAdDetailsViewModel = this.mapper.Map<EditAdDetailsViewModel>(adFromDb);

            return editAdDetailsViewModel;
        }

        public async Task<EditAdBindingModel> GetEditAdBindingModelById(int adId)
        {
            var editAdDetailsViewModel = await this.GetEditAdViewModelByIdAsync(adId);

            var editAdAddressViewModel = await this.GetEditAdAddressViewModelByIdAsync(adId);

            var editAdViewModel = this.GetEditAdViewModel(editAdDetailsViewModel, editAdAddressViewModel);

            var editAdBindingModel = new EditAdBindingModel
            {
                EditAdViewModel = editAdViewModel
            };

            return editAdBindingModel;
        }

        private IQueryable<FavoriteAdViewModel> GetFavoriteAdsByUser(SellMeUser user)
        {
            var favoriteAdsViewModels = this.context
                .SellMeUserFavoriteProducts
                .Where(x => x.SellMeUserId == user.Id)
                .To<FavoriteAdViewModel>();

            return favoriteAdsViewModels;
        }

        public string GetAdTitleById(int adId)
        {
            var adTitle = this.context.Ads.FirstOrDefault(x => x.Id == adId)? .Title;

            return adTitle;
        }

        public  async Task UpdateAdByIdAsync(int adId)
        {
            var adFromDb = await this.GetAdByIdAsync(adId);

            if (adFromDb.Updates > 0)
            {
                adFromDb.ActiveFrom = DateTime.UtcNow;
                adFromDb.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
                adFromDb.Updates--;

                await this.updatesService.CreateUpdateAd(adId);

                this.context.Update(adFromDb);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<PromotedAdViewModel>> GetPromotedAdViewModels()
        {
            var promotedAds = await this.context
                .Ads
                .Where(x => x.PromotionOrders.Any(y => y.IsActive))
                .ToListAsync();

            var numbersCount = promotedAds.Count < GlobalConstants.PromotedAdsCountAtIndexPage
                ? promotedAds.Count
                : GlobalConstants.PromotedAdsCountAtIndexPage;

            List<int> distinctRandomNumbers = GetDistinctRandomNumbersInRange(0, promotedAds.Count, numbersCount);

            var randomPromotedAds = new List<Ad>();

            foreach (var index in distinctRandomNumbers)
            {
                randomPromotedAds.Add(promotedAds[index]);
            }

            var promotedAdViewModels = randomPromotedAds.AsQueryable()
                .To<PromotedAdViewModel>()
                .ToList();

            return promotedAdViewModels;
        }

        public async Task<ICollection<LatestAddedAdViewModel>> GetLatestAddedAdViewModels()
        {
            var latestAddedAdViewModels = await this.context
                .Ads
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted)
                .Take(GlobalConstants.LatestAddedAdsCountAtIndexPage)
                .To<LatestAddedAdViewModel>()
                .ToListAsync();

            return latestAddedAdViewModels;
        }

        public async Task<AdsBySubcategoryViewModel> GetAdsBySubcategoryViewModelAsync(int subcategoryId, int categoryId, int pageNumber, int pageSize)
        {
            var subcategories = await this.subCategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(categoryId);
            var adsBySubcategory = this.GetAdsBySubcategory(subcategoryId);

            var adsBySubcategoryViewModels = adsBySubcategory
                .To<AdViewModel>();

            var pagedSubcategoryViewModels =
                await PaginatedList<AdViewModel>.CreateAsync(adsBySubcategoryViewModels, pageNumber, pageSize);

            var adsBySubcategoryViewModel = new AdsBySubcategoryViewModel
            {
                AdsByCategorySubcategoryViewModels = subcategories.ToList(),
                AdsBySubcategoryViewModels = pagedSubcategoryViewModels,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
            };

            return adsBySubcategoryViewModel;
        }

        public async Task<MyActiveAdsBindingModel> GetMyActiveAdsBindingModelAsync(int pageNumber, int pageSize)
        {
            var myActiveAdViewModel = this.GetMyAdsViewModels();

            var paginatedActiveAdViewModels =
                await PaginatedList<MyActiveAdsViewModel>.CreateAsync(myActiveAdViewModel, pageNumber, pageSize);

            var bindingModel = new MyActiveAdsBindingModel
            {
                Ads = paginatedActiveAdViewModels,
            };

            return bindingModel;
        }

        public async Task<FavoriteAdsBindingModel> GetFavoriteAdsBindingModelAsync(string userId, int pageNumber, int pageSize)
        {
            var user = this.usersService.GetCurrentUser();

            var favoriteAdViewModels = this.GetFavoriteAdsByUser(user);

            var paginatedFavoriteAds =
                await PaginatedList<FavoriteAdViewModel>.CreateAsync(favoriteAdViewModels, pageNumber, pageSize);

            var favoriteAdsBindingModel = new FavoriteAdsBindingModel
            {
                Favorites = paginatedFavoriteAds
            };

            return favoriteAdsBindingModel;
        }

        public async Task<ArchivedAdsBindingModel> GetArchivedAdsBindingModelAsync(int pageNumber, int pageSize)
        {
            var archivedAdViewModels = this.GetMyArchivedAdsViewModels();

            var paginatedArchivedAdViewModels =
                await PaginatedList<MyArchivedAdsViewModel>.CreateAsync(archivedAdViewModels, pageNumber, pageSize);

            var archivedAdsBindingModel = new ArchivedAdsBindingModel
            {
                Ads = paginatedArchivedAdViewModels
            };

            return archivedAdsBindingModel;
        }

        public async Task<AdsBySearchViewModel> GetAdsBySearchViewModelAsync(string searchText)
        {
            var adViewModels = await this.context
                .Ads
                .Where(x => x.Title.Contains(searchText))
                .To<AdViewModel>()
                .ToListAsync();

            var adsBySearchViewModel = new AdsBySearchViewModel
            {
                AdsBySearchViewModels = adViewModels
            };

            return adsBySearchViewModel;
        }

        public async Task<AdsByUserBindingModel> GetAdsByUserBindingModel(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            var adByUserViewModels = await this.context
                .Ads
                .Where(x => x.SellerId == userId && !x.IsDeleted && x.ActiveTo >= DateTime.UtcNow)
                .To<AdViewModel>()
                .ToListAsync();

            var adsByUserBindingModel = new AdsByUserBindingModel
            {
                UserId = user.Id,
                Username = user.UserName,
                AdViewModels = adByUserViewModels
            };

            return adsByUserBindingModel;
        }

        private IQueryable<Ad> GetAdsBySubcategory(int subcategoryId)
        {
            var adsBySubcategory = this.context
                .Ads
                .Where(x => x.SubCategoryId == subcategoryId);

            return adsBySubcategory;
        }

        private List<int> GetDistinctRandomNumbersInRange(int fromNumber, int toNumber, int numbersCount)
        {
            var random = new Random();
            List<int> distinctRandomNumbers = new List<int>();

            for (int i = fromNumber; i < numbersCount; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = random.Next(0, toNumber);
                } while (distinctRandomNumbers.Contains(randomNumber));
                distinctRandomNumbers.Add(randomNumber);
            }

            return distinctRandomNumbers;
        }

        private EditAdViewModel GetEditAdViewModel(EditAdDetailsViewModel editAdDetailsViewModel, EditAdAddressViewModel editAdAddressViewModel)
        {
            var editAdViewModel = new EditAdViewModel
            {
                EditAdDetailsViewModel = editAdDetailsViewModel,
                EditAdAddressViewModel = editAdAddressViewModel
            };

            return editAdViewModel;
        }

        private async Task<EditAdAddressViewModel> GetEditAdAddressViewModelByIdAsync(int adId)
        {
            var addressFromDb = await this.addressService.GetAddressByAdIdAsync(adId);

            var editAdAddressViewModel = this.mapper.Map<EditAdAddressViewModel>(addressFromDb);

            return editAdAddressViewModel;
        }

        private IQueryable<Ad> GetArchivedAdsByUserId(string userId)
        {
            var adsByUser = this.context
                .Ads
                .Where(x => x.SellerId == userId && x.IsDeleted);

            return adsByUser;
        }

        private IQueryable<Ad> GetActiveAdsByUserId(string userId)
        {
            var adsByUser = this.context
                .Ads
                .Where(x => x.SellerId == userId && !x.IsDeleted);

            return adsByUser;
        }

        private AdsByCategoryViewModel CreateAdsByCategoryViewModel(PaginatedList<AdViewModel> paginatedAdViewModels,
            ICollection<CategoryViewModel> allCategoriesViewModel, string categoryName,
            ICollection<AdsByCategorySubcategoryViewModel> subcategoryViewModels, int categoryId)
        {
            var adsByCategoryViewModel = new AdsByCategoryViewModel
            {
                CategoryId = categoryId,
                CategoryName = categoryName,
                AdsViewModels = paginatedAdViewModels,
                AdsByCategorySubcategoryViewModels = subcategoryViewModels.ToList(),
            };

            return adsByCategoryViewModel;
        }

        private IQueryable<AdViewModel> GetAllAdsByCategoryAsync(int categoryId)
        {
            var adsViewModel = this.context
                .Ads
                .Where(x => x.CategoryId == categoryId)
                .To<AdViewModel>();

            return adsViewModel;
        }

        private AdsAllViewModel CreateAdsAllViewModel(PaginatedList<AdViewModel> adsViewModel, ICollection<CategoryViewModel> allCategoriesViewModel)
        {
            var adsAllViewModel = new AdsAllViewModel()
            {
                AdsViewModels = adsViewModel
            };

            return adsAllViewModel;
        }

        private IQueryable<AdViewModel> GetAllAdsViewModel()
        {
            var adsViewModel = this.context
                .Ads
                .To<AdViewModel>();

            return adsViewModel;
        }

        private async Task<string> UploadImages(IFormFile inputModelImage, string title)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();

            var url = await CloudinaryHelper.UploadImage(cloudinary, inputModelImage, title);

            return url;
        }
    }
}
