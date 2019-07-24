namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;
    using System.Threading.Tasks;

    public interface ISubCategoriesService
    {
        Task<ICollection<CreateAdSubcategoryViewModel>> GetSubcategoriesByCategoryIdAsync(int categoryId);

        Task<ICollection<AdsByCategorySubcategoryViewModel>> GetAdsByCategorySubcategoryViewModelsAsync(int categoryId);
    }
}
