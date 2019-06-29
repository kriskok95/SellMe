using SellMe.Web.ViewModels.InputModels.Ads;
using SellMe.Web.ViewModels.ViewModels.Ads;

namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Data.Models;

    public interface IAdsService
    {
        ICollection<string> GetCategoryNames();

        ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName);

        ICollection<string> GetConditionsFromDb();

        void CreateProduct(CreateAdInputModel inputModel);

        AdsAllViewModel GetAllAdViewModels();

    }
}
