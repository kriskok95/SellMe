namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Conditions;

    public interface IConditionsService
    {
        ICollection<CreateAdConditionViewModel> GetConditionViewModels();
    }
}
