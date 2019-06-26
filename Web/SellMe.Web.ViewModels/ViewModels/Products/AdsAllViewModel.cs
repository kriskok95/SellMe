namespace SellMe.Web.ViewModels.ViewModels.Products
{
    using System;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System.Linq;
    using AutoMapper;


    public class AdsAllViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, AdsAllViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Select(y => y.ImageUrl).FirstOrDefault()));

        }
    }
}
