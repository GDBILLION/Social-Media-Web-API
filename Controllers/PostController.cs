using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApi.DTOs.Posts;
using SocialMediaWebApi.Interfaces;
using SocialMediaWebApi.Models;
using System.Security.Claims;

namespace SocialMediaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);

            if (post == null) return NotFound();

            return Ok(post);
        }

        
        [HttpPost("create-post")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPostDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return BadRequest("UserId is null");

            var post = new Post
            {
                Content = createPostDTO.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
            };

            await _postRepository.CreatePostAsync(post);
            
            return Ok(new {  post.Id, Post = post.Content, Date = post.CreatedAt, Message = "Post created successfully." });
        }

        [Authorize]
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updated = await _postRepository.UpdatePostAsync(id, content);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _postRepository.DeletePostAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
