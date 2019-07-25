﻿namespace SellMe.Web.ViewModels.ViewModels.Subcategories
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class AdsByCategorySubcategoryViewModel : BaseViewModel, IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}