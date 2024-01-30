using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Models;
using System;
using MySql.Data;

namespace PMS
{
    public class Program
    {
        
        public static async System.Threading.Tasks.Task Main(string[] args)
        //public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
                //options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            /*
            builder.Services.AddDbContext<TestContext>(options =>
            options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            */

            builder.Services.AddDefaultIdentity<Employee>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                //.AddSignInManager<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<DataLayer.DataLayer>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            /*
            using(var scope = app.Services.CreateScope())
            {
                var test = new Test();
                test.Id = Guid.NewGuid().ToString();
                test.Name = "test";
                var testManager = scope.ServiceProvider.GetRequiredService<TestContext>();
                testManager.Add(test);
                testManager.SaveChanges();
            }
            */

            // intialize main roles and admin user
/*
            using (var scope = app.Services.CreateScope())
            {
                //inialize roles
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Administrator", "Manager", "Employee", "Guest" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                //inialize admin user
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
                var user = await userManager.FindByEmailAsync("mazin@mazin.com");
                if (user != null)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }


            }
*/
            app.Run();
/*          return System.Threading.Tasks.Task.CompletedTask;*/
        }
    }
}
