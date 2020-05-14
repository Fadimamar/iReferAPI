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
    public class AgencyInvitationsController : ControllerBase
    {



        private readonly IAgencyInvitationsService _agencyinvitationsService;
        public AgencyInvitationsController(IAgencyInvitationsService agencyinvitations)
        {

            _agencyinvitationsService = agencyinvitations;
        }

        #region GET
        //get Pending
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyInvitation>))]
        [HttpGet("NotSent/AgencyId={agencyId}")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetNotSent(string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var invitations = _agencyinvitationsService.GetNotSentInvitations(agencyId);
                return Ok(new CollectionResponse<AgencyInvitation>
                {

                    Count = invitations.Count(),
                    IsSuccess = true,
                    Message = "Invitations pending sending by email retrieved successfully!",
                    Records = invitations
                });
            }
            return Unauthorized();
        }
        //get not subscribed
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyInvitation>))]
        [HttpGet("NotSubscribed/AgencyId={agencyId}")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetNotSubscribed(string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var invitations = _agencyinvitationsService.GetNotSubscribedInvitations(agencyId);
                return Ok(new CollectionResponse<AgencyInvitation>
                {

                    Count = invitations.Count(),
                    IsSuccess = true,
                    Message = "Invitations pending subscribtion retrieved successfully!",
                    Records = invitations
                });
            }
            return Unauthorized();
        }
        //get not subscribed
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyInvitation>))]
        [HttpGet("NotViewed/AgencyId={agencyId}")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetNotNotViewed(string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var invitations = _agencyinvitationsService.GetNotViewedInvitations(agencyId);
                return Ok(new CollectionResponse<AgencyInvitation>
                {

                    Count = invitations.Count(),
                    IsSuccess = true,
                    Message = "Invitations pending viewing by customers retrieved successfully!",
                    Records = invitations
                });
            }
            return Unauthorized();
        }
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyInvitation>))]
        [HttpGet("AgencyId={agencyId}")]
        // [Authorize(Roles = "AgencyAdmin,SysAdmin")]

        public IActionResult GetAllInvitations(string agencyId)
        {
            if (agencyId == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var invitations = _agencyinvitationsService.GetAllAgencyInvitations(agencyId);
                return Ok(new CollectionResponse<AgencyInvitation>
                {

                    Count = invitations.Count(),
                    IsSuccess = true,
                    Message = "Agency Invitations retrieved successfully!",
                    Records = invitations
                });
            }
            return Unauthorized();
        }


        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<AgencyInvitation>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<AgencyInvitation>))]
        [HttpPost("Invite")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> InviteCustomer([FromBody]AgencyInvitationRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var invitation = await _agencyinvitationsService.InviteCustomer(model, userId);

                    return Ok(new OperationResponse<AgencyInvitation>
                    {
                        IsSuccess = true,
                        Message = "Invitation submitted successfully",
                        Record = invitation
                    });
                }

                return BadRequest(new OperationResponse<AgencyInvitation>
                {
                    IsSuccess = false,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
       
        
        #endregion





        #region DELETE
        [ProducesResponseType(200, Type = typeof(OperationResponse<AgencyInvitation>))]
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

                var invitation = await _agencyinvitationsService.DeleteInvitationAsync(id, userId);
                if (invitation == null)
                    return NotFound();

                return Ok(new OperationResponse<AgencyInvitation>
                {
                    IsSuccess = true,
                    Message = "invitation deleted successfully!",
                    Record = invitation
                });
            }
            return Unauthorized();
        }
        #endregion

    }
}