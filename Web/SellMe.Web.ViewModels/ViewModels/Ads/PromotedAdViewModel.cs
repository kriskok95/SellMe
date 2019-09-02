namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class PromotedAdViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string MainPictureUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, PromotedAdViewModel>()
                .ForMember(x => x.MainPictureUrl, cfg => cfg.MapFrom(x => x.Images.Any() ? x.Images.FirstOrDefault().ImageUrl : "/img/no-image.png"));
        }
    }
}
