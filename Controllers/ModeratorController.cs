using ForumSentiment.Data;
using ForumSentiment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumSentiment.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModeratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var flaggedComments = await _context.Comments
                .Where(c => c.IsFlagged && !c.IsApproved)
                .Include(c => c.User) 
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.Email
                })
                .ToListAsync();

            return View(flaggedComments);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            comment.IsApproved = true;
            comment.ApprovedByModeratorEmail = User.Identity?.Name ?? "Unknown";

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int postId)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
