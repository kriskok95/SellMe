namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.ViewModels.Categories;

    public interface ICategoriesService
    {
        ICollection<CreateAdCategoryViewModel> GetCategoryViewModels();

        Task<Category> GetCategoryByIdAsync(int categoryId);

        Task<ICollection<CategoryViewModel>> GetAllCategoryViewModelAsync();

        Task<string> GetCategoryNameByIdAsync(int categoryId);
    }
}
