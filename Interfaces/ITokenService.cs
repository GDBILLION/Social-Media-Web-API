using SocialMediaWebApi.Models;

namespace SocialMediaWebApi.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUser applicationUser);
    }
}
