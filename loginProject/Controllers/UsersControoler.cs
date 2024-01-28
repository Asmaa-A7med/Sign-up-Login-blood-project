using loginProject.Data;
using loginProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace loginProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersControoler : ControllerBase
    {

        private readonly DataContext _context;

        public UsersControoler(DataContext context)
        {
            _context = context;
        }

        // 1) Register New User

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.users.Any(r => r.NationalID == request.NationalID))
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new Users
            {
                UserName = request.UserName,
                BirthDate = request.BirthDate,
                Email = request.Email,
                NationalID = request.NationalID,
                BloodBank = request.BloodBank,
                City = request.City,
                Gender = request.Gender,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                verificationToken = CreateRandomToken()
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User successfully created!");

        }


        // 2) Login User

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }

            if (user.verifiedAy == null)
            {
                return BadRequest("Not verified!");
            }

            return Ok($"Welcome back, {user.UserName}! :)");

        }


        // 3) Verify User

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.verificationToken == token);
            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.verifiedAy = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("User verified.");

        }


        // 4) Forgot Password

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password.");

        }


        //5) Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");

        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var ComputedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return ComputedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }




    }
}


