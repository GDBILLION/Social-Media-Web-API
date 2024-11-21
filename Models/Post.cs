namespace SocialMediaWebApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}