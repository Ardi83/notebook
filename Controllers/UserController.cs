using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using notebook.Controllers.Resources;
using notebook.Helpers;
using notebook.Models;

namespace notebook.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserResource model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userIdentity = _mapper.Map<CreateUserResource, ApplicationUser>(model);
                var result = await _userManager.CreateAsync(userIdentity, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userIdentity, "member");
                    // Send token trough email
                    string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(userIdentity).Result;
                    return Ok(result);
                }
                return BadRequest(Errors.AddErrorToModelState("register_failure", result.ToString(), ModelState));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("user/{id}")]
        [Authorize(Policy = Helpers.Constants.Strings.JwtClaims.ApiAccess)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpGet("users")]
        [Authorize(Policy = Helpers.Constants.Strings.JwtClaims.Admin)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return await Task.Run(() => {
                    var users = _userManager.Users;
                    return Ok(users);
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpPost("role")]
        [Authorize(Policy = Helpers.Constants.Strings.JwtClaims.Admin)]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleResource model)
        {
            try
            {
                IdentityResult result;
                var roleCheck = await _roleManager.RoleExistsAsync(model.Name);
                if (roleCheck)
                    return BadRequest("Role exist!");
                var roleIdentity = _mapper.Map<CreateRoleResource, ApplicationRole>(model);
                result = await _roleManager.CreateAsync(roleIdentity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("roles")]
        [AllowAnonymous]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpPost("addrole")]
        [Authorize(Policy = Helpers.Constants.Strings.JwtClaims.Admin)]
        public async Task<IActionResult> AddRole([FromBody]RoleUserResource model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.User);
                if (user == null)
                    return BadRequest("User can't found!");

                var roleCheck = await _roleManager.RoleExistsAsync(model.Role);
                if (!roleCheck)
                    return BadRequest("Role can't found!");

                var result = await _userManager.AddToRoleAsync(user, model.Role);
                return Ok("Role added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}