﻿namespace SellMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using Microsoft.EntityFrameworkCore;


    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category {Name = "Vehicles", FontAwesomeIcon = "fas fa-car-alt", CreatedOn =  DateTime.UtcNow});
            await dbContext.Categories.AddAsync(new Category { Name = "Sport/Books/Hoby", FontAwesomeIcon = "fas fa-running", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Animals", FontAwesomeIcon = "fas fa-paw", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Electronics", FontAwesomeIcon = "fas fa-desktop", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Services", FontAwesomeIcon = "fas fa-hammer", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Real Estate", FontAwesomeIcon = "fas fa-home", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "House & Garden", FontAwesomeIcon = "fas fa-couch", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Work", FontAwesomeIcon = "fas fa-suitcase-rolling", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Tourism", FontAwesomeIcon = "fas fa-mountain", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Baby", FontAwesomeIcon = "fas fa-baby-carriage", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Fashion", FontAwesomeIcon = "fas fa-tshirt", CreatedOn = DateTime.UtcNow });
            await dbContext.Categories.AddAsync(new Category { Name = "Machines/Tools", FontAwesomeIcon = "fas fa-tools", CreatedOn = DateTime.UtcNow });
        }
    }
}
