<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;
=======
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication3.RequestsModels.RequestModels;
>>>>>>> 14c48bd26432dbd3aefa76d46a51b3c0af07bebb

namespace WebApplication3.Controllers
{
    public class UserLoginRequest
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
=======
        [Route("api/[controller]")]
        [ApiController]
        // Let's thinkaabout renaming this controller to smth like UserManegementController
        public class LoginController : ControllerBase
        {
            private readonly UserManager<IdentityUser> _userManager;
            private IConfiguration _configuration; // readonly

            public LoginController(IConfiguration configuration, UserManager<IdentityUser> userManager)
            {
                _configuration = configuration;
                _userManager = userManager;
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

                    return Ok(new // Should we extract it to separate class?
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized(); // should be 403 Forbidden
            }

            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register(RegistrationRequestModel model)
            {
                var userExists = await _userManager.FindByEmailAsync(model.UserName);
                if (userExists != null)
                {
                    // Should be BadRequest with provided message
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseRequestModel { Status = "Error", Message = "User Already exists" });
                }

                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(), // Why we set this prop?
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    // Should be BadRequest in case of no exceptions
                    // let's be последовательны and use {} for IF statement
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseRequestModel { Status = "Error", Message = "User creaion failed. Check user detais and try again" });

                // Let's use 201Created instead of 200Ok
                return Ok(new ResponseRequestModel { Status = "success", Message = "User created successfully" });
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
>>>>>>> 14c48bd26432dbd3aefa76d46a51b3c0af07bebb
    }
}