using Microsoft.IdentityModel.Tokens;
using SocialMediaWebApi.Data;
using SocialMediaWebApi.Interfaces;
using SocialMediaWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMediaWebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly ApplicationDbContext _context;

        public TokenService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["JWT:SigningKey"]));
        }
        public string CreateToken(ApplicationUser applicationUser)
        {
            var roleId = _context.UserRoles.Where(ur=>ur.UserId==applicationUser.Id).Select(ur=>ur.RoleId).FirstOrDefault();
            var role = _context.Roles.Where(r=>r.Id==roleId).Select(r=>r.Name).FirstOrDefault();

            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, applicationUser.Id),
                new Claim(ClaimTypes.Role, role)
            };

            var credential = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credential,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);

            
        }
    }
}
