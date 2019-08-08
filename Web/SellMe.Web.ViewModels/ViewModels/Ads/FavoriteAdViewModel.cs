namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Linq;
    using AutoMapper;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class FavoriteAdViewModel : BaseViewModel, IMapFrom<SellMeUserFavoriteProduct>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SellMeUserFavoriteProduct, FavoriteAdViewModel>()
                .ForMember(x => x.ImageUrl,
                    cfg => cfg.MapFrom(x => x.Ad.Images.Select(y => y.ImageUrl).FirstOrDefault()))
                .ForMember(x => x.Country, cfg => cfg.MapFrom(x => x.Ad.Address.Country))
                .ForMember(x => x.City, cfg => cfg.MapFrom(x => x.Ad.Address.City))
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Ad.Id))
                .ForMember(x => x.Title, cfg => cfg.MapFrom(x => x.Ad.Title))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Ad.Price))
                .ForMember(x => x.CategoryName, cfg => cfg.MapFrom(x => x.Ad.Category.Name))
                .ForMember(x => x.SubcategoryName, cfg => cfg.MapFrom(x => x.Ad.SubCategory.Name))
                .ForMember(x => x.CreatedOn, cfg => cfg.MapFrom(x => x.Ad.CreatedOn))
                .ForMember(x => x.Description, cfg => cfg.MapFrom(x => x.Ad.Description));
        }
    }
}
