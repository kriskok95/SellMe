﻿using System;
using System.Security.Policy;

namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Linq;
    using AutoMapper;

    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class RejectedAdAllViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, RejectedAdAllViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Select(y => y.ImageUrl).FirstOrDefault()));
        }
    }
}