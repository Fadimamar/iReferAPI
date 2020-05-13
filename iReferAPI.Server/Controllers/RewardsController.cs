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
    public class RewardsController : ControllerBase
    {



        private readonly IRewardsService _rewardsService;
        public RewardsController(IRewardsService RewardsService)
        {

            _rewardsService = RewardsService;
        }

        #region GET
        //get all agency active Roles
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [HttpGet("ActiveReward/AgencyId={agencyId}")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetActiveReward(string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var reward = _rewardsService.GetActiveReward(agencyId);
                return Ok(new OperationResponse<Reward>
                {

                    IsSuccess = true,
                    Message = "Reward retrieved successfully!",
                    Record = reward
                });
            }
            return Unauthorized();
        }
        //get all active Roles
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Reward>))]
        [HttpGet("AllRewards/AgencyId={agencyId}")]
        // [Authorize(Roles = "SysAdmin")]
        public IActionResult GetAllRewards(string agencyId)
        {


            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //  if (User.IsInRole("SysAdmin")) 
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin")
            {
                var rewards = _rewardsService.GetAllAgencyRewards(agencyId);
                return Ok(new CollectionResponse<Reward>
                {
                    Count = rewards.Count(),
                    IsSuccess = true,
                    Message = "Rewards retrieved successfully!",
                    Records = rewards
                });
            }
            else
                return Unauthorized();
        }

        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigureCustom")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> AddCustomReward([FromBody]CustomRewardRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var reward = await _rewardsService.AddCustomRewardAsync(model, userId);

                    return Ok(new OperationResponse<Reward>
                    {
                        IsSuccess = true,
                        Message = "Custo Reward has been configured successfully",
                        Record = reward
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = true,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigureCash")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> AddCashReward([FromBody]CashRewardRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var reward = await _rewardsService.AddCashRewardAsync(model, userId);

                    return Ok(new OperationResponse<Reward>
                    {
                        IsSuccess = true,
                        Message = "Cash Reward has been configured successfully",
                        Record = reward
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = true,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigurePoints")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]
        public async Task<IActionResult> AddPointsReward([FromBody]PointsRewardRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var reward = await _rewardsService.AddPointRewardAsync(model, userId);

                    return Ok(new OperationResponse<Reward>
                    {
                        IsSuccess = true,
                        Message = "Point Reward has been configured successfully",
                        Record = reward
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = true,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigureCoupon")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]
        public async Task<IActionResult> AddCouponReward([FromBody]CouponRewardRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var reward = await _rewardsService.AddCouponRewardAsync(model, userId);

                    return Ok(new OperationResponse<Reward>
                    {
                        IsSuccess = true,
                        Message = "Coupon Reward has been configured successfully",
                        Record = reward
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = true,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
        #endregion





        #region DELETE
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var r = User.FindFirst(ClaimTypes.Role).Value;
            if (r == "SysAdmin" || r == "AgencyAdmin")
            {

                var reward = await _rewardsService.DeleteRewardAsync(id, userId);
                if (reward == null)
                    return NotFound();

                return Ok(new OperationResponse<Reward>
                {
                    IsSuccess = true,
                    Message = "Reward deleted successfully!",
                    Record = reward
                });
            }
            return Unauthorized();
        }
        #endregion

    }
}