namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;

    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Conditions;

    public interface IConditionsService
    {
        ICollection<ConditionViewModel> GetConditionViewModels();
    }
}
