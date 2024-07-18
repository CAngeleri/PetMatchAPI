using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using MomosMatch.Models;
using System;
using System.Linq;


namespace MomosMatch.Controllers
{
    public class PetController : Controller
    {
        private readonly MyContext _context;

        public PetController(MyContext context)
        {
            _context = context;
        }

        /* ---------- Dashboard ---------- */

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            // Retrieve pets including their related data
            var pets = _context.Pet
                .Include(p => p.LikeList)
                .Include(p => p.Owner)
                .ToList();

            // Retrieve UserId and UserName from session
            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userName = HttpContext.Session.GetString("UserName");

            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "User");
            }

            ViewData["CurrentUserName"] = userName;
            return View("Dashboard", pets);
        }

        /* ---------- Details Page ---------- */

        [HttpGet("Pet/{id}")]
        public IActionResult PetDetailsPage(int id)
        {
            var pet = _context.Pet
                .Include(p => p.LikeList)
                    .ThenInclude(l => l.User)
                .Include(p => p.Owner)
                .FirstOrDefault(p => p.PetId == id);

            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userName = HttpContext.Session.GetString("UserName");

            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "User");
            }

            ViewData["CurrentUserName"] = userName;
            return View("PetDetailsPage", pet);
        }

        /* ---------- Gallery Page ---------- */
[HttpGet("Pet/Gallery")]
public IActionResult GalleryPage(string type = "all", string size = "all", int age = 0, string gender = "all")
{
    // Retrieve pets including their related data
    var petsQuery = _context.Pet
        .Include(p => p.LikeList)
        .Include(p => p.Owner)
        .AsQueryable(); // Start with IQueryable for dynamic filtering

    // Filter pets based on parameters
    if (type != "all")
    {
        petsQuery = petsQuery.Where(p => p.Type.ToLower() == type.ToLower());
    }

    if (size != "all")
    {
        petsQuery = petsQuery.Where(p => p.Size.ToLower() == size.ToLower());
    }

    if (age > 0)
    {
        petsQuery = petsQuery.Where(p => p.Age == age);
    }

    if (gender != "all")
    {
        petsQuery = petsQuery.Where(p => p.Gender.ToLower() == gender.ToLower());
    }

    // Execute query to get filtered pets
    var pets = petsQuery.ToList();

    // Retrieve UserId and UserName from session
    int? userId = HttpContext.Session.GetInt32("UserId");
    string? userName = HttpContext.Session.GetString("UserName");

    if (!userId.HasValue)
    {
        return RedirectToAction("Index", "User");
    }

    return View("Gallery", pets);
}


        /* ---------- Like/Unlike Action ---------- */

        [HttpPost("Pet/Like/{PetId}")]
        public IActionResult Like(int PetId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Check if the user has already Liked/Unliked the pet
            var existingLike = _context.Like.FirstOrDefault(l => l.PetId == PetId && l.UserId == userId);

            if (existingLike != null)
            {
                _context.Like.Remove(existingLike);
            }
            else
            {
                var newLike = new Like
                {
                    PetId = PetId,
                    UserId = userId.Value
                };

                _context.Like.Add(newLike);
            }

            _context.SaveChanges();

            return RedirectToAction("PetDetailsPage", "Pet", new { id = PetId });
        }
    }
}
