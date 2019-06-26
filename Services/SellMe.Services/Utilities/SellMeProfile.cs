using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SellMe.Data.Models;
using SellMe.Web.ViewModels.InputModels.Products;

namespace SellMe.Services.Utilities
{
    public class SellMeProfile : Profile
    {
        public SellMeProfile()
        {
            CreateMap<CreateProductInputModel, Ad>()
                .ForMember(x => x.AvailabilityCount, cfg => cfg.MapFrom(x => x.Availability))
                .ForMember(x => x.Category, cfg => cfg.MapFrom(x => x.Category))
                .ForMember(x => x.Condition, cfg => cfg.MapFrom(x => x.Condition))
                .ForMember(x => x.Description, cfg => cfg.MapFrom(x => x.Description))
                .ForMember(x => x.SubCategory, cfg => cfg.MapFrom(x => x.SubCategory))
                .ForMember(x => x.Title, cfg => cfg.MapFrom(x => x.Title))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Price));
        }
    }
}
