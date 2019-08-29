namespace SellMe.Web.ViewModels.ViewModels.Messages
{
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class MessageDetailsViewModel : BaseViewModel, IMapFrom<Message>, IHaveCustomMappings
    {
        public string AdTitle { get; set; }

        public string Content { get; set; }

        public string SentOn { get; set; }

        public string Sender { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageDetailsViewModel>()
                .ForMember(x => x.Sender, cfg => cfg.MapFrom(x => x.Sender.UserName))
                .ForMember(x => x.SentOn, cfg => cfg.MapFrom(x => x.CreatedOn.AddHours(2).ToString("MM/dd/yyyy hh:mm tt")));
        }
    }
}
