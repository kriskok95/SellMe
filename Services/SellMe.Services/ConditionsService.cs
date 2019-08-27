namespace SellMe.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Interfaces;
    using Mapping;
    using Web.ViewModels.ViewModels.Conditions;

    public class ConditionsService : IConditionsService
    {
        private readonly SellMeDbContext context;

        public ConditionsService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<ConditionViewModel> GetConditionViewModels()
        {
            var conditionViewModels = context
                .Conditions
                .To<ConditionViewModel>()
                .ToList();

            return conditionViewModels;
        }
    }
}
