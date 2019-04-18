using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using notebook.Auth;
using notebook.Controllers.Resources;
using notebook.Helpers;
using notebook.Models;

namespace notebook.Controllers
{
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        // POST api/login
        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Post([FromBody]CredentialsResource credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.Username, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(credentials.Username));
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.Username, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented }, userRoles);
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id.ToString()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        // POST api/auth/confirm
        [HttpPost("api/confirm")]
        public async Task<IActionResult> Confirm([FromBody]ConfirmResource confirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(confirm.Id);

            if (user == null)
            {
                return BadRequest(Errors.AddErrorToModelState("confirm_failure", "Invalid username.", ModelState));
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirm.Token);

            if (!result.Succeeded)
            {
                return BadRequest(Errors.AddErrorToModelState("confirm_failure", "Invalid token.", ModelState));
            }

            return new OkObjectResult(result);
        }
    }
}