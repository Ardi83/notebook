using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace notebook.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, IList<string> userRoles);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}