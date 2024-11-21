using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApi.Data;
using SocialMediaWebApi.DTOs.Account;
using SocialMediaWebApi.Interfaces;
using SocialMediaWebApi.Models;

namespace SocialMediaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = new ApplicationUser
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    UserName = registerDTO.FirstName.ToLower(),
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    NormalizedEmail = registerDTO.Email.ToUpper()
                };

                var newUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if (!newUser.Succeeded) return StatusCode(500, newUser.Errors);

                var role = await _userManager.AddToRoleAsync(user, UserRoles.User);

                if (role.Succeeded)
                {
                    return Ok("User Registered Successfully");
                }

                return StatusCode(500, role.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null) return Unauthorized("Invalid email address");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

                if (result.Succeeded)
                {
                    return Ok(new ResponseDTO
                    {
                        Message = "Login successful",
                        Name = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user),
                    });
                }

                return Unauthorized("Invalid username or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
