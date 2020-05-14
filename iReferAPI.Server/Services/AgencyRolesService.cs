using Microsoft.EntityFrameworkCore;
using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using iReferAPI.Server.Models;
namespace iReferAPI.Server.Services
{
    public interface IAgenciesRolesService
    {

        IEnumerable<AgencyRole> GetAgencyRoles(string agencyid, string userId);

        IEnumerable<AgencyRole> GetAgencyRoles( string userId);

        Task<AgencyRole> AddAgencyRoleAsync(AgencyRoleRequest model, string userId);



        Task<AgencyRole> DeleteAgencyRoleAsync(string AgencyRoleID, string userId);

    }

    public class AgenciesRolesService : IAgenciesRolesService
    {

        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManger;

        public AgenciesRolesService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManger = userManager;
        }

        public async Task<AgencyRole> AddAgencyRoleAsync(AgencyRoleRequest model, string userId)
        {
            
                

            if (model == null)
                throw new NullReferenceException("Model is null");
           
            

            String Role;
            switch (model.UserType)
            {
                case UserTypes.AgencyAdmin:
                    Role = "AgencyAdmin";
                    break;
                case UserTypes.AgencyUser:
                    Role = "AgencyUser";
                    break;
                default:
                    Role = "AgencyUser";
                    break;
            }



            var agency = await _db.Agencies.FindAsync(model.AgencyID);
            if (agency != null)
            {
                var employee = await _userManger.FindByIdAsync(model.EmployeeUserID);
                if (employee != null)
                {
                    
                    var result = await _userManger.AddToRoleAsync(employee, Role);
                    var item = new AgencyRole { FirstName = employee.FirstName, LastName = employee.LastName, email = employee.Email, AgencyId = model.AgencyID, UserRoleID = (int)model.UserType, EmployeeUserID = employee.Id, UserId = userId,Agency= agency };

                    var res = await _db.AgencyRoles.AddAsync(item);
                    await _db.SaveChangesAsync();
                    return item;




                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }





        }

        public async Task<AgencyRole> DeleteAgencyRoleAsync(string AgencyRoleID, string userId)
        {
            var item = await _db.AgencyRoles.FindAsync(AgencyRoleID);

            if (item == null || userId != item.UserId)
                return null;

            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;
            

            await _db.SaveChangesAsync();

            
            
            var employee = await _userManger.FindByIdAsync(item.EmployeeUserID);
            if (employee != null)
            {
                String Role = "";
                switch (item.UserRoleID)
                {
                    case (int)UserTypes.AgencyAdmin:
                        Role = "AgencyAdmin";
                        break;
                    case (int)UserTypes.AgencyUser:
                        Role = "AgencyUser";
                        break;


                }
                var result = await _userManger.RemoveFromRoleAsync(employee, Role);
                if (result.Succeeded)
                  return  item;
            }

            return null;
        }

      

        public IEnumerable<AgencyRole> GetAgencyRoles(string agencyid, string userId)
        {
            var agencyRoles = _db.AgencyRoles.Where(i => i.AgencyId == agencyid && !i.IsDeleted).ToArray();

            return agencyRoles;
        }

        public IEnumerable<AgencyRole> GetAgencyRoles( string userId)
        {
            var agencyRoles = _db.AgencyRoles.Where(i =>  !i.IsDeleted).ToArray();

            return agencyRoles;
        }


    }
}
