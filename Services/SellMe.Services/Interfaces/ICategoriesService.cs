using SellMe.Data.Models;
using SellMe.Web.ViewModels.ViewModels.Categories;

namespace SellMe.Services.Interfaces
{
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using System.Collections.Generic;

    public interface ICategoriesService
    {
        int GetCategoryIdByName(string categoryName);

        ICollection<CreateAdCategoryViewModel> GetCategoryViewModels();

        Category GetCategoryById(int categoryId);
    }
}
