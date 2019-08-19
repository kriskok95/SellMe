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
    using CloudinaryDotNet.Actions;
    using SellMe.Web.ViewModels.BindingModels.Favorites;
    using System;
    using SellMe.Common;

    public class AdsService : IAdsService
    {
        private const string InvalidAdIdErrorMessage = "Ad with the given id doesn't exist!";
        private const string InvalidRejectionIdMessage = "Ad Rejection with the given id doesn't exist!";
        private const string AlreadyApprovedAdErrorMessage = "The given ad is already approved!";
        private const int CreatedAdsStatisticDaysCount = 10;

        private readonly SellMeDbContext context;
        private readonly IAddressesService _addressesService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly IUpdatesService updatesService;
        private readonly UserManager<SellMeUser> userManager;
        private readonly ISubCategoriesService subCategoriesService;

        public AdsService(SellMeDbContext context, IAddressesService addressesService, IMapper mapper, IUsersService usersService, ICategoriesService categoriesService, IUpdatesService updatesService, UserManager<SellMeUser> userManager, IHttpContextAccessor contextAccessor, ISubCategoriesService subCategoriesService)
        {
            this.context = context;
            this._addressesService = addressesService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.updatesService = updatesService;
            this.userManager = userManager;
            this.subCategoriesService = subCategoriesService;
        }

        public async Task CreateAdAsync(CreateAdInputModel inputModel)
        {
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

        public async Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId, int pageNumber, int pageSize)
        {
            var adsViewModel = this.GetAllAdsByCategoryAsync(categoryId);
            var paginatedAdsViewModel =
                await PaginatedList<AdViewModel>.CreateAsync(adsViewModel, pageNumber, pageSize);

            var allCategoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();
            var subcategoryViewModels = await this.subCategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(categoryId);
            string categoryName = await this.categoriesService.GetCategoryNameByIdAsync(categoryId);

            var adsByCategoryViewModel = this.CreateAdsByCategoryViewModel(paginatedAdsViewModel, allCategoriesViewModel, categoryName, subcategoryViewModels, categoryId);

            return adsByCategoryViewModel;
        }

        public async Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId)
        {
            var adFromDb = await this.GetAdByIdAsync(adId);
            await CreateViewForAdAsync(adFromDb);
            var addressForGivenAd = await this._addressesService.GetAddressByIdAsync(adFromDb.AddressId);

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
                .Where(x => x.PromotionOrders.Any(y => y.IsActive) && x.IsApproved && !x.IsDeleted)
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
                .Where(x => !x.IsDeleted && x.IsApproved)
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
                .Where(x => x.Title.Contains(searchText) && x.IsApproved)
                .To<AdViewModel>()
                .ToListAsync();

            var adsBySearchViewModel = new AdsBySearchViewModel
            {
                AdsBySearchViewModels = adViewModels
            };

            return adsBySearchViewModel;
        }

        public async Task<AdsByUserBindingModel> GetAdsByUserBindingModelAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            var adByUserViewModels = await this.context
                .Ads
                .Where(x => x.SellerId == userId && !x.IsDeleted && x.ActiveTo >= DateTime.UtcNow && x.IsApproved)
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

        public async Task EditAdById(EditAdInputModel inputModel)
        {
            var adFromDb = await this.context.Ads.FirstOrDefaultAsync(x => x.Id == inputModel.AdId);

            if (adFromDb == null)
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            var imageUrls = inputModel.EditAdDetailsInputModel.Images
                .Select(x => this.UploadImages(x, inputModel.EditAdDetailsInputModel.Title))
                .ToList();

            adFromDb.Title = inputModel.EditAdDetailsInputModel.Title;
            adFromDb.Description = inputModel.EditAdDetailsInputModel.Description;
            adFromDb.Price = inputModel.EditAdDetailsInputModel.Price;
            adFromDb.AvailabilityCount = inputModel.EditAdDetailsInputModel.Availability;
            adFromDb.ConditionId = inputModel.EditAdDetailsInputModel.ConditionId;
            //adFromDb.Images = imageUrls.Select(x => new Image { ImageUrl = x.Result})
            //    .ToList();

            foreach (var image in imageUrls)
            {
                adFromDb.Images.Add(new Image { ImageUrl = image.Result});
            }

            adFromDb.Address.Country = inputModel.EditAdAddressInputModel.Country;
            adFromDb.Address.City = inputModel.EditAdAddressInputModel.City;
            adFromDb.Address.Street = inputModel.EditAdAddressInputModel.Street;
            adFromDb.Address.District = inputModel.EditAdAddressInputModel.District;
            adFromDb.Address.ZipCode = inputModel.EditAdAddressInputModel.ZipCode;
            adFromDb.Address.PhoneNumber = inputModel.EditAdAddressInputModel.PhoneNumber;
            adFromDb.Address.EmailAddress = inputModel.EditAdAddressInputModel.EmailAddress;

            

            context.Ads.Update(adFromDb);
            await context.SaveChangesAsync();
        }

        public async Task<AdsForApprovalViewModel> GetAdsForApprovalViewModelsAsync(int pageNumber, int pageSize)
        {
            var adForApprovalViewModels = this.context
                .Ads
                .Where(x => !x.IsApproved && !x.IsDeclined)
                .OrderBy(x => x.CreatedOn)
                .To<AdForApprovalViewModel>();

            var paginatedListViewModels = await PaginatedList<AdForApprovalViewModel>.CreateAsync(adForApprovalViewModels, pageNumber, pageSize);

            var adsForApprovalViewModel = new AdsForApprovalViewModel
            {
                AdsAdForApprovalViewModels = paginatedListViewModels
            };


            return adsForApprovalViewModel;
        }

        public async Task<bool> ApproveAdAsync(int adId)
        {
            var adFromDb = await this.context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == adId);

            if (adFromDb == null)
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            if (adFromDb.IsApproved)
            {
                throw new InvalidOperationException(AlreadyApprovedAdErrorMessage);
            }

            adFromDb.IsApproved = true;
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<object> GetRejectAdBindingModelAsync(int adId)
        {
            var adFromDb = await this.context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == adId && !x.IsDeleted);

            var rejectAdViewModel = this.mapper.Map<RejectAdViewModel>(adFromDb);

            var rejectAdBindingModel = new RejectAdBindingModel
            {
                ViewModel = rejectAdViewModel
            };

            return rejectAdBindingModel;
        }

        public async Task CreateAdRejectionAsync(int adId, string comment)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            var adFromDb = await this.context.Ads.FirstOrDefaultAsync(x => x.Id == adId);
            adFromDb.IsDeclined = true;
            this.context.Update(adFromDb);

            var adRejection = new AdRejection
            {
                AdId = adId,
                Comment = comment
            };

            await this.context.AdRejections.AddAsync(adRejection);
            await this.context.SaveChangesAsync();
        }

        public async Task<PaginatedList<WaitingForApprovalByUserViewModel>> GetWaitingForApprovalByCurrentUserViewModels(int pageNumber, int pageSize)
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var waitingForApprovalViewModels = this.context
                .Ads
                .Where(x => x.SellerId == currentUserId && !x.IsApproved && !x.IsDeclined)
                .To<WaitingForApprovalByUserViewModel>();

            var paginatedListViewModels =
                await PaginatedList<WaitingForApprovalByUserViewModel>.CreateAsync(waitingForApprovalViewModels,
                    pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<PaginatedList<RejectedByUserAdViewModel>> GetRejectedAdByUserViewModelsAsync(int pageNumber, int pageSize)
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var rejectedAdByUserViewModels = this.context
                .AdRejections
                .Where(x => x.Ad.SellerId == currentUserId && x.Ad.IsDeclined && !x.IsDeleted)
                .To<RejectedByUserAdViewModel>();

            var paginatedListViewModels =
                await PaginatedList<RejectedByUserAdViewModel>.CreateAsync(rejectedAdByUserViewModels, pageNumber,
                    pageSize);

            return paginatedListViewModels;
        }

        public async Task<bool> SubmitRejectedAdAsync(int rejectionId)
        {
            if (!await this.context.AdRejections.AnyAsync(x => x.Id == rejectionId))
            {
                throw new ArgumentException(InvalidRejectionIdMessage);
            }

            var rejectionFromDb = await this.context
                .AdRejections
                .FirstOrDefaultAsync(x => x.Id == rejectionId);

            if (!await this.context.Ads.AnyAsync(x => x.Id == rejectionFromDb.AdId))
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            var adFromDb = await this.context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == rejectionFromDb.AdId);

            adFromDb.IsDeclined = false;

            rejectionFromDb.IsDeleted = true;
            this.context.Update(rejectionFromDb);
            this.context.Update(adFromDb);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedList<RejectedAdAllViewModel>> GetRejectedAdAllViewModelsAsync(int pageNumber, int pageSize)
        {
            var rejectedAdAllViewModels = this.context.Ads
                .Where(x => x.IsDeclined)
                .OrderByDescending(x => x.CreatedOn)
                .To<RejectedAdAllViewModel>();

            var paginatedListViewModels =
                await PaginatedList<RejectedAdAllViewModel>.CreateAsync(rejectedAdAllViewModels, pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<PaginatedList<ActiveAdAllViewModel>> GetAllActiveAdViewModelsAsync(int pageNumber, int pageSize)
        {
            var activeAdAllViewModels = this.context
                .Ads
                .Where(x => !x.IsDeleted && x.IsApproved)
                .To<ActiveAdAllViewModel>();

            var paginatedListViewModels =
                await PaginatedList<ActiveAdAllViewModel>.CreateAsync(activeAdAllViewModels, pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<List<int>> GetTheCountForTheCreatedAdsForTheLastTenDays()
        {
            var adsCount = new List<int>();

            for (DateTime i = DateTime.UtcNow.AddDays(-CreatedAdsStatisticDaysCount); i <= DateTime.UtcNow; i = i.AddDays(1))
            {
                var currentDaysAdsCount = await this.context.Ads
                    .CountAsync(x => x.CreatedOn.DayOfYear == i.DayOfYear);

                adsCount.Add(currentDaysAdsCount);
            }

            return adsCount;
        }

        private void DeleteImages(ICollection<Image> images)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();

            //var resultImages = images.Select(x => x.ImageUrl.Split('/', '.', x));

            var delParams = new DelResParams
            {
                PublicIds = images.Select(x => x.ImageUrl).ToList()
            };


            CloudinaryHelper.DeleteImages(cloudinary, delParams);
        }

        private IQueryable<Ad> GetAdsBySubcategory(int subcategoryId)
        {
            var adsBySubcategory = this.context
                .Ads
                .Where(x => x.SubCategoryId == subcategoryId && x.IsApproved);

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
            var addressFromDb = await this._addressesService.GetAddressByIdAsync(adId);

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
                .Where(x => x.SellerId == userId && !x.IsDeleted && x.IsApproved);

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
                .Where(x => x.CategoryId == categoryId && x.IsApproved)
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

        private async Task<string> UploadImages(IFormFile inputModelImage, string title)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();

            var url = await CloudinaryHelper.UploadImage(cloudinary, inputModelImage, title);

            return url;
        }
    }
}
