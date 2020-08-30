using System.Linq;
using System.Security.Claims;

namespace PetGame.Web
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubject(this ClaimsPrincipal self)
        {
            return self.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
