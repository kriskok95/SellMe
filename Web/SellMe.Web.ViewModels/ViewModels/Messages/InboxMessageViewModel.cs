namespace SellMe.Web.ViewModels.ViewModels.Messages
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System;
    using AutoMapper;

    public class InboxMessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Sender { get; set; }

        public string AdTitle { get; set; }

        public DateTime SentOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, InboxMessageViewModel>()
                .ForMember(x => x.Sender, cfg => cfg.MapFrom(x => x.Sender.UserName))
                .ForMember(x => x.AdTitle, cfg => cfg.MapFrom(x => x.Ad.Title))
                .ForMember(x => x.SentOn, cfg => cfg.MapFrom(x => x.CreatedOn));
        }
    }
}
