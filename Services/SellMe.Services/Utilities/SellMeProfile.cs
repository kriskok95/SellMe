namespace SellMe.Services.Utilities
{
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Addresses;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using AutoMapper;
    using SellMe.Data.Models;
    using System.Linq;

    public class SellMeProfile : Profile
    {
        public SellMeProfile()
        {
            CreateMap<CreateAdInputModel, Ad>()
                .ForMember(x => x.CategoryId, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.CategoryId))
                .ForMember(x => x.SubCategoryId, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.SubCategoryId))
                .ForMember(x => x.AvailabilityCount, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Availability))
                .ForMember(x => x.Description, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Description))
                .ForMember(x => x.ConditionId, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.ConditionId))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Price))
                .ForMember(x => x.Title, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Title))
                .ForMember(x => x.Address, cfg => cfg.MapFrom(x => x.CreateAdAddressInputModel));

            CreateMap<CreateAdAddressInputModel, Address>();


            CreateMap<Ad, AdDetailsViewModel>()
                .ForMember(x => x.Images, cfg => cfg.MapFrom(x => x.Images.Select(img => img.ImageUrl)))
                .ForMember(x => x.Phone, cfg => cfg.MapFrom(x => x.Address.PhoneNumber));

            CreateMap<Address, AddressViewModel>();

            CreateMap<Ad, SendMessageViewModel>()
                .ForMember(x => x.AdId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.SellerPhone, cfg => cfg.MapFrom(x => x.Address.PhoneNumber))
                .ForMember(x => x.AdTitle, cfg => cfg.MapFrom(x => x.Title))
                .ForMember(x => x.AdPrice, cfg => cfg.MapFrom(x => x.Price))
                .ForMember(x => x.RecipientId, cfg => cfg.MapFrom(x => x.SellerId));

            //Map message
            CreateMap<SendMessageInputModel, Message>();

            CreateMap<Ad, EditAdDetailsViewModel>()
                .ForMember(x => x.Images, cfg => cfg.MapFrom(x => x.Images.Select(y => y.ImageUrl)))
                .ForMember(x => x.Availability, cfg => cfg.MapFrom(x => x.AvailabilityCount));

            CreateMap<Address, EditAdAddressViewModel>();
        }
    }
}
