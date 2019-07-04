namespace SellMe.Services
{
    using System.Security.Claims;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data.Models;
    using SellMe.Services.Utilities;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using SellMe.Web.ViewModels.ViewModels.Addresses;

    public class AdsService : IAdsService
    {
        private readonly SellMeDbContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IAddressService addressService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;

        public AdsService(SellMeDbContext context, IHttpContextAccessor contextAccessor, IAddressService addressService, IMapper mapper, IUsersService usersService)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.addressService = addressService;
            this.mapper = mapper;
            this.usersService = usersService;
        }

        public void CreateAd(CreateAdInputModel inputModel)
        {

            //TODO: Export this into separate method
            var imageUrls = inputModel.CreateAdDetailInputModel.Images
                .Select(x => this.UploadImages(x, inputModel.CreateAdDetailInputModel.Title))
                .ToList();

            var ad = this.mapper.Map<Ad>(inputModel);
            ad.Images = imageUrls.Select(x => new Image { ImageUrl = x.Result })
                .ToList();
            ad.SellerId = this.usersService.GetCurrentUserId();

            this.context.Ads.Add(ad);
            this.context.SaveChanges();
        }

        public AdsAllViewModel GetAllAdViewModels()
        {
            var adsViewModel = this.GetAllAdsViewModel();
            var allCategoriesViewModel = this.GetAllCategoryViewModel();

            var adsAllViewModel = this.CreateAdsAllViewModel(adsViewModel, allCategoriesViewModel);

            return adsAllViewModel;
        }

        public AdsByCategoryViewModel GetAdsByCategoryViewModel(int categoryId)
        {
            var adsViewModel = this.GetAllAdsByCategory(categoryId);
            var allCategoriesViewModel = this.GetAllCategoryViewModel();
            string categoryName = this.GetCategoryNameById(categoryId);

            var adsByCategoryViewModel = this.CreateAdsByCategoryViewModel(adsViewModel, allCategoriesViewModel, categoryName);

            return adsByCategoryViewModel;
        }

        private string GetCategoryNameById(int categoryId)
        {
            var categoryName = this.context.Categories.FirstOrDefault(x => x.Id == categoryId)?.Name;
            return categoryName;
        }

        public AdDetailsViewModel GetAdDetailsViewModel(int adId)
        {
            var adFromDb = this.GetAdById(adId);
            var addressForGivenAd = this.addressService.GetAddressByAdId(adFromDb.AddressId);

            //TODO: Map with auto mapper nested objects
            var adDetailsViewModel = mapper.Map<AdDetailsViewModel>(adFromDb);
            var addressViewModel = mapper.Map<AddressViewModel>(addressForGivenAd);

            adDetailsViewModel.AddressViewModel = addressViewModel;

            return adDetailsViewModel;
        }

        private Ad GetAdById(int adId)
        {
            //TODO: Validate for null

            var ad = this.context.Ads
                .FirstOrDefault(x => x.Id == adId);

            return ad;
        }

        public ICollection<MyActiveAdsViewModel> GetMyAdsViewModels()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var adsForCurrentUser = this.GetActiveAdsByUserId(currentUserId);

            var adsForCurrentUserViewModels = adsForCurrentUser
                .To<MyActiveAdsViewModel>()
                .ToList();

            return adsForCurrentUserViewModels;
        }

        public bool ArchiveAdById(int adId)
        {
            var ad = this.GetAdById(adId);

            if (ad.IsDeleted == true)
            {
                return false;
            }
            ad.IsDeleted = true;

            this.context.Update(ad);
            this.context.SaveChanges();

            return true;
        }

        public ICollection<MyArchivedAdsViewModel> GetMyArchivedAdsViewModels()
        {
            string currentUserId = this.usersService.GetCurrentUserId();

            var archivedAds = this.GetArchivedAdsByUserId(currentUserId);

            var archivedAdsForCurrentUserViewModels = archivedAds
                .To<MyArchivedAdsViewModel>()
                .ToList();

            return archivedAdsForCurrentUserViewModels;
        }

        public bool ActivateAdById(int adId)
        {
            var ad = this.GetAdById(adId);

            if (!ad.IsDeleted)
            {
                return false;
            }
            ad.IsDeleted = false;

            this.context.Update(ad);
            this.context.SaveChanges();

            return true;
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
            //TODO: Implement auto mapper!

            var adsByCategoryViewModel = new AdsByCategoryViewModel
            {
                CategoryName = categoryName,
                AdsViewModels = adsViewModel,
                Categories = allCategoriesViewModel
            };

            return adsByCategoryViewModel;
        }

        private ICollection<AdViewModel> GetAllAdsByCategory(int categoryId)
        {
            var adsViewModel = this.context
                .Ads
                .Where(x => x.CategoryId == categoryId)
                .To<AdViewModel>()
                .ToList();

            return adsViewModel;
        }

        private AdsAllViewModel CreateAdsAllViewModel(ICollection<AdViewModel> adsViewModel, ICollection<CategoryViewModel> allCategoriesViewModel)
        {
            //TODO: Map with auto mapper

            var adsAllViewModel = new AdsAllViewModel()
            {
                AdsViewModels = adsViewModel,
                Categories = allCategoriesViewModel
            };

            return adsAllViewModel;
        }

        private ICollection<CategoryViewModel> GetAllCategoryViewModel()
        {
            var allCategories = this.context
                .Categories
                .To<CategoryViewModel>()
                .ToList();

            return allCategories;
        }

        private ICollection<AdViewModel> GetAllAdsViewModel()
        {
            var adsViewModel = this.context
                .Ads
                .To<AdViewModel>()
                .ToList();

            return adsViewModel;

        }

        private async Task<string> UploadImages(IFormFile inputModelImage, string title)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();

            var url = await CloudinaryHelper.UploadImage(cloudinary, inputModelImage, title);

            return url;
        }

        Ad IAdsService.GetAdById(int adId)
        {
            var ad = this.context
                .Ads
                .FirstOrDefault(x => x.Id == adId);

            return ad;
        }
    }
}
