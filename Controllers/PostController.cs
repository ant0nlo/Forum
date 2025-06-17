using ForumSentiment.Data;
using ForumSentiment.Models;
using ForumSentiment.ViewModels;
using ForumSentiment.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumSentiment.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.User).ToListAsync();
            return View(posts);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                UserId = user.Id
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                 .Include(p => p.User)
                 .Include(p => p.Comments)
                     .ThenInclude(c => c.User)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                NewComment = new CommentViewModel { PostId = post.Id },
                Comments = _context.Comments
                    .Where(c => c.PostId == post.Id && c.IsApproved)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList()
            };

            return View(viewModel);
        }
        
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, Post updatedPost)
        {
            if (id != updatedPost.Id) return BadRequest();

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}