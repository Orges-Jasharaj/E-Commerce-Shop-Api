using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Responses;
using System.Security.Claims;

namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, List<string> roles);
        RefreshTokenDto GenerateRrefreshToken();

        ClaimsPrincipal GetClaimsPrincipal(string token);
    }
}
