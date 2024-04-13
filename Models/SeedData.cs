using Microsoft.EntityFrameworkCore;
using EatEvals.Data;

namespace EatEvals.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestaurantReviewDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestaurantReviewDbContext>>()))
            {
                // Look for any Review.
                if (context.Review.Any())
                {
                    return;   // DB has been seeded
                }

                byte[] image = Convert.FromBase64String("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");


                context.Review.AddRange(
new Review
{
    Restaurant = "Urban Eatery",
    Score = 1,
    Food = "Vegan Burger",
    Price = 12.75F,
    Date = DateTime.Parse("2023-12-03 13:00"),
    Image = image
},
                new Review
                {
                    Restaurant = "Steakhouse 88",
                    Score = 2,
                    Food = "Beef Steak",
                    Price = 25.00F,
                    Date = DateTime.Parse("2023-12-04 19:45"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Ocean's Plate",
                    Score = 3,
                    Food = "Seafood Pasta",
                    Price = 22.30F,
                    Date = DateTime.Parse("2023-12-05 20:00"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Sushi Zen",
                    Score = 4,
                    Food = "Sushi Platter",
                    Price = 18.90F,
                    Date = DateTime.Parse("2023-12-06 18:15"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Lamb & Spice",
                    Score = 5,
                    Food = "Lamb Chops",
                    Price = 29.95F,
                    Date = DateTime.Parse("2023-12-07 21:30"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Garden Bistro",
                    Score = 1,
                    Food = "Caesar Salad",
                    Price = 9.00F,
                    Date = DateTime.Parse("2023-12-08 12:00"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Tomato's Home",
                    Score = 2,
                    Food = "Tomato Soup",
                    Price = 7.50F,
                    Date = DateTime.Parse("2023-12-09 12:45"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Pizza Haven",
                    Score = 3,
                    Food = "Pepperoni Pizza",
                    Price = 14.00F,
                    Date = DateTime.Parse("2023-12-10 17:00"),
                    Image = image
                },
                new Review
                {
                    Restaurant = "Sweet Spot",
                    Score = 4,
                    Food = "Chocolate Cake",
                    Price = 6.20F,
                    Date = DateTime.Parse("2023-12-11 15:30"),
                    Image = image
                }

                );
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving data: {ex.Message}");
                    // You can log the exception or handle it in any other appropriate way
                }
            }
        }

    }
}


