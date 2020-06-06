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
    public class FriendsOfferController : ControllerBase
    {



        private readonly IOffersService _offersService;
        public FriendsOfferController(IOffersService offersService)
        {

            _offersService = offersService;
        }

        #region GET
        //get all agency active Roles
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [HttpGet("activeoffer")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetActiveOffer([FromQuery]string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var offer = _offersService.GetActiveOffer(agencyId);
                return Ok(new OperationResponse<FriendOffer>
                {

                    IsSuccess = true,
                    Message = "Offer retrieved successfully!",
                    Record = offer
                });
            }
            return Unauthorized();
        }
        //get all active Roles
        [ProducesResponseType(200, Type = typeof(CollectionResponse<Reward>))]
        [HttpGet("alloffers")]
        // [Authorize(Roles = "SysAdmin")]
        public IActionResult GetAllOffers([FromQuery]string agencyId)
        {


            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //  if (User.IsInRole("SysAdmin")) 
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin")
            {
                var offers = _offersService.GetAllAgencyOffers(agencyId);
                return Ok(new CollectionResponse<FriendOffer>
                {
                    Count = offers.Count(),
                    IsSuccess = true,
                    Message = "Rewards retrieved successfully!",
                    Records = offers
                });
            }
            else
                return Unauthorized();
        }

        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigureLink")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> AddLinkOffer([FromBody]OnlineOfferRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var offer = await _offersService.AddOnlineOfferAsync(model, userId);

                    return Ok(new OperationResponse<FriendOffer>
                    {
                        IsSuccess = true,
                        Message = "Link Offer has been configured successfully",
                        Record = offer
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = false,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
       
        [ProducesResponseType(200, Type = typeof(OperationResponse<Reward>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Reward>))]
        [HttpPost("ConfigureCoupon")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]
        public async Task<IActionResult> AddCouponOffer([FromBody]CouponOfferRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var offer = await _offersService.AddCouponOfferAsync(model, userId);

                    return Ok(new OperationResponse<FriendOffer>
                    {
                        IsSuccess = true,
                        Message = "Coupon Offer has been configured successfully",
                        Record = offer
                    });
                }

                return BadRequest(new OperationResponse<Reward>
                {
                    IsSuccess = false,
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

                var offer = await _offersService.DeleteofferAsync(id, userId);
                if (offer == null)
                    return NotFound();

                return Ok(new OperationResponse<FriendOffer>
                {
                    IsSuccess = true,
                    Message = "Offer deleted successfully!",
                    Record = offer
                });
            }
            return Unauthorized();
        }
        #endregion

    }
}