namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System.Linq;
    using AutoMapper;
    using System;


    public class MyActiveAdsViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public bool IsPromoted { get; set; }

        public int Updates { get; set; }

        public string ImageUrl { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, MyActiveAdsViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.FirstOrDefault().ImageUrl))
                .ForMember(x => x.IsPromoted, cfg => cfg.MapFrom(x => x.PromotionOrders.Any(y => y.IsActive)));
        }
    }
}
