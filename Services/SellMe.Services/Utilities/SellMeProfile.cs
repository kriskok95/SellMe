using SellMe.Web.ViewModels.InputModels.Ads;

namespace SellMe.Services.Utilities
{
    using AutoMapper;
    using SellMe.Data.Models;

    public class SellMeProfile : Profile
    {
        public SellMeProfile()
        {
            CreateMap<CreateAdInputModel, Ad>()
                .ForMember(x => x.AvailabilityCount, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Availability))
                .ForMember(x => x.Category, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Category))
                .ForMember(x => x.Condition, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Condition))
                .ForMember(x => x.Description, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Description))
                .ForMember(x => x.SubCategory, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.SubCategory))
                .ForMember(x => x.Title, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Title))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.CreateAdDetailInputModel.Price));
        }
    }
}
