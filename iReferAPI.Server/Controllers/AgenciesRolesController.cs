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
    public class AgenciesRolesController : ControllerBase
    {

        

        private readonly IAgenciesRolesService _agenciesRolesService;
        public AgenciesRolesController(IAgenciesRolesService AgencyRolesService)
        {
            
            _agenciesRolesService = AgencyRolesService;
        }

        #region GET
        //get all agency active Roles
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyRole>))]
        [HttpGet("agencyid={agencyId}")]
      // [Authorize(Roles = "AgencyAdmin,SysAdmin")]
        
        public IActionResult Get(string agencyid)
        {
            if (agencyid == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "SysAdmin" || role == "AgencyAdmin")
            {
                var Roles = _agenciesRolesService.GetAgencyRoles(agencyid, userId);
                return Ok(new CollectionResponse<AgencyRole>
                {
                    Count = Roles.Count(),
                    IsSuccess = true,
                    Message = "Roles retrieved successfully!",
                    Records = Roles
                });
            }
            return Unauthorized();
        }
        //get all active Roles
        [ProducesResponseType(200, Type = typeof(CollectionResponse<AgencyRole>))]
        [HttpGet("AllAgencies")]
       // [Authorize(Roles = "SysAdmin")]
        public IActionResult Get()
        {
           

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //  if (User.IsInRole("SysAdmin")) 
            var role = User.FindFirst(ClaimTypes.Role).Value;
               if (role=="SysAdmin" )
            {
                var Roles = _agenciesRolesService.GetAgencyRoles(userId);
                return Ok(new CollectionResponse<AgencyRole>
                {
                    Count = Roles.Count(),
                    IsSuccess = true,
                    Message = "Roles retrieved successfully!",
                    Records = Roles
                });
            }
            else
                return Unauthorized();
        }

        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<AgencyRole>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<AgencyRole>))]
        [HttpPost("AddRoleByID")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> Post([FromBody]AgencyRoleRequestByID model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var role = await _agenciesRolesService.AddAgencyRoleByIDAsync(model, userId);

                    return Ok(new OperationResponse<AgencyRole>
                    {
                        IsSuccess = true,
                        Message = "Agency Role has been created successfully",
                        Record = role
                    });
                }

                return BadRequest(new OperationResponse<AgencyRole>
                {
                    IsSuccess = false,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }
        [ProducesResponseType(200, Type = typeof(OperationResponse<AgencyRole>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<AgencyRole>))]
        [HttpPost("AddRoleByEmail")]
        // [Authorize(Roles = "AgencyAdmin, SysAdmin")]

        public async Task<IActionResult> Post([FromBody]AgencyRoleRequestByEmail model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var r = User.FindFirst(ClaimTypes.Role).Value;
                if (r == "SysAdmin" || r == "AgencyAdmin")
                {

                    var role = await _agenciesRolesService.AddAgencyRoleByEmailAsync(model, userId);

                    return Ok(new OperationResponse<AgencyRole>
                    {
                        IsSuccess = true,
                        Message = "Agency Role has been created successfully",
                        Record = role
                    });
                }

                return BadRequest(new OperationResponse<AgencyRole>
                {
                    IsSuccess = false,
                    Message = "Some properties are not valid"
                });
            }
            return Unauthorized();
        }

        #endregion





        #region DELETE
        [ProducesResponseType(200, Type = typeof(OperationResponse<AgencyRole>))]
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

                var role = await _agenciesRolesService.DeleteAgencyRoleAsync(id, userId);
                if (role == null)
                    return NotFound();

                return Ok(new OperationResponse<AgencyRole>
                {
                    IsSuccess = true,
                    Message = "Agency Role deleted successfully!",
                    Record = role
                });
            }
            return Unauthorized();
        }
        #endregion

    }
}