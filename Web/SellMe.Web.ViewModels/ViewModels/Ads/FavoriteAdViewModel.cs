namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Linq;
    using AutoMapper;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class FavoriteAdViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, FavoriteAdViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Select(y => y.ImageUrl).FirstOrDefault()));

        }
    }
}
