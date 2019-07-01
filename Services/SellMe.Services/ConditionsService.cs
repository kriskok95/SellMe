namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using SellMe.Data.Models;
    using System.Linq;
    using SellMe.Data;
    using System.Collections.Generic;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.ViewModels.Conditions;

    public class ConditionsService : IConditionsService
    {
        private readonly SellMeDbContext context;

        public ConditionsService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<CreateAdConditionViewModel> GetConditionViewModels()
        {
            var conditionViewModels = this.context
                .Conditions
                .To<CreateAdConditionViewModel>()
                .ToList();

            return conditionViewModels;
        }
    }
}
