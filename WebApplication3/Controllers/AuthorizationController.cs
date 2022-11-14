using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication3.RequestsModels.RequestModels;

namespace WebApplication3.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class AuthorizationController : ControllerBase
        {
            private readonly ILogger<AuthorizationController> _logger;
            private readonly UserManager<IdentityUser> _userManager;
            private readonly IConfiguration _configuration;

            public AuthorizationController(IConfiguration configuration, UserManager<IdentityUser> userManager, ILogger<AuthorizationController> logger)
            {
                _configuration = configuration;
                _userManager = userManager;
                _logger = logger;
            }

            [AllowAnonymous]
            [Route("login")]
            [HttpPost]
            public async Task<IActionResult> Login(UserLoginRequestModel userLogin)
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);

                // var user = Authentificate(userLogin);
                if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    var authClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Why it is here?
                    };

                    var token = GetToken(authClaims);

                    _logger.LogInformation("User was found and new token was generated");
                    return Ok(new // Should we extract it to separate class?
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                _logger.LogError("User was not found or credentials werent correct");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register(RegistrationRequestModel model)
            {
                var userExists = await _userManager.FindByEmailAsync(model.UserName);
                if (userExists != null)
                {
                    // Should be BadRequest with provided message
                    _logger.LogError("user already exist");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(), // Why do we set this prop?
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                _logger.LogError("User creaion failed. Check user detais and try again");
                return StatusCode(StatusCodes.Status400BadRequest);
                }
                // Should be BadRequest in case of no exceptions
                // let's be последовательны and use {} for IF statement

                _logger.LogInformation("User was created");
                return Ok(StatusCodes.Status201Created);
            }


            private JwtSecurityToken GetToken(List<Claim> authClaims)
            {
                var authSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["Lwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSecurityKey, SecurityAlgorithms.HmacSha256)
                    );
                return token;
            }

          
        }
    
}
