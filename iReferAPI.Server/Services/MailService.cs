using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using iReferAPI.Models;
using iReferAPI.Server.Models;
using Microsoft.Extensions.Configuration;


namespace iReferAPI.Server.Services
{

    public interface IMailService
    {

        Task<Response> SendEmailAsync(String toEmail, string subject, string content); 
    
    }
    public class SendGridMailService : IMailService

    {
        private IConfiguration _configuration;
        public SendGridMailService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        
        }
        public async Task<Response> SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("info@westlinesoft.com", "iRefer Confirmation");
            var to = new EmailAddress(toEmail);
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var  response = await client.SendEmailAsync(msg);
            return response;
            //if (response.StatusCode != )

            //    return new EmailResponse
            //    {
            //        IsSuccess = true,
            //        Message = "Email was sent succesfuly!"
            //};
            //return new EmailResponse
            //{
            //    IsSuccess = false,
            //    Message = response.Body.ReadAsStringAsync().Result
            //};


        }
    }
}
