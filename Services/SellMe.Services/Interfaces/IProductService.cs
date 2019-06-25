using System.Collections.Generic;
using SellMe.Data.Models;
using SellMe.Web.ViewModels.InputModels.Products;

namespace SellMe.Services.Interfaces
{
    public interface IProductService
    {
        ICollection<string> GetCategoryNames();

        ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName);

        ICollection<string> GetConditionsFromDb();

        void CreateProduct(CreateProductInputModel inputModel);

    }
}
