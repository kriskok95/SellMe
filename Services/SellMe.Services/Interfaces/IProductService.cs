using SellMe.Web.ViewModels.ViewModels.Products;

namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.InputModels.Products;

    public interface IProductsService
    {
        ICollection<string> GetCategoryNames();

        ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName);

        ICollection<string> GetConditionsFromDb();

        void CreateProduct(CreateAdInputModel inputModel);

        ICollection<AdsAllViewModel> GetAllProductsViewModels();

    }
}
