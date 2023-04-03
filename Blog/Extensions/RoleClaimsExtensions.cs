using System.Security.Claims;
using blog.Models;

namespace blog.Extensions
{
    public static class RoleClaimsExtensions
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Email)
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

            return claims;
        }
    }
}