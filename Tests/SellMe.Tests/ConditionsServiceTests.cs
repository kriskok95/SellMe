﻿namespace SellMe.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Data.Models;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels.ViewModels.Conditions;
    using Xunit;

    public class ConditionsServiceTests
    {
        private IConditionsService conditionsService;

        [Fact]
        public async Task GetConditionViewModels_WithValidData_ShouldReturnProperCollection()
        {
            //Arrange
            var expected = new List<ConditionViewModel>
            {
                new ConditionViewModel
                {
                    Id = 1,
                    Name = "Brand New"
                },
                new ConditionViewModel
                {
                    Id = 2,
                    Name = "Used"
                }
            };

            var context = InitializeContext.CreateContextForInMemory();
            conditionsService = InitializeConditionsService(context);

            var conditions = CreateTestingConditions();
            await context.Conditions.AddRangeAsync(conditions);
            await context.SaveChangesAsync();

            //Act
            var actual = conditionsService.GetConditionViewModels();

            //Assert
            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Name, elem1.Name);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Name, elem2.Name);
                });

            Assert.Equal(expected.Count, actual.Count);
        }

        [Fact]
        public void GetConditionViewModels_WithoutData_ShouldReturnAnEmptyCollection()
        {
            //Arrange
            var expected = 0;

            var context = InitializeContext.CreateContextForInMemory();
            conditionsService = InitializeConditionsService(context);

            var actual = conditionsService.GetConditionViewModels();

            //Act and assert
            Assert.Equal(expected, actual.Count);
        }

        private List<Condition> CreateTestingConditions()
        {
            var conditions = new List<Condition>
            {
                new Condition
                {
                    Id = 1,
                    Name = "Brand New"
                },
                new Condition
                {
                    Id = 2,
                    Name = "Used"
                }
            };

            return conditions;
        }

        private static ConditionsService InitializeConditionsService(SellMeDbContext context)
        {
            MapperInitializer.InitializeMapper();

            ConditionsService service = new ConditionsService(context);

            return service;
        }
    }
}
