using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace SellMe.Web.ViewModels.ViewModels.Products
{
    using System;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class ProductsAllViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductsAllViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Select(y => y.ImageUrl).FirstOrDefault()));

        }
    }
}
