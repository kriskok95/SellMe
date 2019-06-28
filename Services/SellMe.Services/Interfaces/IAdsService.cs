using SellMe.Web.ViewModels.InputModels.Ads;

namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.ViewModels.Products;

    public interface IAdsService
    {
        ICollection<string> GetCategoryNames();

        ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName);

        ICollection<string> GetConditionsFromDb();

        void CreateProduct(CreateAdInputModel inputModel);

        ICollection<AdsAllViewModel> GetAllProductsViewModels();

    }
}
