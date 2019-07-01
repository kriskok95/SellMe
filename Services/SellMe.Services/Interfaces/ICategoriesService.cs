namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.ViewModels.Categories;

    public interface ICategoriesService
    {
        ICollection<CreateAdCategoryViewModel> GetCategoryViewModels();

        Category GetCategoryById(int categoryId);
    }
}
