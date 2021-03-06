﻿namespace SellMe.Web.ViewModels.ViewModels.Promotions
{
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class PromotionViewModel : IMapFrom<Promotion>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Updates { get; set; }

        public int ActiveDays { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Promotion, PromotionViewModel>()
                .ForMember(x => x.Type,
                    cfg => cfg.MapFrom(x => x.Type.First().ToString().ToUpper() + x.Type.Substring(1)));
        }
    }
}
