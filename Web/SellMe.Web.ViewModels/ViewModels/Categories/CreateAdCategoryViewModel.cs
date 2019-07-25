namespace SellMe.Web.ViewModels.ViewModels.Categories
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class CreateAdCategoryViewModel : BaseViewModel, IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
