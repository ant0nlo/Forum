using Microsoft.AspNetCore.Identity;

namespace ForumSentiment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
