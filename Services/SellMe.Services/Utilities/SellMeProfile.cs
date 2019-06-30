using SellMe.Data;

namespace SellMe.Services.Utilities
{
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Addresses;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using AutoMapper;
    using SellMe.Data.Models;
    using System.Linq;

    public class SellMeProfile : Profile
    {
        public SellMeProfile()
        {
            CreateMap<Ad, AdDetailsViewModel>()
                .ForMember(x => x.Images, cfg => cfg.MapFrom(x => x.Images.Select(img => img.ImageUrl)));

            CreateMap<Address, AddressViewModel>();
        }
    }
}
