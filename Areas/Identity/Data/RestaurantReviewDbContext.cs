using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EatEvals.Areas.Identity.Data;
using EatEvals.Models;

namespace EatEvals.Data;

public class RestaurantReviewDbContext : IdentityDbContext<RestaurantReviewUser>
{
    public RestaurantReviewDbContext(DbContextOptions<RestaurantReviewDbContext> options)
        : base(options)
    {
    }
    //Review is used for table name
    public DbSet<Review> Review { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
