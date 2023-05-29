using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
     
        public AccountController(DataContext context, ITokenService tokenService) //Constructor where defined the Services that are injected
        {
            _tokenService = tokenService;
            _context = context;


        }
 
        [HttpPost("register")] // POST: api/account/register is the route to hit this End Point, the convention would automatically bind parameters
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512(); // using instruct to destroy this variable once is no longer used - that implement the IDisposible methode

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user); // this does not do anything in the database, it is just tracking a new entity AppUser in memory 
            await _context.SaveChangesAsync(); // here is were the changes are applied to the Database

            return new UserDto
            {
                Username = user.UserName,
                Token =_tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x =>
                x.UserName == loginDto.Username.ToLower());
                
            if (user == null) return Unauthorized("Invalid Username");   

            using var hmac = new HMACSHA512(user.PasswordSalt); // used in the reverse way by providing the PasswordSalt this will return the hash

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            };

            return new UserDto
            {
                Username = user.UserName,
                Token =_tokenService.CreateToken(user)
            };
        }

        private async Task<bool>UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}