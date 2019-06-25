namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;

    public interface ISubCategoriesService
    {
        int GetSubCategoryIdByName(string subCategoryName);
    }
}
