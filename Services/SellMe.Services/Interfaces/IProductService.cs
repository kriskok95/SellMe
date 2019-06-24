using System.Collections.Generic;
using SellMe.Data.Models;

namespace SellMe.Services.Interfaces
{
    public interface IProductService
    {
        ICollection<string> GetCategoryNames();

        ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName);
        ICollection<string> GetConditionsFromDb();
    }
}
