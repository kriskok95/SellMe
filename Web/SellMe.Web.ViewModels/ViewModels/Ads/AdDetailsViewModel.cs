namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Addresses;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System.Linq;
    using AutoMapper;

    public class AdDetailsViewModel : BaseViewModel, IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string SellerId { get; set; }

        public string Seller { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Phone { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        //TODO: Change property name to type
        public string ConditionName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Views { get; set; }

        public int Observed { get; set; }

        public AddressViewModel AddressViewModel { get; set; }

        public ICollection<string> Images { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, AdDetailsViewModel>()
                .ForMember(x => x.Images, cfg => cfg.MapFrom(x => x.Images.Select(img => img.ImageUrl)))
                .ForMember(x => x.Phone, cfg => cfg.MapFrom(x => x.Address.PhoneNumber))
                .ForMember(x => x.Views, cfg => cfg.MapFrom(x => x.AdViews.Count))
                .ForMember(x => x.Seller, cfg => cfg.MapFrom(x => x.Seller.UserName));
        }
    }
}
