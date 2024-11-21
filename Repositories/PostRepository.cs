using Microsoft.EntityFrameworkCore;
using SocialMediaWebApi.Data;
using SocialMediaWebApi.DTOs.Posts;
using SocialMediaWebApi.Interfaces;
using SocialMediaWebApi.Models;

namespace SocialMediaWebApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreatePostAsync(Post post)
        {
           
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();
            
        }

        public async Task<PostDto> GetPostByIdAsync(int postId)
        {
            var Post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == postId);


            if (Post == null) return null;

            return new PostDto
            {
                Content = Post.Content,
                CreatedAt = Post.CreatedAt,
                UserId = int.Parse(Post.UserId)
            };
        }

        public async Task<bool> UpdatePostAsync(int postId, string content)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p=> p.Id == postId);

            if (post == null) return false;

            post.Content = content;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
