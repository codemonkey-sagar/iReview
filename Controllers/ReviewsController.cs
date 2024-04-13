using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EatEvals.Data;
using EatEvals.Models;
using System.IO;


namespace EatEvals.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly RestaurantReviewDbContext _context;

        public ReviewsController(RestaurantReviewDbContext context)
        {
            _context = context;
            ViewBag.isAdmin = true;
        }


        // GET: Reviews
        public async Task<IActionResult> Index(string restaurant = "", string food = "", string searchField = "", string searchQuery = "")
        {

            if (_context.Review == null)
            {
                return Problem("Entity set 'RestaurantReviewDbContext.Review' is null.");
            }

            var reviews = from m in _context.Review
                          select m;

            if (!String.IsNullOrEmpty(restaurant))
            {
                reviews = reviews.Where(s => s.Restaurant!.Contains(restaurant));
            }

            if (!String.IsNullOrEmpty(food))
            {
                reviews = reviews.Where(s => s.Food!.Contains(food));
            }
            if (!String.IsNullOrEmpty(searchQuery))
            {
                switch (searchField)
                {
                    case "RestaurantName":
                        reviews = reviews.Where(s => s.Restaurant.Contains(searchQuery));
                        break;
                    case "FoodName":
                        reviews = reviews.Where(s => s.Food.Contains(searchQuery));
                        break;
                    case "PublishingDate":
                        if (DateTime.TryParse(searchQuery, out var date))
                        {
                            reviews = reviews.Where(s => s.Date.Date == date.Date);
                        }
                        break;
                    // Add more cases as needed.
                    default:
                        // Optionally handle the default case.
                        break;
                }
            }
            ViewBag.restaurant = restaurant;
           
            ViewBag.food = food;
            ViewBag.SearchField = searchField;
            ViewBag.SearchQuery = searchQuery;

            return View(await reviews.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }
        // GET: Reviews/Create
        public IActionResult Create()
        {
            // Get restaurant names from the seedData.cs file
            var restaurantNames = _context.Review.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Restaurant }).ToList();
            ViewBag.RestaurantNames = restaurantNames;

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reviewviewmodel review)
        {
            string filename = "";
            if (review.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await review.Photo.CopyToAsync(memoryStream);

                    Review eatEvalsReview = new Review()
                    {
                        Id = review.Id,
                        Restaurant = review.Restaurant,
                        Food = review.Food,
                        Price = review.Price,
                        Score = review.Score,
                        Date = review.Date,
                        Image = memoryStream.ToArray()
                    };
                    _context.Add(eatEvalsReview);
                    await _context.SaveChangesAsync();

                    ViewBag.success = "record added";
                    return RedirectToAction(nameof(Index));

                }
            }

            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review.Image != null)
            {
                return File(review.Image, "image/jpeg"); // Change "image/jpeg" based on your image type
            }
            return NotFound();
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Restaurant,Food,Price,Score,Date,Photo")] Reviewviewmodel review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string filename = "";

                    if (review.Photo != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await review.Photo.CopyToAsync(memoryStream);

                            Review eatEvalsReview = new Review()
                            {
                                Id = review.Id,
                                Restaurant = review.Restaurant,
                                Food = review.Food,
                                Price = review.Price,
                                Score = review.Score,
                                Date = review.Date,
                                Image = memoryStream.ToArray()
                            };
                            _context.Update(eatEvalsReview);
                            await _context.SaveChangesAsync();

                            ViewBag.success = "record updated";


                        }
                    }
                    else // no change photo, only data
                    {

                        Review eatEvalsReview = new Review()
                        {
                            Id = review.Id,
                            Restaurant = review.Restaurant,
                            Food = review.Food,
                            Price = review.Price,
                            Score = review.Score,
                            Date = review.Date
                        };
                        _context.Update(eatEvalsReview);
                        _context.Entry(eatEvalsReview).Property(x => x.Image).IsModified = false; //avoid try change the image

                        await _context.SaveChangesAsync();

                        ViewBag.success = "record Updated";

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
