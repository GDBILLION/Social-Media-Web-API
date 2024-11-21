using Microsoft.AspNetCore.Identity;

namespace SocialMediaWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
            //model.Bio ?? string.Empty

        public string? ProfilePictureUrl { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; set; } 
        public ICollection<Follower> Followers { get; set; } 
        public ICollection<Follower> Following { get; set; } 
        public ICollection<Like> Likes { get; set; } 
        public ICollection<Comment> Comments { get; set; } 
        public ICollection<Notification> Notifications { get; set; }
    }
}
