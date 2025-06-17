using System.ComponentModel.DataAnnotations;
using ForumSentiment.Models;

namespace ForumSentiment.ViewModels.PostViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public CommentViewModel NewComment { get; set; }
    }
}