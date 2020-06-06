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
        private const int PAGE_SIZE = 10;
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsSerivce)
        {
            _accountsService = accountsSerivce;   
        }

        #region GET
        [ProducesResponseType(200, Type = typeof(OperationResponse<Agency>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Agency>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByAccountID(string id)

        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var account = await _accountsService.GetAccountByIdAsync(id);
            if (account == null)
                return BadRequest(new OperationResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid operation",
                });

            return Ok(new OperationResponse<Account>
            {
                Record = account,
                Message = "Account retrieved successfully!",
                IsSuccess = true,
                OperationDate = DateTime.UtcNow
            });

        }
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Account>))]
        [HttpGet("Agency={agencyId}")]
        public IActionResult GetByAgencyID(string Agency, int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var allowed = User.IsInRole("SysAdmin") || User.IsInRole("AgencyAdmin");
            int totalAccounts = 0;
            if (page == 0)
                page = 1;
            IEnumerable<Account> Accounts;
            if (allowed)
            {
                Accounts = _accountsService.GetAgencyAccounts(Agency, PAGE_SIZE, page, out totalAccounts);


                int totalPages = 0;
                if (totalAccounts % PAGE_SIZE == 0)
                    totalPages = totalAccounts / PAGE_SIZE;
                else
                    totalPages = (totalAccounts / PAGE_SIZE) + 1;

                return Ok(new CollectionPagingResponse<Account>
                {
                    Count = totalAccounts,
                    IsSuccess = true,
                    Message = "Agencies received successfully!",
                    OperationDate = DateTime.UtcNow,
                    PageSize = PAGE_SIZE,
                    Page = page,
                    Records = Accounts
                });
            }
            else
                return Unauthorized(new OperationResponse<string>
                {
                    IsSuccess = false,
                    Message = "Not Authorized",
                });
            

        }

        // Get not acheived Accounts 
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Account>))]
        [HttpGet()]
        public IActionResult Get(int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var allowed = User.IsInRole("SysAdmin") || User.IsInRole("AgencyAdmin");
            int totalAccounts = 0;
            if (page == 0)
                page = 1;
            IEnumerable<Account> Accounts;
            if (allowed)
            {
                Accounts = _accountsService.GetAllAccounts( PAGE_SIZE, page, out totalAccounts);


                int totalPages = 0;
                if (totalAccounts % PAGE_SIZE == 0)
                    totalPages = totalAccounts / PAGE_SIZE;
                else
                    totalPages = (totalAccounts / PAGE_SIZE) + 1;

                return Ok(new CollectionPagingResponse<Account>
                {
                    Count = totalAccounts,
                    IsSuccess = true,
                    Message = "Accounts received successfully!",
                    OperationDate = DateTime.UtcNow,
                    PageSize = PAGE_SIZE,
                    Page = page,
                    Records = Accounts
                });
            }
            else
                return Unauthorized(new OperationResponse<string>
                {
                    IsSuccess = false,
                    Message = "Not Authorized",
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