using System.ComponentModel.DataAnnotations;

namespace ForumSentiment.ViewModels.PostViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}