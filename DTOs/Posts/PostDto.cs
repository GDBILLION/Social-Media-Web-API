namespace SocialMediaWebApi.DTOs.Posts
{
    public class PostDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
