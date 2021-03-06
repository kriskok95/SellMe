﻿namespace SellMe.Web
{
    using System.Reflection;
    using AutoMapper;
    using CloudinaryDotNet;
    using Data;
    using Data.Models;
    using Data.Seeding;
    using Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Interfaces;
    using Services.Mapping;
    using Services.Messaging;
    using Services.Utilities;
    using ViewModels;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<SellMeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<SellMeUser>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;

                })
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<SellMeDbContext>();

            Account cloudinaryCredentials = new Account(
                Configuration["Cloudinary:CloudName"],
                Configuration["Cloudinary:ApiKey"],
                Configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.AddSingleton(cloudinaryUtility);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(typeof(SellMeProfile));

            //Applications services
            services.AddTransient<IAdsService, AdsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IConditionsService, ConditionsService>();
            services.AddTransient<ISubCategoriesService, SubCategoriesService>();
            services.AddTransient<IAddressesService, AddressesService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IFavoritesService, FavoritesService>();
            services.AddTransient<IPromotionsService, PromotionsService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IUpdatesService, UpdatesService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(Ad).GetTypeInfo().Assembly);

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<SellMeDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new SellMeDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSignalR(
                routes =>
                {
                    routes.MapHub<MessageHub>("/message");
                });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Administration",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
