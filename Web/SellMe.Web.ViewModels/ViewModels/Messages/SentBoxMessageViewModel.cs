namespace SellMe.Web.ViewModels.ViewModels.Messages
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class SentBoxMessageViewModel : BaseViewModel, IMapFrom<Message>, IHaveCustomMappings
    {
        public string RecipientId { get; set; }

        public string Sender { get; set; }

        public string SenderId { get; set; }

        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public DateTime SentOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, SentBoxMessageViewModel>()
                .ForMember(x => x.SentOn, cfg => cfg.MapFrom(x => x.CreatedOn.ToLocalTime()))
                .ForMember(x => x.SenderId, cfg => cfg.MapFrom(x => x.SenderId))
                .ForMember(x => x.RecipientId, cfg => cfg.MapFrom(x => x.RecipientId));
        }
    }
}
