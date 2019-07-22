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
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using SellMe.Web.ViewModels.ViewModels.Addresses;
    using SellMe.Web.ViewModels.BindingModels.Ads;
    using System;
    using SellMe.Common;

    public class AdsService : IAdsService
    {
        private readonly SellMeDbContext context;
        private readonly IAddressService addressService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<SellMeUser> userManager;

        public AdsService(SellMeDbContext context, IAddressService addressService, IMapper mapper, IUsersService usersService, ICategoriesService categoriesService, UserManager<SellMeUser> userManager, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.addressService = addressService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
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

        public async Task<AdsAllViewModel> GetAllAdViewModelsAsync()
        {
            var adsViewModel = await this.GetAllAdsViewModelAsync();
            var allCategoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();

            var adsAllViewModel = this.CreateAdsAllViewModel(adsViewModel, allCategoriesViewModel);

            return adsAllViewModel;
        }

        public async Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId)
        {
            var adsViewModel = await this.GetAllAdsByCategoryAsync(categoryId);
            var allCategoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();
            string categoryName = this.categoriesService.GetCategoryNameById(categoryId);

            var adsByCategoryViewModel = this.CreateAdsByCategoryViewModel(adsViewModel, allCategoriesViewModel, categoryName);

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

        public async Task<ICollection<MyActiveAdsViewModel>> GetMyAdsViewModelsAsync()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var adsForCurrentUser = this.GetActiveAdsByUserId(currentUserId);

            var adsForCurrentUserViewModels = await adsForCurrentUser
                .To<MyActiveAdsViewModel>()
                .OrderBy(x => x.ActiveTo)
                .ToListAsync();

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

        public async Task<ICollection<MyArchivedAdsViewModel>> GetMyArchivedAdsViewModelsAsync()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var archivedAds = this.GetArchivedAdsByUserId(currentUserId);

            var archivedAdsForCurrentUserViewModels = await archivedAds
                .To<MyArchivedAdsViewModel>()
                .ToListAsync();

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

        private EditAdDetailsViewModel GetEditAdViewModelById(int adId)
        {
            var adFromDb = this.GetAdByIdAsync(adId);

            var editAdDetailsViewModel = this.mapper.Map<EditAdDetailsViewModel>(adFromDb);

            return editAdDetailsViewModel;
        }

        public EditAdBindingModel GetEditAdBindingModelById(int adId)
        {
            var editAdDetailsViewModel = this.GetEditAdViewModelById(adId);

            var editAdAddressViewModel = this.GetEditAdAddressViewModelById(adId);

            var editAdViewModel = this.GetEditAdViewModel(editAdDetailsViewModel, editAdAddressViewModel);

            var editAdBindingModel = new EditAdBindingModel
            {
                EditAdViewModel = editAdViewModel
            };

            return editAdBindingModel;
        }

        public async Task<ICollection<FavoriteAdViewModel>> GetFavoriteAdsByUserIdAsync(string loggedInUserId)
        {
            var currentUser = await this.userManager.FindByIdAsync(loggedInUserId);
            var favoriteAdsViewModels = currentUser
                .SellMeUserFavoriteProducts.Select(x => x.Ad)
                .AsQueryable()
                .To<FavoriteAdViewModel>()
                .ToList();

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

            adFromDb.CreatedOn = DateTime.UtcNow;
            adFromDb.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
            this.context.Update(adFromDb);
            await this.context.SaveChangesAsync();
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

        private EditAdAddressViewModel GetEditAdAddressViewModelById(int adId)
        {
            var addressFromDb = this.addressService.GetAddressByAdIdAsync(adId);

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

        private AdsByCategoryViewModel CreateAdsByCategoryViewModel(ICollection<AdViewModel> adsViewModel, ICollection<CategoryViewModel> allCategoriesViewModel, string categoryName)
        {
            var adsByCategoryViewModel = new AdsByCategoryViewModel
            {
                CategoryName = categoryName,
                AdsViewModels = adsViewModel
            };

            return adsByCategoryViewModel;
        }

        private async Task<ICollection<AdViewModel>> GetAllAdsByCategoryAsync(int categoryId)
        {
            var adsViewModel = await this.context
                .Ads
                .Where(x => x.CategoryId == categoryId)
                .To<AdViewModel>()
                .ToListAsync();

            return adsViewModel;
        }

        private AdsAllViewModel CreateAdsAllViewModel(ICollection<AdViewModel> adsViewModel, ICollection<CategoryViewModel> allCategoriesViewModel)
        {
            var adsAllViewModel = new AdsAllViewModel()
            {
                AdsViewModels = adsViewModel
            };

            return adsAllViewModel;
        }

        private async  Task<ICollection<AdViewModel>> GetAllAdsViewModelAsync()
        {
            var adsViewModel = await this.context
                .Ads
                .To<AdViewModel>()
                .ToListAsync();

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
