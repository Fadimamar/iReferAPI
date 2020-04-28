using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using iReferAPI.Models;
using iReferAPI.Server.Services;

namespace iReferAPI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMailService _mailService;
        private IUserService _userService;
        private IConfiguration _configuration;
        public AuthController(IUserService userService, IConfiguration configuration, IMailService mailservice)
        {
            _userService = userService;
            _configuration = configuration;
            _mailService = mailservice;

        }
            // /api/auth/register
            [HttpPost("Register")]
        [ProducesResponseType(200, Type = typeof(UserManagerResponse))]
        [ProducesResponseType(400, Type = typeof(UserManagerResponse))]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest model)
        {
            
            if (ModelState.IsValid)
            {
                
                     var    result = await _userService.RegisterUserAsync(model);
                      

               
              

                if (result.IsSuccess)
                { 
                    //await _mailService.SendEmailAsync(model.Email,"Welcome to iRefer", "<h1>Welcome to iRefer</h1><P>Please let us know if you did not initiate this registration</P>");
                    //return Ok(result); // Status Code: 200 

                }
                return BadRequest(result);
            }

            return BadRequest(new UserManagerResponse
            {
                Message = "Some properties are not valid",
                IsSuccess = false
            }); // Status code: 400
        }
      
        // /api/auth/login
        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(UserManagerResponse))]
        [ProducesResponseType(400, Type = typeof(UserManagerResponse))]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest(new UserManagerResponse
            {
                Message = "Some properties are not valid",
                IsSuccess = false
            }); // Status code: 400
        }

        // /api/auth/confirmemail?userid=xxx&token=xxx
        [HttpPost("confirmEmail")]
        [ProducesResponseType(200, Type = typeof(UserManagerResponse))]
        [ProducesResponseType(400, Type = typeof(UserManagerResponse))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);
           
               

                if (result.IsSuccess)
            {
                   // return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
               return Ok(result);

            }
            return BadRequest(result); // Status code: 400
        }
    }
}