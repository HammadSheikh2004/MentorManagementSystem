using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mentor_Management_System.Areas.Identity.Data;
using Mentor_Management_System.Models;
using Stripe;

namespace Mentor_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("MyConn") ?? throw new InvalidOperationException("Connection string 'MyConn' not found.");

                                    builder.Services.AddDbContext<Mentor_Management_SystemContext>(options =>
                options.UseSqlServer(connectionString));

                                                                      builder.Services.AddDefaultIdentity<Mentor_Management_SystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Mentor_Management_SystemContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Mentor}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}