using System.Collections.Generic;

namespace SellMe.Services.Interfaces
{
    public interface IProductService
    {
        ICollection<string> GetCategoryNames();
    }
}
