using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography.X509Certificates;
using MomosMatch.Models;
using Microsoft.EntityFrameworkCore;


namespace MomosMatch.Controllers;

public class UserController : Controller
{
    private readonly MyContext _context;  

    public UserController(MyContext context){
        _context = context;
    }

/* ---------- Dashboard ---------- */

[HttpGet("")]
    public IActionResult Index()
    {
        return View();
        }


/* ---------- Create ---------- */

[HttpPost("user/create")]
    public IActionResult CreateUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();           
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password); 
            _context.Add(newUser);
            _context.SaveChanges();

            // Store UserId and FirstName in session
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.FirstName);

            return RedirectToAction("Dashboard", "Pet");  
        }
        
        return View("Index");
    }

// /* ---------- Login ---------- */

[HttpPost("User/LoginUser")]
public IActionResult Login(UserLogin userSubmission)
{    
    if(ModelState.IsValid)    
    {             
        User? userInDb = _context.User.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);        
        
        // If no user exists with the provided email        
        if(userInDb == null)        
        {         
            ModelState.AddModelError("LoginEmail", "Email not found. Please register if you haven't.");            
            return View("Index");        
        } 

        PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin>();                         
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);                                   
        
        // Check if password does not match
        if(result != PasswordVerificationResult.Success)        
        {   
            ModelState.AddModelError("LoginPassword", "Incorrect password.");            
            return View("Index");                
        } 

        // Store UserId and FirstName in session
        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        HttpContext.Session.SetString("UserName", userInDb.FirstName);
        
        return RedirectToAction("Dashboard", "Pet");               
        } 
    return View("Index");
}

// /* ---------- User Details Page ---------- */
[HttpGet("account/{id}")]
public IActionResult AccountDetails(int id)
{
    int? userId = HttpContext.Session.GetInt32("UserId");

    if (!userId.HasValue)
    {
        return RedirectToAction("Index", "User"); 
    }

    var currentUser = _context.User
        .Include(u => u.LikedPets)
            .ThenInclude(lp => lp.Pet)
        .FirstOrDefault(u => u.UserId == id);

    if (currentUser == null)
    {
        return NotFound(); // Handle the case where user with given id is not found
    }

    var model = new
    {
        currentUser.UserId,
        currentUser.FirstName,
        currentUser.LastName,
        currentUser.Email,
        LikeCount = currentUser.LikedPets.Count,
        LikedPets = currentUser.LikedPets.Select(lp => new
        {
            PetId = lp.PetId,
            PetName = lp.Pet.Name,
            PetImg = lp.Pet.PhotoUrl
        }).ToList()
    };

    return View("UserDetailsPage", model);
}


//  /*----------- Likes Page ---------------*/
    [HttpGet("LikesPage")]
    public IActionResult LikesPage(){
        return View();
    }

// /* ---------- Log out ---------- */

[HttpPost("Logout")]
public IActionResult Logout() 
{
HttpContext.Session.Clear();
return RedirectToAction("Index");
}



public IActionResult Privacy()
    {
        return View();
    }

}