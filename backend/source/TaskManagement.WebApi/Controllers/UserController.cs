using AutoMapper;
using TaskManagement.Domain.UserDetail;
using TaskManagement.Domain.UserDetails;
using TaskManagementWebApi.Models;
using TaskManagementWebApi.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Common.AppRoles;

namespace TaskManagementWebApi.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="roleManager">roleManager</param>
        /// <param name="configuration">configuration</param>
        /// <param name="_usersService">usersService</param>
        /// <param name="_mapper">mapper</param>
        public UserController(
             UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             IConfiguration configuration,
             IUsersService _usersService,
             IMapper _mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            usersService = _usersService;
            mapper = _mapper;
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <returns>Login Details</returns>
        /// <response code="400">Invalid credetials.</response>
        /// <response code="500">Internal server error.</response>
        /// <response code="200">OK.</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userDetails = await usersService.GetUserByIdAsync(Guid.Parse(user.Id));
                    var viewModel = mapper.Map<LoginViewModel>(userDetails);
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim("timeStamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture)),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);
                    viewModel.token = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(viewModel);
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid credentials." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// User registration.
        /// </summary>
        /// <returns>User Details</returns>
        /// <response code="400">User already exists!. <br /> <br /> Email not send.</response>
        /// <response code="500">Internal server error.</response>
        /// <response code="200">OK.</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "User already exists!" });
                }

                if (!(model.Role.Equals(AppRoles.User, StringComparison.OrdinalIgnoreCase)
                    || model.Role.Equals(AppRoles.Admin, StringComparison.OrdinalIgnoreCase)))
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                        new Response { Status = "Error", Message = "Role should be Admin or User" });
                }


                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                }
                else
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);

                    var userDetails = mapper.Map<User>(user);
                    userDetails.Username = model.UserName;
                    userDetails.CreatedAt = model.CreatedAt;
                    userDetails.Role = model.Role;
                    var user1 = await usersService.AddUserAsync(userDetails);
                    var viewModel = mapper.Map<UserDetailViewModel>(user1);
                    viewModel.EmailConfirmed = user.EmailConfirmed;
                    viewModel.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    return Ok(viewModel);
                }
             }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        /// <response code="500">Internal server error.</response>
        /// <response code="200">OK.</response>
        [Authorize]
        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await usersService.GetUsersAsync();
                var userDetails = mapper.Map<List<UserTaskDetailViewModel>>(users);
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        #region Private Methods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays (7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        #endregion

    }


}







