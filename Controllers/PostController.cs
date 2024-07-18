using Microsoft.AspNetCore.Mvc;
using MomosMatch.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MomosMatch.Controllers
{
    public class PostController : Controller
    {
        private readonly MyContext _context;

        public PostController(MyContext context)
        {
            _context = context;
        }

        // GET: Post
        public IActionResult Index()
        {
            var posts = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            return View(posts);
        }

        // GET: Post/Details/5
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.Now;
                post.UpdatedAt = DateTime.Now;
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return RedirectToAction("Index", "User");
                }
                post.UserId = userId.Value;

                _context.Posts.Add(post);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            // Ensure only the author can edit the post
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != post.UserId)
            {
                return Forbid(); // or handle unauthorized access appropriately
            }

            return View(post);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.UpdatedAt = DateTime.Now;
                    _context.Update(post);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        // POST: Post/Like/5
        [HttpPost]
        public IActionResult Like(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                var like = new PostLike
                {
                    PostId = id,
                    UserId = 1 // Replace with actual user ID
                };
                _context.PostLikes.Add(like);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Post/AddComment
        [HttpPost]
        public IActionResult AddComment(int postId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return RedirectToAction("Details", new { id = postId });
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "User");
            }

            var comment = new PostComment
            {
                PostId = postId,
                UserId = userId.Value,
                Comment = commentText
            };

            _context.PostComments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = postId });
        }
    }
}
