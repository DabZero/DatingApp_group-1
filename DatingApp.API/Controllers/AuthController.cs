using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            this._config = config;
            this._repo = repo;
        }



        // POST api/auth/register
        // ----
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            //If userName has already been taken ... return BadRequest
            //
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");


            else
            {   //Create User with encrpted password & return status coode
                //
                var userToCreate = new User { UserName = userForRegisterDto.Username };

                User createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

                return StatusCode(201);
            }
        }



        // POST api/auth/login
        // ----
        [HttpPost("login")]
        public async Task<IActionResult> login(UserFromLoginDto userFromLoginDto)
        {

            //Check if Login credentials match against the DB
            //
            var userFromRepo = await _repo.Login(userFromLoginDto.Username, userFromLoginDto.Password);

            //If Login credentials do not match...the user is unauthorized
            //
            if (userFromRepo == null)
                return Unauthorized();

            //Start building Claims for UserName and password.  Claim = Build Identity of user
            //We already verified that this usrNm/pass exists
            //
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString() ),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            //We define this key in our appSettings.json but, a key must be in bytes[]
            //The key is required for the Server to sign the Token
            //
            var key = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //In order for Server to sign the token.  Our key must be hashed using a security algorithm.  
            //The Server Validates the Token by signing using the key... Microsoft.IdentityModel.Tokens
            //
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Bundle (Claims we made about the user + Validation = Server Signed Token "creds")
            //
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            //The token needs a handler to deal with the token in a secure way
            //
            var tokenHandler = new JwtSecurityTokenHandler();

            //Create a JWT token and pass the bundles properties of the token
            //Contains the JWT token that we want to return to our client
            //
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Convert user to Dto with photoUrl info, not full user so, only limited info passed
            // This is passed on login so that, we can save the main photo will be passed to 
            // local storage.  We will use photoUrl to display member picture in NavBar
            //
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            // Return the JWT Token as an (obj) Token to the Client
            // Serialize/Write token (obj) as a response back to the client
            // Anonymous object passed that we can customize
            //
            return Ok(
                new
                {
                    token = tokenHandler.WriteToken(token),
                    user
                }
            );
        }
    }


}