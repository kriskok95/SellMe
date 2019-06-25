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

        void CreateProduct(CreateProductInputModel inputModel);

    }
}
