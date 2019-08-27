namespace SellMe.Web.ViewModels.ViewModels.Subcategories
{
    using Data.Models;
    using Services.Mapping;

    public class CreateAdSubcategoryViewModel : BaseViewModel, IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
