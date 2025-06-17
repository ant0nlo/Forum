using ForumSentiment.Data;
using ForumSentiment.Models;
using ForumSentiment.Services;
using ForumSentiment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // нужно за FindFirstValue

namespace ForumSentiment.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SentimentService _sentimentService;

        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SentimentService sentimentService)
        {
            _context = context;
            _userManager = userManager;
            _sentimentService = sentimentService;
        }

        [HttpGet]
        [AllowAnonymous] // ако искаш формата да е достъпна и без логин
        public IActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var isOffensive = _sentimentService.IsOffensive(model.Content);

            var comment = new Comment
            {
                Content = model.Content,
                UserId = user.Id,
                PostId = model.PostId,
                IsFlagged = isOffensive,
                IsApproved = !isOffensive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new { id = model.PostId });
        }
    }
}
