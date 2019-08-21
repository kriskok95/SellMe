namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.ViewModels.Categories;

    public interface ICategoriesService
    {
        Task<ICollection<CreateAdCategoryViewModel>> GetCategoryViewModelsAsync();

        Task<Category> GetCategoryByIdAsync(int categoryId);

        Task<ICollection<CategoryViewModel>> GetAllCategoryViewModelsAsync();

        Task<string> GetCategoryNameByIdAsync(int categoryId);
    }
}
