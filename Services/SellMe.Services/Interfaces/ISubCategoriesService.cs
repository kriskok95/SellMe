namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public interface ISubCategoriesService
    {
        int GetSubCategoryIdByName(string subCategoryName);

        ICollection<CreateAdSubcategoryViewModel> GetSubcategoriesByCategoryId(int categoryId);
    }
}
