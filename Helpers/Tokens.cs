using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using notebook.Auth;
using notebook.Models;

namespace notebook.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings, IList<string> userRoles)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity, userRoles),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };
            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}