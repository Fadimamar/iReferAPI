using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using iReferAPI.Models;
using iReferAPI.Server.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using iReferAPI.Server.Data;
using Microsoft.AspNetCore.WebUtilities;

namespace iReferAPI.Server.Services
{
    

    public interface IUserService
    {

        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);

      
        Task<UserManagerResponse> LoginUserAsync(LoginRequest model);
        Task<UserManagerResponse> ConfirmEmailAsync(string UserId, string token );
        Task<UserManagerResponse> ForgotPasswordAsync(string email);
        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordRequest model);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

       
        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext db,   IMailService mailservice)
        {
            _userManger = userManager;
            _configuration = configuration;
            _db = db;
            _mailService = mailservice;

        }

      


        public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model)
        {
            if (model == null)
                throw new NullReferenceException("Reigster Model is null");
           

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName, 
                LastName = model.LastName
            };



            var result = await _userManger.CreateAsync(identityUser, model.Password);


            if (result.Succeeded)
            {
                var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(identityUser);

                //string Role = "Customer";

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userid={identityUser.Id}&token={validEmailToken}";
                await _mailService.SendEmailAsync(identityUser.Email, "Confirm you email", "<h1> Welcome to iRefer</h1>" +
                    $"<p> Please confirm your email by <a href='{url}'> clicking Here</p>");
                ////var result2 = await _userManger.AddToRoleAsync(identityUser, Role);
                //if (result2.Succeeded)
                //{ 
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
                //}
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }

      



        public async Task<UserManagerResponse> LoginUserAsync(LoginRequest model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            var roles = await _userManger.GetRolesAsync(user);
            
            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Role,roles[0])
            };
            //foreach (string role in roles)
            //    { 
            //    i
            //    claims[claims.Length+1]
            //}
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                UserInfo = claims.ToDictionary(c => c.Type, c => c.Value),
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string UserId, string token)
        {
            var user =await _userManger.FindByIdAsync(UserId);
            if (user == null)
                return new UserManagerResponse { IsSuccess = false, Message = "User not found" };
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var normalToken= Encoding.UTF8.GetString(decodedToken);
            var result = await _userManger.ConfirmEmailAsync(user, normalToken);
            if (result.Succeeded)
                return new UserManagerResponse
                { Message = "Email Confirmed successfully!",
                    IsSuccess = true 
                };
            return new UserManagerResponse
            {
                Message = "Email was not Confirmed!",
                IsSuccess = false,
                Errors=result.Errors.Select(e=>e.Description)
            };
        }

        public async Task<UserManagerResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user is associated with this email",
                };
            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedRestToken = Encoding.UTF8.GetBytes(token);
            var validRestToken = WebEncoders.Base64UrlEncode(encodedRestToken);
            string url = $"{_configuration["AppUrl"]}/resetPassword?email={email}&token={validRestToken}";
            var res = await _mailService.SendEmailAsync(email, "Reset Password", "<h1> Instructions</h1>" +
                   $"<p> To reset your password please <a href='{url}'> click Here</a></p>");
            if (res.StatusCode == System.Net.HttpStatusCode.Accepted || res.StatusCode == System.Net.HttpStatusCode.OK)
                return new UserManagerResponse
                { 
                    IsSuccess = true,
                    Message = "Rest password URL has been sent by email succesflly!"
                };
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Couldnt send Reset Email"
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No User is accosiated with this email"

                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };
            
            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Password was changed successfuly!"

                };

            return new UserManagerResponse
            {
                Message = "Password Reset failed!",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}
