namespace SellMe.Web.Middlewares
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Data.Models.Enums;

    public class SeedMiddleware
    {
        private readonly RequestDelegate next;

        public SeedMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<SellMeUser> userManager,
            RoleManager<IdentityRole> roleManager, SellMeDbContext db)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager).GetAwaiter().GetResult();

            await this.next(context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Role.Administrator.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Administrator.ToString()));
            }
        }

        private static async Task SeedUserInRoles(UserManager<SellMeUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new SellMeUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                }
            }
        }
    }
}
