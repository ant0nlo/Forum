using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumSentiment.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null!;
        public bool IsFlagged { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ApprovedByModeratorId { get; set; }
        public string? ApprovedByModeratorEmail { get; set; }
        public ApplicationUser? ApprovedByModerator { get; set; }
    }
}
