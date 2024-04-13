using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EatEvals.Areas.Identity.Data;
using EatEvals.Data;
using EatEvals.Models;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RestaurantReviewDbContextConnection") ?? throw new InvalidOperationException("Connection string 'RestaurantReviewDbContextConnection' not found.");

builder.Services.AddDbContext<RestaurantReviewDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<RestaurantReviewUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<RestaurantReviewDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});
var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<RestaurantReviewDbContext>();
        var userManager = services.GetRequiredService<UserManager<RestaurantReviewUser>>();

        // Apply database migrations
        dbContext.Database.Migrate();

        // Seed data only if the database is empty
        //if (!dbContext.Review.Any())
        //{
            SeedData.Initialize(services);
        //}
    }
    catch (Exception ex)
    {
        // Handle any errors that occur during seeding
        Console.WriteLine("An error occurred while seeding the database.");
        Console.WriteLine(ex.Message);
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "login",
    pattern: "admin/{controller=Home}/{action=IndexAdmin}");


app.MapRazorPages();

app.Run();
