namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System.Linq;
    using AutoMapper;


    public class MyActiveAdsViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, MyActiveAdsViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.FirstOrDefault().ImageUrl));
        }
    }
}
