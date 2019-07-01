namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public interface ISubCategoriesService
    {
        ICollection<CreateAdSubcategoryViewModel> GetSubcategoriesByCategoryId(int categoryId);
    }
}
