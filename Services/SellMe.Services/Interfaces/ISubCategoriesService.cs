namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.ViewModels.ViewModels.Subcategories;

    public interface ISubCategoriesService
    {
        Task<ICollection<CreateAdSubcategoryViewModel>> GetSubcategoriesByCategoryIdAsync(int categoryId);

        Task<ICollection<AdsByCategorySubcategoryViewModel>> GetAdsByCategorySubcategoryViewModelsAsync(int categoryId);
    }
}
