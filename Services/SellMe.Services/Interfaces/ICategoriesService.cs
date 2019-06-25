namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;

    public interface ICategoriesService
    {
        int GetCategoryIdByName(string categoryName);
    }
}
