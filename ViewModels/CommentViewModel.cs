namespace ForumSentiment.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsApproved { get; set; }
        public string? ApprovedByModeratorEmail { get; set; }
        public string Text => Content; 
    }
}
