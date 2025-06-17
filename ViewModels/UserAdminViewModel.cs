namespace ForumSentiment.ViewModels
{
    public class UserAdminViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; }
    }
}
