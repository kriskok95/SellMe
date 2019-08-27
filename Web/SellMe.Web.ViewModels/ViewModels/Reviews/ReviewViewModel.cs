namespace SellMe.Web.ViewModels.ViewModels.Reviews
{
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public int Rating { get; set; }

        public string Sender { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>()
                .ForMember(x => x.Sender, cfg => cfg.MapFrom(x => x.Creator.UserName))
                .ForMember(x => x.Content, cfg => cfg.MapFrom(x => x.Comment));
        }
    }
}
