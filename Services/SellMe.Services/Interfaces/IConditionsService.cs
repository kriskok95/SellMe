namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using Web.ViewModels.ViewModels.Conditions;

    public interface IConditionsService
    {
        ICollection<ConditionViewModel> GetConditionViewModels();
    }
}
