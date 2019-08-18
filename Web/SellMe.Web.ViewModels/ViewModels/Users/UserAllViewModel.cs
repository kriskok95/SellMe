using AutoMapper;

namespace SellMe.Web.ViewModels.ViewModels.Users
{
    using System;
    using Microsoft.AspNetCore.Identity;

    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class UserAllViewModel : IMapFrom<SellMeUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SellMeUser, UserAllViewModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.UserName))
                .ForMember(x => x.EmailConfirmed, cfg => cfg.MapFrom(x => x.EmailConfirmed))
                .ForMember(x => x.CreatedOn, cfg => cfg.MapFrom(x => x.CreatedOn));
        }
    }
}
