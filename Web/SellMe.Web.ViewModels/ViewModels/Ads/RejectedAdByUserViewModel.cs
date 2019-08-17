namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Linq;
    using AutoMapper;

    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class RejectedByUserAdViewModel : IMapFrom<AdRejection>, IHaveCustomMappings
    {
        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public string ImageUrl { get; set; }

        public string Comment { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AdRejection, RejectedByUserAdViewModel>()
                .ForMember(x => x.AdId, cfg => cfg.MapFrom(x => x.AdId))
                .ForMember(x => x.AdTitle, cfg => cfg.MapFrom(x => x.Ad.Title))
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Ad.Images.FirstOrDefault().ImageUrl));
        }
    }
}
