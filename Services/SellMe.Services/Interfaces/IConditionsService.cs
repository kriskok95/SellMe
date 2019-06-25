namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;

    public interface IConditionsService
    {
        Condition GetConditionByName(string conditionName);
    }
}
