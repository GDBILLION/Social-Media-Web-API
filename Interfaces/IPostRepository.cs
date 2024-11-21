using SocialMediaWebApi.DTOs.Posts;
using SocialMediaWebApi.Models;

namespace SocialMediaWebApi.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(int postId);
        Task CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(int postId, string content);
        Task<bool> DeletePostAsync(int postId);
    }
}
