﻿namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class AdViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, AdViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Any() ? x.Images.FirstOrDefault().ImageUrl : "/img/no-image.png"))
                .ForMember(x => x.Country, cfg => cfg.MapFrom(x => x.Address.Country))
                .ForMember(x => x.City, cfg => cfg.MapFrom(x => x.Address.City));

        }
    }
}
