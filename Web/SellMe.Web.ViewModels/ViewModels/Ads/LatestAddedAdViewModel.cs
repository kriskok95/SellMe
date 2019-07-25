namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Linq;
    using AutoMapper;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class LatestAddedAdViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string MainPictureUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, LatestAddedAdViewModel>()
                .ForMember(x => x.MainPictureUrl, cfg => cfg.MapFrom(x => x.Images.FirstOrDefault().ImageUrl));
        }
    }
}
