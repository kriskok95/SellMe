namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class AdForApprovalViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, AdForApprovalViewModel>()
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Images.Any() ? x.Images.FirstOrDefault().ImageUrl : "/img/no-image.png"));
        }
    }
}
