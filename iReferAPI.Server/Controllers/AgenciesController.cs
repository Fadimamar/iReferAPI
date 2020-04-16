using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using iReferAPI.Models;
using iReferAPI.Server.Services;

namespace iReferAPI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgenciesController : ControllerBase
    {

        private readonly IAgenciesService _AgenciesService;
        private readonly IConfiguration _configuration;

        private const int PAGE_SIZE = 10;
        public AgenciesController(IAgenciesService AgenciesService, IConfiguration configuration)
        {
            _AgenciesService = AgenciesService;
            _configuration = configuration;
        }

        private readonly List<string> allowedExtensions = new List<string>
        {
            ".jpg", ".bmp", ".png"
        };

        #region Get
        [ProducesResponseType(200, Type = typeof(CollectionPagingResponse<Agency>))]
        [HttpGet]
        public IActionResult Get(int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int totalAgencies = 0;
            if (page == 0)
                page = 1;
            var Agencies = _AgenciesService.GetAllAgenciesAsync(PAGE_SIZE, page, userId, out totalAgencies);

            int totalPages = 0;
            if (totalAgencies % PAGE_SIZE == 0)
                totalPages = totalAgencies / PAGE_SIZE;
            else
                totalPages = (totalAgencies / PAGE_SIZE) + 1;

            return Ok(new CollectionPagingResponse<Agency>
            {
                Count = totalAgencies,
                IsSuccess = true,
                Message = "Agencies received successfully!",
                OperationDate = DateTime.UtcNow,
                PageSize = PAGE_SIZE,
                Page = page,
                Records = Agencies
            });
        }

        [ProducesResponseType(200, Type = typeof(CollectionPagingResponse<Agency>))]
        [HttpGet("query={query}")]
        public IActionResult Get(string query, int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int totalAgencies = 0;
            if (page == 0)
                page = 1;
            var Agencies = _AgenciesService.SearchAgenciesAsync(query, PAGE_SIZE, page, userId, out totalAgencies);

            int totalPages = 0;
            if (totalAgencies % PAGE_SIZE == 0)
                totalPages = totalAgencies / PAGE_SIZE;
            else
                totalPages = (totalAgencies / PAGE_SIZE) + 1;

            return Ok(new CollectionPagingResponse<Agency>
            {
                Count = totalAgencies,
                IsSuccess = true,
                Message = $"Agencies of '{query}' received successfully!",
                OperationDate = DateTime.UtcNow,
                PageSize = PAGE_SIZE,
                Page = page,
                Records = Agencies
            });
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<Agency>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Agency>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var Agency = await _AgenciesService.GetAgencyById(id, userId);
            if (Agency == null)
                return BadRequest(new OperationResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid operation",
                });

            return Ok(new OperationResponse<Agency>
            {
                Record = Agency,
                Message = "Agency retrieved successfully!",
                IsSuccess = true,
                OperationDate = DateTime.UtcNow
            });
        }

        #endregion

        #region Post 
        [ProducesResponseType(200, Type = typeof(OperationResponse<Agency>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Agency>))]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AgencyRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string url = $"{_configuration["AppUrl"]}Images/default.jpg";
            string fullPath = null;
            // Check the file 
            if (model.Logo != null)
            {
                string extension = Path.GetExtension(model.Logo.FileName);

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new OperationResponse<Agency>
                    {
                        Message = "Agency image is not a valid image file",
                        IsSuccess = false,
                    });

                if (model.Logo.Length > 500000)
                    return BadRequest(new OperationResponse<Agency>
                    {
                        Message = "Image file cannot be more than 5mb",
                        IsSuccess = false,
                    });

                string newFileName = $"Images/{Guid.NewGuid()}{extension}";
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newFileName);
                url = $"{_configuration["AppUrl"]}{newFileName}";
            }

            var addedAgency = await _AgenciesService.AddAgencyAsync(model.AgencyName, model.Address1, model.Address2, model.Website, model.PhoneNo, model.State, model.ZipCode, model.City, model.PhoneNo, url, userId);

            if (addedAgency != null)
            {
                if (fullPath != null)
                {
                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.Logo.CopyToAsync(fs);
                    }
                }

                return Ok(new OperationResponse<Agency>
                {
                    IsSuccess = true,
                    Message = $"{addedAgency.AgencyName} has been added successfully!",
                    Record = addedAgency
                });

            }

            return BadRequest(new OperationResponse<Agency>
            {
                Message = "Something went wrong",
                IsSuccess = false
            });

        }
        #endregion

        #region Put 
        [ProducesResponseType(200, Type = typeof(OperationResponse<Agency>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Agency>))]
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]AgencyRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string url = $"{_configuration["AppUrl"]}Images/default.jpg";
            string fullPath = null;
            // Check the file 
            if (model.Logo != null)
            {
                string extension = Path.GetExtension(model.Logo.FileName);

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new OperationResponse<Agency>
                    {
                        Message = "Agency image is not a valid image file",
                        IsSuccess = false,
                    });

                if (model.Logo.Length > 500000)
                    return BadRequest(new OperationResponse<Agency>
                    {
                        Message = "Image file cannot be more than 5mb",
                        IsSuccess = false,
                    });

                string newFileName = $"Images/{Guid.NewGuid()}{extension}";
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newFileName);
                url = $"{_configuration["AppUrl"]}{newFileName}";
            }
            var oldAgency = await _AgenciesService.GetAgencyById(model.Id, userId);
            if (fullPath == null)
                url = oldAgency.Logo;

            var editedAgency = await _AgenciesService.EditAgencyAsync(model.Id, model.AgencyName, model.Address1, model.Address2, model.Website, model.PhoneNo, model.State, model.ZipCode, model.City, model.PhoneNo, url, userId);

            if (editedAgency != null)
            {
                if (fullPath != null)
                {
                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.Logo.CopyToAsync(fs);
                    }
                }

                return Ok(new OperationResponse<Agency>
                {
                    IsSuccess = true,
                    Message = $"{editedAgency.AgencyName} has been edited successfully!",
                    Record = editedAgency
                });
            }


            return BadRequest(new OperationResponse<Agency>
            {
                Message = "Something went wrong",
                IsSuccess = false
            });

        }
        #endregion

        #region Delete
        [ProducesResponseType(200, Type = typeof(OperationResponse<Agency>))]
        [ProducesResponseType(404)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var getOld = await _AgenciesService.GetAgencyById(id, userId);
            if (getOld == null)
                return NotFound();

            // Remove the file 
            //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", getOld.CoverPath.Replace(_configuration["AppUrl"], ""));
            //System.IO.File.Delete(fullPath);

            var deletedAgency = await _AgenciesService.DeleteAgencyAsync(id, userId);

            return Ok(new OperationResponse<Agency>
            {
                IsSuccess = true,
                Message = $"{getOld.AgencyName} has been deleted successfully!",
                Record = deletedAgency
            });
        }
        #endregion 


    }
}