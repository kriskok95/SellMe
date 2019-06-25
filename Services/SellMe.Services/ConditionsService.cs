namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using SellMe.Data.Models;
    using System.Linq;
    using SellMe.Data;

    public class ConditionsService : IConditionsService
    {
        private readonly SellMeDbContext context;

        public ConditionsService(SellMeDbContext context)
        {
            this.context = context;
        }

        public Condition GetConditionByName(string conditionName)
        {
            Condition condition = this.context
                .Conditions
                .FirstOrDefault(x => x.Name == conditionName);

            return condition;
        }
    }
}
