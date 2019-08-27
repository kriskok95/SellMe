namespace SellMe.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SubcategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.SubCategories.AnyAsync())
            {
                return;
            }

            //Subcategories for Vehicles
            await dbContext.SubCategories.AddAsync(new SubCategory
                {Name = "Wheels/Tyres", CategoryId = 1, CreatedOn = DateTime.UtcNow});
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Auto Parts", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Accessories", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Auto Services", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Trucks", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Trailers/Wagons", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Automobiles", CategoryId = 1, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "motorcycles", CategoryId = 1, CreatedOn = DateTime.UtcNow });

            //Subcategories for Sport/books/hoby

            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Sport Goods", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Tourism/Camping", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Books", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Music", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Movies", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Tickets/Events", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Games", CategoryId = 2, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Musical Instruments", CategoryId = 2, CreatedOn = DateTime.UtcNow });

            //Subcategories for Animals
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Dogs", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Cats", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Fishes", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Birds", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Movies", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Farm Animals", CategoryId = 3, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Other Pets", CategoryId = 3, CreatedOn = DateTime.UtcNow });

            //Subcategories for Electronics
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Computers", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Computer Accessories/Reparing Parts", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Tablets", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Phones", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Phones Accessories/Reparing Parts", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "TVs", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Audio", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Home appliances", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Air Conditioners", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Photo/Video", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Navigation", CategoryId = 4, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Others", CategoryId = 4, CreatedOn = DateTime.UtcNow });


            //Subcategories for services
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Garden Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Cosmetic Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Baby Sitters", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Animal Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Courses", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Trainings/Dancing", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Sewing Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Medical Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Auto Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Building Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Transporting Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Business Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Wedding Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Security Services", CategoryId = 5, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Others", CategoryId = 5, CreatedOn = DateTime.UtcNow });

            //Subcategories for Real Estate
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Selling", CategoryId = 6, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Rents", CategoryId = 6, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Roommates", CategoryId = 6, CreatedOn = DateTime.UtcNow });


            //Subcategories for home and garden
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Furniture", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Workman", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Art", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Household Goods", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Curtains/Carpets", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Lighting", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Garden", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Cleaning Goods", CategoryId = 7, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Others", CategoryId = 7, CreatedOn = DateTime.UtcNow });

            //Subcategories for work
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Hotels/Tourism", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Production/Building", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Trade", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Health/Beauty", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Cleaning", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Transport/Logistic", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Administration", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Security", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Information Technology", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Economics/Law", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Advertising/Marketing", CategoryId = 8, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Other", CategoryId = 8, CreatedOn = DateTime.UtcNow });

            //Subcategories for tourism

            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Sea", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Mountain", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Guest House", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "SPA", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Economics/Law", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Abroad", CategoryId = 9, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Other", CategoryId = 9, CreatedOn = DateTime.UtcNow });

            //Subcategories for baby
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Clothes", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Shoes", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Guest House", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Baby Strollers", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Toys", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Furniture", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Accessories", CategoryId = 10, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Others", CategoryId = 10, CreatedOn = DateTime.UtcNow });

            //Subcategories for fashion
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Clothes", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Shoes", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Accessories", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "jewelery", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Watches", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Perfumery/Cosmetics", CategoryId = 11, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                {Name = "Others", CategoryId = 11, CreatedOn = DateTime.UtcNow});

            //Subcategories for machines/tools
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Industrial Equipment", CategoryId = 12, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Machines", CategoryId = 12, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Tools", CategoryId = 12, CreatedOn = DateTime.UtcNow });
            await dbContext.SubCategories.AddAsync(new SubCategory
                { Name = "Others", CategoryId = 12, CreatedOn = DateTime.UtcNow });
        }
    }
}
