using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iReferAPI.Server.Services
{
    public interface IAccountsService
    {

        IEnumerable<Account> GetAgencyAccounts(string agencyid,int pageSize, int pageNumber, out int totalAccounts);

        IEnumerable<Account> GetAllAccounts(int pageSize, int pageNumber, out int totalAccounts);

        Task<Account> CreateAccountAsync(string agencyId, string referralUrl, string referralcode, string userId);

        Task<Account> EditAccountsAsync(string accountId, string referralUrl, string referralcode, string userId);

        Task<Account> GetAccountByIdAsync(string id);

        Task<Account> DeleteAccountAsync(string itemId, string userId);

    }

    public class AccountsService : IAccountsService
    {

        private readonly ApplicationDbContext _db;

        public AccountsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Account> CreateAccountAsync(string agencyId, string referralUrl, string referralcode, string userId)
        {
            // Check the plan if existing 
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;

            var item = new Account
            {
                ReferralURL = referralUrl,
                ReferralCode = referralcode,
                AgencyId = agencyId,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                Agency= agency
            };

            await _db.Accounts.AddAsync(item);
            await _db.SaveChangesAsync();

            return item;
        }

        public async Task<Account> DeleteAccountAsync(string accountId, string userId)
        {
            var item = await _db.Accounts.FindAsync(accountId);
            if (item == null || userId != item.UserId)
                return null;

            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;
            item.UserId = userId;
            await _db.SaveChangesAsync();

            return item;
        }

        public async Task<Account> EditAccountsAsync(string accountId, string referralUrl, string referralcode, string userId)
        {
            var item = await _db.Accounts.FindAsync(accountId);

            if (item == null || userId != item.UserId)
                return null;

            item.ReferralURL = referralUrl;
            item.ReferralCode = referralcode;
            item.UserId = userId;

            await _db.SaveChangesAsync();

            return item;
        }

        public IEnumerable<Account> GetAgencyAccounts(string agencyid, int pageSize, int pageNumber, out int totalAccounts)

        {

            var AllAccounts = _db.Accounts.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            totalAccounts = AllAccounts.Count();

            var Accounts = AllAccounts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            

            return Accounts;

          
        }
        public  async Task<Account> GetAccountByIdAsync(string id)
        {
            var Account = await _db.Accounts.FindAsync(id);
            if (Account.IsDeleted)
                return null;

            

            return Account;
        }

        public IEnumerable<Account> GetAllAccounts(int pageSize, int pageNumber, out int totalAccounts)

        {

            var AllAccounts = _db.Accounts.Where(i =>  !i.IsDeleted);

            totalAccounts = AllAccounts.Count();

            var Accounts = AllAccounts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();


            return Accounts;


        }


    }
}
