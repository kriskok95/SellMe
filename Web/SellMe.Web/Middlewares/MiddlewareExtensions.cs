namespace SellMe.Web.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedDataMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedMiddleware>();
        }
    }
}
