namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Castle.Core.Internal;
    using Common;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Paging;
    using Web.ViewModels.BindingModels.Ads;
    using Web.ViewModels.InputModels.Ads;
    using Web.ViewModels.ViewModels.Addresses;
    using Web.ViewModels.ViewModels.Ads;
    using Web.ViewModels.ViewModels.Categories;
    using Web.ViewModels.ViewModels.Subcategories;

    public class AdsService : IAdsService
    {
        private const string InvalidRejectionIdMessage = "Ad Rejection with the given id doesn't exist!";
        private const string AlreadyApprovedAdErrorMessage = "The given ad is already approved!";
        private const string CommentNullOrEmptyErrorMessage = "The comment can't be null or empty string!";
        private const string NotOwnerOfAnAdErrorMessage = "You are not the owner of this ad!";

        private readonly SellMeDbContext context;
        private readonly IAddressesService addressesService;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly IUpdatesService updatesService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IMapper mapper;
        private readonly ICloudinaryService cloudinaryService;

        public AdsService(SellMeDbContext context, IAddressesService addressesService, IUsersService usersService,
            ICategoriesService categoriesService, IUpdatesService updatesService,
            ISubCategoriesService subCategoriesService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.addressesService = addressesService;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.updatesService = updatesService;
            this.subCategoriesService = subCategoriesService;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task CreateAdAsync(CreateAdInputModel inputModel)
        {
            var imageUrls = inputModel.CreateAdDetailInputModel.Images
                .Select(async x =>
                    await cloudinaryService.UploadPictureAsync(x, x.FileName))
                .Select(x => x.Result)
                .ToList();

            var ad = mapper.Map<Ad>(inputModel);
            ad.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
            ad.Images = imageUrls.Select(x => new Image {ImageUrl = x})
                .ToList();
            ad.SellerId = usersService.GetCurrentUserId();

            await context.Ads.AddAsync(ad);
            await context.SaveChangesAsync();
        }

        public async Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId, int pageNumber,
            int pageSize)
        {
            if (!await context.Categories.AnyAsync(x => x.Id == categoryId))
            {
                throw new ArgumentException(GlobalConstants.InvalidCategoryIdErrorMessage);
            }

            var adsViewModel = GetAllAdsByCategory(categoryId);
            var paginatedAdsViewModel =
                await PaginatedList<AdViewModel>.CreateAsync(adsViewModel, pageNumber, pageSize);

            var allCategoriesViewModel = await categoriesService.GetAllCategoryViewModelsAsync();
            var subcategoryViewModels =
                await subCategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(categoryId);
            string categoryName = await categoriesService.GetCategoryNameByIdAsync(categoryId);

            var adsByCategoryViewModel = CreateAdsByCategoryViewModel(paginatedAdsViewModel,
                allCategoriesViewModel, categoryName, subcategoryViewModels, categoryId);

            return adsByCategoryViewModel;
        }

        public async Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var adFromDb = await GetAdByIdAsync(adId);
            await CreateViewForAdAsync(adFromDb);
            var addressForGivenAd = await addressesService.GetAddressByIdAsync(adFromDb.AddressId);

            var adDetailsViewModel = mapper.Map<AdDetailsViewModel>(adFromDb);

            var rating = await usersService.GetRatingByUserAsync(adFromDb.SellerId);

            adDetailsViewModel.Rating = rating;

            var addressViewModel = mapper.Map<AddressViewModel>(addressForGivenAd);
            adDetailsViewModel.Observed = await GetObservedAdsByAdIdAsync(adId);

            adDetailsViewModel.AddressViewModel = addressViewModel;

            return adDetailsViewModel;
        }

        public async Task<Ad> GetAdByIdAsync(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var ad = await context.Ads
                .FirstOrDefaultAsync(x => x.Id == adId);

            return ad;
        }

        public async Task<bool> ArchiveAdByIdAsync(int adId)
        {
            var ad = await GetAdByIdAsync(adId);

            if (ad.IsDeleted)
            {
                return false;
            }

            ad.IsDeleted = true;

            context.Update(ad);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateAdByIdAsync(int adId)
        {
            var ad = await GetAdByIdAsync(adId);

            if (!ad.IsDeleted)
            {
                return false;
            }

            ad.IsDeleted = false;
            ad.CreatedOn = DateTime.UtcNow;
            ad.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);

            context.Update(ad);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<EditAdBindingModel> GetEditAdBindingModelById(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var adFromDb = await this.GetAdByIdAsync(adId);
            var currentUserId = this.usersService.GetCurrentUserId();

            if(adFromDb.SellerId != currentUserId)
            {
                throw new InvalidOperationException(NotOwnerOfAnAdErrorMessage);
            }

            var editAdDetailsViewModel = await GetEditAdViewModelByIdAsync(adId);

            var editAdAddressViewModel = await GetEditAdAddressViewModelByIdAsync(adId);

            var editAdViewModel = GetEditAdViewModel(editAdDetailsViewModel, editAdAddressViewModel);

            var editAdBindingModel = new EditAdBindingModel
            {
                EditAdViewModel = editAdViewModel
            };

            return editAdBindingModel;
        }

        public async Task<string> GetAdTitleByIdAsync(int adId)
        {

            var adFromDb = await GetAdByIdAsync(adId);

            var adTitle = context.Ads.FirstOrDefault(x => x.Id == adId)?.Title;

            return adTitle;
        }

        public async Task<bool> UpdateAdByIdAsync(int adId)
        {
            var adFromDb = await GetAdByIdAsync(adId);

            if (adFromDb.Updates > 0)
            {
                adFromDb.ActiveFrom = DateTime.UtcNow;
                adFromDb.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
                adFromDb.Updates--;

                await updatesService.CreateUpdateAdAsync(adId);

                context.Update(adFromDb);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ICollection<PromotedAdViewModel>> GetPromotedAdViewModelsAsync()
        {
            var promotedAds = await context
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

        public async Task<ICollection<LatestAddedAdViewModel>> GetLatestAddedAdViewModelsAsync()
        {
            var latestAddedAdViewModels = await context
                .Ads
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted && x.IsApproved)
                .Take(GlobalConstants.LatestAddedAdsCountAtIndexPage)
                .To<LatestAddedAdViewModel>()
                .ToListAsync();

            return latestAddedAdViewModels;
        }

        public async Task<AdsBySubcategoryViewModel> GetAdsBySubcategoryViewModelAsync(int subcategoryId,
            int categoryId, int pageNumber, int pageSize)
        {
            if (!await context.Categories.AnyAsync(x => x.Id == categoryId))
            {
                throw new ArgumentException(GlobalConstants.InvalidCategoryIdErrorMessage);
            }

            if (!await context.SubCategories.AnyAsync(x => x.CategoryId == categoryId && x.Id == subcategoryId))
            {
                throw new ArgumentException(GlobalConstants.InvalidSubcategoryIdErrorMessage);
            }

            var subcategories = await subCategoriesService.GetAdsByCategorySubcategoryViewModelsAsync(categoryId);
            var adsBySubcategory = GetAdsBySubcategory(subcategoryId);

            var adsBySubcategoryViewModels = adsBySubcategory
                .To<AdViewModel>();

            var pagedSubcategoryViewModels =
                await PaginatedList<AdViewModel>.CreateAsync(adsBySubcategoryViewModels, pageNumber, pageSize);

            var adsBySubcategoryViewModel = new AdsBySubcategoryViewModel
            {
                AdsByCategorySubcategoryViewModels = subcategories.ToList(),
                AdsBySubcategoryViewModels = pagedSubcategoryViewModels,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId
            };

            return adsBySubcategoryViewModel;
        }

        public async Task<PaginatedList<MyActiveAdsViewModel>> GetMyActiveAdsViewModelsAsync(int pageNumber,
            int pageSize)
        {
            var myActiveAdViewModel = GetMyAdsViewModels();

            var paginatedActiveAdViewModels =
                await PaginatedList<MyActiveAdsViewModel>.CreateAsync(myActiveAdViewModel, pageNumber, pageSize);

            return paginatedActiveAdViewModels;
        }

        public async Task<PaginatedList<FavoriteAdViewModel>> GetFavoriteAdsViewModelsAsync(string userId,
            int pageNumber, int pageSize)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentException(GlobalConstants.NullOrEmptyUserIdErrorMessage);
            }

            var user = await usersService.GetCurrentUserAsync();

            var favoriteAdViewModels = GetFavoriteAdViewModelsByUser(user);

            var paginatedFavoriteAds =
                await PaginatedList<FavoriteAdViewModel>.CreateAsync(favoriteAdViewModels, pageNumber, pageSize);

            return paginatedFavoriteAds;
        }

        public async Task<PaginatedList<MyArchivedAdsViewModel>> GetArchivedAdsViewModelsAsync(int pageNumber,
            int pageSize)
        {
            var archivedAdViewModels = GetMyArchivedAdsViewModels();

            var paginatedArchivedAdViewModels =
                await PaginatedList<MyArchivedAdsViewModel>.CreateAsync(archivedAdViewModels, pageNumber, pageSize);

            return paginatedArchivedAdViewModels;
        }

        public async Task<AdsBySearchViewModel> GetAdsBySearchViewModelAsync(string searchText, int pageNumber,
            int pageSize)
        {
            var adViewModels = context
                .Ads
                .Where(x => x.Title.Contains(searchText) && x.IsApproved)
                .To<AdViewModel>();

            var paginatedAdsBySearchViewModels =
                await PaginatedList<AdViewModel>.CreateAsync(adViewModels, pageNumber, pageSize);

            var adsBySearchViewModel = new AdsBySearchViewModel
            {
                Search = searchText,
                Ads = paginatedAdsBySearchViewModels
            };

            return adsBySearchViewModel;
        }

        public async Task<AdsByUserBindingModel> GetAdsByUserBindingModelAsync(string userId, int pageNumber,
            int pageSize)
        {
            var user = await usersService.GetUserByIdAsync(userId);

            var adByUserViewModels = context
                .Ads
                .Where(x => x.SellerId == userId && !x.IsDeleted && x.ActiveTo >= DateTime.UtcNow && x.IsApproved)
                .To<AdViewModel>();

            var adsByUserPaginatedList =
                await PaginatedList<AdViewModel>.CreateAsync(adByUserViewModels, pageNumber, pageSize);

            var adsByUserBindingModel = new AdsByUserBindingModel
            {
                UserId = user.Id,
                Username = user.UserName,
                AdViewModels = adsByUserPaginatedList
            };

            return adsByUserBindingModel;
        }

        public async Task EditAd(EditAdInputModel inputModel)
        {
            var adFromDb = await context.Ads.FirstOrDefaultAsync(x => x.Id == inputModel.AdId);

            if (adFromDb == null)
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var imageUrls = inputModel.EditAdDetailsInputModel.Images
                .Select(async x =>
                    await cloudinaryService.UploadPictureAsync(x, inputModel.EditAdDetailsInputModel.Title))
                .Select(x => x.Result)
                .ToList();

            adFromDb.Title = inputModel.EditAdDetailsInputModel.Title;
            adFromDb.Description = inputModel.EditAdDetailsInputModel.Description;
            adFromDb.Price = inputModel.EditAdDetailsInputModel.Price;
            adFromDb.AvailabilityCount = inputModel.EditAdDetailsInputModel.Availability;
            adFromDb.ConditionId = inputModel.EditAdDetailsInputModel.ConditionId;

            foreach (var image in imageUrls)
            {
                adFromDb.Images.Add(new Image {ImageUrl = image});
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

        public async Task<PaginatedList<AdForApprovalViewModel>> GetAdsForApprovalViewModelsAsync(int pageNumber, int pageSize)
        {
            var adForApprovalViewModels = context
                .Ads
                .Where(x => !x.IsApproved && !x.IsDeclined)
                .OrderByDescending(x => x.CreatedOn)
                .To<AdForApprovalViewModel>();

            var paginatedListViewModels =
                await PaginatedList<AdForApprovalViewModel>.CreateAsync(adForApprovalViewModels, pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<bool> ApproveAdAsync(int adId)
        {
            var adFromDb = await context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == adId);

            if (adFromDb == null)
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            if (adFromDb.IsApproved)
            {
                throw new InvalidOperationException(AlreadyApprovedAdErrorMessage);
            }

            adFromDb.IsApproved = true;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<RejectAdBindingModel> GetRejectAdBindingModelAsync(int adId)
        {
            var adFromDb = await context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == adId && !x.IsDeleted);

            if (adFromDb == null)
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var rejectAdViewModel = mapper.Map<RejectAdViewModel>(adFromDb);

            var rejectAdBindingModel = new RejectAdBindingModel
            {
                ViewModel = rejectAdViewModel
            };

            return rejectAdBindingModel;
        }

        public async Task CreateAdRejectionAsync(int adId, string comment)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            if (comment.IsNullOrEmpty())
            {
                throw new ArgumentException(CommentNullOrEmptyErrorMessage);
            }

            var adFromDb = await context.Ads.FirstOrDefaultAsync(x => x.Id == adId);
            adFromDb.IsDeclined = true;
            context.Update(adFromDb);

            var adRejection = new AdRejection
            {
                AdId = adId,
                Comment = comment
            };

            await context.AdRejections.AddAsync(adRejection);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<WaitingForApprovalByUserViewModel>>
            GetWaitingForApprovalByCurrentUserViewModels(int pageNumber, int pageSize)
        {
            var currentUserId = usersService.GetCurrentUserId();

            var waitingForApprovalViewModels = context
                .Ads
                .Where(x => x.SellerId == currentUserId && !x.IsApproved && !x.IsDeclined)
                .To<WaitingForApprovalByUserViewModel>();

            var paginatedListViewModels =
                await PaginatedList<WaitingForApprovalByUserViewModel>.CreateAsync(waitingForApprovalViewModels,
                    pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<PaginatedList<RejectedByUserAdViewModel>> GetRejectedAdByUserViewModelsAsync(int pageNumber,
            int pageSize)
        {
            var currentUserId = usersService.GetCurrentUserId();

            var rejectedAdByUserViewModels = context
                .AdRejections
                .Where(x => x.Ad.SellerId == currentUserId && x.Ad.IsDeclined && !x.Ad.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .To<RejectedByUserAdViewModel>();

            var paginatedListViewModels =
                await PaginatedList<RejectedByUserAdViewModel>.CreateAsync(rejectedAdByUserViewModels, pageNumber,
                    pageSize);

            return paginatedListViewModels;
        }

        public async Task<bool> SubmitRejectedAdAsync(int rejectionId)
        {
            if (!await context.AdRejections.AnyAsync(x => x.Id == rejectionId))
            {
                throw new ArgumentException(InvalidRejectionIdMessage);
            }

            var rejectionFromDb = await context
                .AdRejections
                .FirstOrDefaultAsync(x => x.Id == rejectionId);

            if (!await context.Ads.AnyAsync(x => x.Id == rejectionFromDb.AdId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var adFromDb = await context
                .Ads
                .FirstOrDefaultAsync(x => x.Id == rejectionFromDb.AdId);

            adFromDb.IsDeclined = false;

            rejectionFromDb.IsDeleted = true;
            context.Update(rejectionFromDb);
            context.Update(adFromDb);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedList<RejectedAdAllViewModel>> GetRejectedAdAllViewModelsAsync(int pageNumber,
            int pageSize)
        {
            var rejectedAdAllViewModels = context
                .Ads
                .Where(x => x.IsDeclined)
                .OrderByDescending(x => x.CreatedOn)
                .To<RejectedAdAllViewModel>();

            var paginatedListViewModels =
                await PaginatedList<RejectedAdAllViewModel>.CreateAsync(rejectedAdAllViewModels, pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<PaginatedList<ActiveAdAllViewModel>> GetAllActiveAdViewModelsAsync(int pageNumber,
            int pageSize)
        {
            var activeAdAllViewModels = context
                .Ads
                .Where(x => !x.IsDeleted && x.IsApproved)
                .OrderByDescending(x => x.CreatedOn)
                .To<ActiveAdAllViewModel>();

            var paginatedListViewModels =
                await PaginatedList<ActiveAdAllViewModel>.CreateAsync(activeAdAllViewModels, pageNumber, pageSize);

            return paginatedListViewModels;
        }

        public async Task<List<int>> GetTheCountForTheCreatedAdsForTheLastTenDaysAsync()
        {
            var adsCount = new List<int>();

            for (DateTime i = DateTime.UtcNow.AddDays(-GlobalConstants.CreatedAdsStatisticDaysCount + 1);
                i <= DateTime.UtcNow;
                i = i.AddDays(1))
            {
                var currentDaysAdsCount = await context.Ads
                    .CountAsync(x => x.CreatedOn.DayOfYear == i.DayOfYear);

                adsCount.Add(currentDaysAdsCount);
            }

            return adsCount;
        }

        public async Task<int> GetAllActiveAdsCountAsync()
        {
            var allAdsCount = await context.Ads.CountAsync(x => !x.IsDeleted && x.IsApproved);

            return allAdsCount;
        }

        private IQueryable<MyArchivedAdsViewModel> GetMyArchivedAdsViewModels()
        {
            string currentUserId = usersService.GetCurrentUserId();

            var archivedAds = GetArchivedAdsByUserId(currentUserId);

            var archivedAdsForCurrentUserViewModels = archivedAds
                .To<MyArchivedAdsViewModel>();

            return archivedAdsForCurrentUserViewModels;
        }

        private Task<int> GetObservedAdsByAdIdAsync(int adId)
        {
            var observed = context
                .SellMeUserFavoriteProducts
                .CountAsync(x => x.AdId == adId);

            return observed;
        }

        private async Task CreateViewForAdAsync(Ad ad)
        {
            var adView = new AdView
            {
                AdId = ad.Id
            };
            await context.AdViews.AddAsync(adView);
            await context.SaveChangesAsync();
        }

        private IQueryable<Ad> GetAdsBySubcategory(int subcategoryId)
        {
            var adsBySubcategory = context
                .Ads
                .Where(x => x.SubCategoryId == subcategoryId && x.IsApproved && !x.IsDeleted)
                .OrderByDescending(x => x.ActiveTo);

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

        private async Task<EditAdDetailsViewModel> GetEditAdViewModelByIdAsync(int adId)
        {
            var adFromDb = await GetAdByIdAsync(adId);

            var editAdDetailsViewModel = mapper.Map<EditAdDetailsViewModel>(adFromDb);

            return editAdDetailsViewModel;
        }

        private EditAdViewModel GetEditAdViewModel(EditAdDetailsViewModel editAdDetailsViewModel,
            EditAdAddressViewModel editAdAddressViewModel)
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
            var addressFromDb = await addressesService.GetAddressByIdAsync(adId);

            var editAdAddressViewModel = mapper.Map<EditAdAddressViewModel>(addressFromDb);

            return editAdAddressViewModel;
        }

        private IQueryable<Ad> GetArchivedAdsByUserId(string userId)
        {
            var adsByUser = context
                .Ads
                .Where(x => x.SellerId == userId && x.IsDeleted);

            return adsByUser;
        }

        private IQueryable<Ad> GetActiveAdsByUserId(string userId)
        {
            var adsByUser = context
                .Ads
                .Where(x => x.SellerId == userId && !x.IsDeleted && x.IsApproved);

            return adsByUser;
        }

        private IQueryable<FavoriteAdViewModel> GetFavoriteAdViewModelsByUser(SellMeUser user)
        {
            var favoriteAdsViewModels = context
                .SellMeUserFavoriteProducts
                .Where(x => x.SellMeUserId == user.Id)
                .OrderByDescending(x => x.CreatedOn)
                .To<FavoriteAdViewModel>();

            return favoriteAdsViewModels;
        }

        private AdsByCategoryViewModel CreateAdsByCategoryViewModel(PaginatedList<AdViewModel> paginatedAdViewModels,
            IEnumerable<CategoryViewModel> allCategoriesViewModel, string categoryName,
            ICollection<AdsByCategorySubcategoryViewModel> subcategoryViewModels, int categoryId)
        {
            var adsByCategoryViewModel = new AdsByCategoryViewModel
            {
                CategoryId = categoryId,
                CategoryName = categoryName,
                AdsViewModels = paginatedAdViewModels,
                AdsByCategorySubcategoryViewModels = subcategoryViewModels.ToList()
            };

            return adsByCategoryViewModel;
        }

        private IQueryable<AdViewModel> GetAllAdsByCategory(int categoryId)
        {
            var adsViewModel = context
                .Ads
                .Where(x => x.CategoryId == categoryId && x.IsApproved && !x.IsDeleted)
                .OrderByDescending(x => x.ActiveTo)
                .To<AdViewModel>();

            return adsViewModel;
        }

        private IQueryable<MyActiveAdsViewModel> GetMyAdsViewModels()
        {
            string currentUserId = usersService.GetCurrentUserId();

            var adsForCurrentUser = GetActiveAdsByUserId(currentUserId);

            var adsForCurrentUserViewModels = adsForCurrentUser
                .OrderByDescending(x => x.ActiveTo)
                .To<MyActiveAdsViewModel>();

            return adsForCurrentUserViewModels;
        }
    }
}
