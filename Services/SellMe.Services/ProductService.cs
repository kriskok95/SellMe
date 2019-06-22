namespace SellMe.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Services.Interfaces;

    public class ProductService : IProductService
    {
        private readonly SellMeDbContext context;

        public ProductService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<string> GetCategoryNames()
        {
            var categoryNames = this.context
                .Categories
                .Select(x => x.Name)
                .ToList();

            return categoryNames;
        }
    }
}
