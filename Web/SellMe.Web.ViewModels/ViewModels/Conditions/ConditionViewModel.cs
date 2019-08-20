namespace SellMe.Web.ViewModels.ViewModels.Conditions
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class ConditionViewModel : BaseViewModel, IMapFrom<Condition>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
