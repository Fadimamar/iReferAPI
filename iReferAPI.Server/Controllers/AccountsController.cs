using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iReferAPI.Models;
using iReferAPI.Server.Services;

namespace iReferAPI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsSerivce)
        {
            _accountsService = accountsSerivce;   
        }

        #region GET
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Account>))]
        [HttpGet("agency={agencyId}")]
        public IActionResult Get(string agency)
        {
            if (agency == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Accounts = _accountsService.GetAgencyAccounts(agency, userId);
            return Ok(new CollectionResponse<Account>
            {
                Count = Accounts.Count(),
                IsSuccess = true,
                Message = "Accounts retrieved successfully!",
                Records = Accounts
            });
        }

        // Get not acheived Accounts 
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Account>))]
        [HttpGet("notachieved")]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Accounts = _accountsService.GetAllAccounts(userId);
            return Ok(new CollectionResponse<Account>
            {
                Count = Accounts.Count(),
                IsSuccess = true,
                Message = "Accounts retrieved successfully!",
                Records = Accounts
            });
        }

        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<Account>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Account>))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AccountRequest model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var agency = await _accountsService.CreateAccountAsync(model.AgencyId, model.ReferralURL, model.ReferralCode, userId);

                return Ok(new OperationResponse<Account>
                {
                    IsSuccess = true,
                    Message = "Account has been inserted successfully",
                    Record = agency
                });
            }

            return BadRequest(new OperationResponse<Account>
            {
                IsSuccess = false,
                Message = "Some properties are not valid"
            });
        }
        #endregion

        #region PUT
        [ProducesResponseType(200, Type = typeof(OperationResponse<Account>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Account>))]
        [ProducesResponseType(404)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]AccountRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                var agency = await _accountsService.EditAccountsAsync(model.Id, model.ReferralURL, model.ReferralCode, userId);
                if (agency == null)
                    return NotFound();

                return Ok(new OperationResponse<Account>
                {
                    IsSuccess = true,
                    Message = "Account has been edited successfully",
                    Record = agency
                });
            }

            return BadRequest(new OperationResponse<Account>
            {
                IsSuccess = false,
                Message = "Some properties are not valid"
            });
        }

        /// <summary>
        ///// Change the status of the Account
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[ProducesResponseType(200, Type = typeof(OperationResponse<Account>))]
        //[ProducesResponseType(400, Type = typeof(OperationResponse<Account>))]
        //[ProducesResponseType(404)]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id))
        //        return NotFound();

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    var agency = await _accountsService.MarkAccountAsync(id, userId);
        //    if (agency == null)
        //        return NotFound();

        //    return Ok(new OperationResponse<Account>
        //    {
        //        IsSuccess = true,
        //        Message = "Account status changed successfully! successfully!",
        //        Record = agency
        //    });
        //}
        #endregion

        #region DELETE
        [ProducesResponseType(200, Type = typeof(OperationResponse<Account>))]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var agency = await _accountsService.DeleteAccountAsync(id, userId);
            if (agency == null)
                return NotFound();

            return Ok(new OperationResponse<Account>
            {
                IsSuccess = true,
                Message = "Account deleted successfully!",
                Record = agency
            });
        }
        #endregion

    }
}