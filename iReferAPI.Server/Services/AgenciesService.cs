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
    public interface IAgenciesService
    {
     
        IEnumerable<Agency> GetAllAgenciesPagedAsync(int pageSize, int pageNumber, string userId, out int totalAgencies);
        IEnumerable<Agency> GetAllAgenciesPagedAsync(int pageSize, int pageNumber,  out int totalAgencies);
        IEnumerable<Agency> GetAllAgenciesAsync(string userId);
        IEnumerable<Agency> GetAllAgenciesAsync();
        IEnumerable<Agency> SearchAgenciesAsync(string query, int pageSize, int pageNumber, string userId, out int totalAgencies);
        Task<Agency> AddAgencyAsync(string agencyname, string address1, string address2, string
            website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId);
        Task<Agency> EditAgencyAsync(string id, string agencyname, string address1, string address2, string
            website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId);
        Task<Agency> DeleteAgencyAsync(string id, string userId);
        Task<Agency> GetAgencyById(string id, string userId);
        Agency GetAgencyByName(string name, string userId);
        //Task<UserManagerResponse> AddEmployeetoRole(AgencyRoleRequest model, string userId);
    }

    public class AgenciesService : IAgenciesService
    {

        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManger;
        public AgenciesService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManger = userManager;
        }
        //public async Task<UserManagerResponse> AddEmployeetoRole(AgencyRoleRequest model, string userId)

        //{
        //    if (model == null)
        //        throw new NullReferenceException("Reigster Model is null");


        //    String Role;
        //    switch (model.UserType)
        //    {
        //        case UserTypes.AgencyAdmin:
        //            Role = "AgencyAdmin";
        //            break;
        //        default:
        //            Role = "AgencyUser";
        //            break;
        //    }



        //    var agency = await _db.Agencies.FindAsync(model.AgencyID);
        //    if (agency != null)
        //    {
        //        var employee = await _userManger.FindByIdAsync(model.EmployeeUserID);
        //        if (employee != null)
        //        {

        //            var result = await _userManger.AddToRoleAsync(employee, Role);
        //            var item = new AgencyRole { FirstName=model.FirstName, LastName=model.LastName, email=model.email, AgencyId = model.AgencyID, UserRoleID = (int)model.UserType, EmployeeUserID = employee.Id, UserId= userId };

        //            var res = await _db.AgencyRoles.AddAsync(item);
        //            await _db.SaveChangesAsync();
        //            return new UserManagerResponse
        //            {
        //                Message = "Role Added successfully for the supplied userid under the supplied Agency ID!",
        //                IsSuccess = true,
        //            };




        //        }
        //        else
        //        {
        //            return new UserManagerResponse
        //            {
        //                Message = "User Does not exists!",
        //                IsSuccess = false,
        //            };
        //        }
        //    }
        //    else
        //    {
        //        return new UserManagerResponse
        //        {
        //            Message = "Agency Does not exists",
        //            IsSuccess = false,
        //        };
        //    }





    //}
    public async Task<Agency> AddAgencyAsync(string agencyname, string address1, string address2, string
        website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId)
    {
        var Agency = new Agency
        {
            AgencyName = agencyname,
            Website = website,
            ZipCode = zipcode,
            City = city,
            State = state,
            Address2 = address2,
            Address1 = address1,
            PhoneNo = phoneno,
            UserId = userId,
           
        };

        await _db.Agencies.AddAsync(Agency);
        await _db.SaveChangesAsync();
        var employee = await _userManger.FindByIdAsync(userId);
        if (employee != null)
        {
            await _userManger.AddToRoleAsync(employee, "AgencyAdmin");
            var item = new AgencyRole { AgencyId = Agency.Id, UserRoleID = (int)UserTypes.AgencyAdmin, EmployeeUserID = userId, CreatedDate=DateTime.UtcNow };

            var res = await _db.AgencyRoles.AddAsync(item);
            await _db.SaveChangesAsync();
        }
        return Agency;
    }

    public async Task<Agency> DeleteAgencyAsync(string id, string userId)
    {
        var Agency = await _db.Agencies.FindAsync(id);
        if (Agency.UserId != userId || Agency.IsDeleted)
            return null;

        Agency.IsDeleted = true;
        Agency.ModifiedDate = DateTime.UtcNow;
            Agency.UserId = userId;
        await _db.SaveChangesAsync();
        return Agency;
    }

    public async Task<Agency> EditAgencyAsync(string id, string agencyname, string address1, string address2, string
        website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId)
    {
        var Agency = await _db.Agencies.FindAsync(id);
        if (Agency.UserId != userId || Agency.IsDeleted)
            return null;

        Agency.AgencyName = agencyname;
        Agency.Website = website;
        Agency.State = state;
        Agency.Address1 = address1;
        Agency.Address2 = address2;
        Agency.State = state;
        Agency.City = city;
        Agency.ZipCode = zipcode;
        Agency.PhoneNo = phoneno;
        Agency.ModifiedDate = DateTime.Now;
        Agency.Logo = logo;

        // await _db.SaveChangesAsync();
        return Agency;
    }

    public IEnumerable<Agency> GetAllAgenciesPagedAsync(int pageSize, int pageNumber, string userId, out int totalAgencies)
    {
        // total Agencys 



        var AllAgencies = _db.Agencies.Where(p => !p.IsDeleted && p.UserId == userId);

        totalAgencies = AllAgencies.Count();

        var Agencys = AllAgencies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
        //foreach (var item in Agencys)
        //{
        //    item.AgencyRoles = _db.AgencyRoles.Where(i => !i.IsDeleted && i.AgencyId == item.Id).ToArray();
        //}

        return Agencys;
    }
        public IEnumerable<Agency> GetAllAgenciesAsync(string userId)
        {

            var AllAgencies = _db.Agencies.Where(p => !p.IsDeleted && p.UserId == userId);

           
                

            return AllAgencies;
        }

        public async Task<Agency> GetAgencyById(string id, string userId)
        {
            var Agency = await _db.Agencies.FindAsync(id);
            if (Agency.UserId != userId || Agency.IsDeleted)
                return null;

            //Agency.AgencyRoles = _db.AgencyRoles.Where(i => !i.IsDeleted && i.UserId == userId && i.AgencyId == id).ToArray();

            return Agency;
        }

        public Agency GetAgencyByName(string name, string userId)
        {
                     
            
            var Agency = _db.Agencies.SingleOrDefault(p => p.AgencyName == name && p.UserId == userId);
            if (Agency.UserId != userId || Agency.IsDeleted)
                return null;

            //Agency.ToDoItems = _db.ToDoItems.Where(i => !i.IsDeleted && i.UserId == userId && i.AgencyId == id).ToArray();
            return Agency;
        }

        public IEnumerable<Agency> SearchAgenciesAsync(string query, int pageSize, int pageNumber, string userId, out int totalAgencies)
        {
            // total Agencys 
            var allAgencys = _db.Agencies.Where(p => !p.IsDeleted && p.UserId == userId && (p.PhoneNo.Contains(query) || p.AgencyName.Contains(query) || p.Website.Contains(query)));

            totalAgencies = allAgencys.Count();

            var Agencys = allAgencys.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            //foreach (var item in Agencys)
            //{
            //    item.AgencyRoles = _db.AgencyRoles.Where(i => !i.IsDeleted && i.AgencyId == item.Id).ToArray();
            //}

            return Agencys;
        }

        public IEnumerable<Agency> GetAllAgenciesPagedAsync(int pageSize, int pageNumber, out int totalAgencies)
        {

            var AllAgencies = _db.Agencies.Where(p => !p.IsDeleted);

            totalAgencies = AllAgencies.Count();

            var Agencys = AllAgencies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            //foreach (var item in Agencys)
            //{
            //    item.AgencyRoles = _db.AgencyRoles.Where(i => !i.IsDeleted && i.AgencyId == item.Id).ToArray();
            //}

            return Agencys;
        }
        public IEnumerable<Agency> GetAllAgenciesAsync()
        {

            var AllAgencies = _db.Agencies.Where(p => !p.IsDeleted);

            return AllAgencies;
        }
    }
}
