using Microsoft.EntityFrameworkCore;
using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iReferAPI.Server.Services
{
    public interface IAgenciesService
    {

        IEnumerable<Agency> GetAllAgenciesAsync(int pageSize, int pageNumber, string userId, out int totalAgencies);
        IEnumerable<Agency> SearchAgenciesAsync(string query, int pageSize, int pageNumber, string userId, out int totalAgencies);
        Task<Agency> AddAgencyAsync(string agencyname, string address1, string address2, string
            website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId);
        Task<Agency> EditAgencyAsync(string id, string agencyname, string address1, string address2, string
            website, string phonenumber, string state, string zipcode, string city, string phoneno, String logo, string userId);
        Task<Agency> DeleteAgencyAsync(string id, string userId);
        Task<Agency> GetAgencyById(string id, string userId);
        Agency GetAgencyByName(string name, string userId);
    }

    public class AgenciesService : IAgenciesService
    {

        private readonly ApplicationDbContext _db;
        public AgenciesService(ApplicationDbContext db)
        {
            _db = db;
        }

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
                UserId = userId
            };

            await _db.Agencies.AddAsync(Agency);
            await _db.SaveChangesAsync();

            return Agency;
        }

        public async Task<Agency> DeleteAgencyAsync(string id, string userId)
        {
            var Agency = await _db.Agencies.FindAsync(id);
            if (Agency.UserId != userId || Agency.IsDeleted)
                return null;

            Agency.IsDeleted = true;
            Agency.ModifiedDate = DateTime.UtcNow;

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

        public IEnumerable<Agency> GetAllAgenciesAsync(int pageSize, int pageNumber, string userId, out int totalAgencies)
        {
            // total Agencys 
            var AllAgencies = _db.Agencies.Where(p => !p.IsDeleted && p.UserId == userId);

            totalAgencies = AllAgencies.Count();

            var Agencys = AllAgencies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            foreach (var item in Agencys)
            {
                item.Accounts = _db.Accounts.Where(i => !i.IsDeleted && i.AgencyId == item.Id).ToArray();
            }

            return Agencys;
        }

        public async Task<Agency> GetAgencyById(string id, string userId)
        {
            var Agency = await _db.Agencies.FindAsync(id);
            if (Agency.UserId != userId || Agency.IsDeleted)
                return null;

            Agency.Accounts = _db.Accounts.Where(i => !i.IsDeleted && i.UserId == userId && i.AgencyId == id).ToArray();

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
            foreach (var item in Agencys)
            {
                item.Accounts = _db.Accounts.Where(i => !i.IsDeleted && i.AgencyId == item.Id).ToArray();
            }

            return Agencys;
        }


    }
}
