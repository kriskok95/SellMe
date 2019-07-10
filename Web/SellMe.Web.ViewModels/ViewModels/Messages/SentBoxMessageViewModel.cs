﻿namespace SellMe.Web.ViewModels.ViewModels.Messages
{
    using System;
    using AutoMapper;
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class SentBoxMessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public int SenderId { get; set; }

        public string Sender { get; set; }

        public string AdTitle { get; set; }

        public DateTime SentOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, SentBoxMessageViewModel>()
                .ForMember(x => x.SentOn, cfg => cfg.MapFrom(x => x.CreatedOn));
        }
    }
}
