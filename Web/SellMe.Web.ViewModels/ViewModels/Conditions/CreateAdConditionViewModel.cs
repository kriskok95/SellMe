namespace SellMe.Web.ViewModels.ViewModels.Conditions
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class CreateAdConditionViewModel : BaseViewModel, IMapFrom<Condition>
    {
        public int Id { get; set; }

        //TODO: Rename property name to type
        public string Name { get; set; }
    }
}
