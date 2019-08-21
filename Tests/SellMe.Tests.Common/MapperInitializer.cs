namespace SellMe.Tests.Common
{
    using System.Reflection;

    using SellMe.Services;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public static class MapperInitializer 
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(AdsService).GetTypeInfo().Assembly,
                typeof(ActiveAdAllViewModel).GetTypeInfo().Assembly);
        }
    }
}
