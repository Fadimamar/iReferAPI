using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iReferAPI.Server.Services
{
    public interface IRewardsService
    {

        Task<CouponReward> GetCouponRewardDetail(string agencyrewardID);
        Task<CustomReward> GetCustomRewardDetail(string agencyrewardID);
        Task<PointReward> GetPointRewardDetail(string agencyrewardID);
        Task<AgencyReward> GetActiveReward(string agencyid, string userId);
        IEnumerable<AgencyReward> GetAllAgencyRewards(string agencyid, string userId);
        Task<CouponReward> AddCouponRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId);
        Task<CustomReward> AddCustomRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId);
        Task<PointReward> AddPointRewardAsync(string agencyId, int rewardreviewdays, decimal equivalentdollarAmount, int pointsamount, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId);

    }

    public class RewardsService : IRewardsService
    {

        private readonly ApplicationDbContext _db;

        public RewardsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CouponReward> AddCouponRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new AgencyReward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = "Coupon",

                UserId = userId,

            };
            await _db.AgencyRewards.AddAsync(item);
            await _db.SaveChangesAsync();
            var citem = new CouponReward
            {
                AgencyRewardId = item.Id,
                Description = description,
                ExpirationDate = expirationDate,
                DiscountRate = discount,
                Message = message,
                UserId = userId,
                NoExpiration = noexpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.CouponRewards.AddAsync(citem);
            await _db.SaveChangesAsync();
            return citem;
        }
        public async Task<CustomReward> AddCustomRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new AgencyReward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = "Custom",

                UserId = userId,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow

            };
            await _db.AgencyRewards.AddAsync(item);
            await _db.SaveChangesAsync();
            var citem = new CustomReward
            {
                AgencyRewardId = item.Id,
                Description = description,
                ExpirationDate = expirationDate, 
                Message = message,
                UserId = userId,
                NoExpiration = noexpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.CustomRewards.AddAsync(citem);
            await _db.SaveChangesAsync();
            return citem;
        }
        public async Task<PointReward> AddPointRewardAsync(string agencyId, int rewardreviewdays, decimal equivalentdollarAmount, int pointsamount, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new AgencyReward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = "Point",

                UserId = userId,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow

            };
            await _db.AgencyRewards.AddAsync(item);
            await _db.SaveChangesAsync();
            var citem = new PointReward
            {
                AgencyRewardId = item.Id,
                Description = description,
                ExpirationDate = expirationDate,
                Message = message,
                UserId = userId,
                EquivalentDollarAmount = equivalentdollarAmount,
                PointsAmount = pointsamount,
                NoExpiration = noexpiration,
                IsDeleted = false,
            ModifiedDate = DateTime.UtcNow
        };
            await _db.PointRewards.AddAsync(citem);
            await _db.SaveChangesAsync();
            return citem;
        }
            public IEnumerable<AgencyReward> GetAllAgencyRewards(string agencyid, string userId)
            {
                var AgencyRewards = _db.AgencyRewards.Where(i => i.AgencyId== agencyid).ToArray();

                return AgencyRewards;
            }
        public async Task<AgencyReward> GetActiveReward(string agencyid, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyid);
            if (agency == null)
                return null;
            var agencyreward =  _db.AgencyRewards.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            return (AgencyReward)agencyreward;
        }
        public async Task<CouponReward> GetCouponRewardDetail(string agencyrewardid)
        {
           
            var agencyReward = await _db.AgencyRewards.FindAsync(agencyrewardid);
            if (agencyReward == null)
                return null;
            var Couponreward = _db.CouponRewards.Where(i => i.AgencyRewardId == agencyrewardid);
            return (CouponReward)Couponreward;
        }
        public async Task<CustomReward> GetCustomRewardDetail(string agencyrewardid)
        {

            var agencyReward = await _db.AgencyRewards.FindAsync(agencyrewardid);
            if (agencyReward == null)
                return null;
            var customreward = _db.CustomRewards.Where(i => i.AgencyRewardId == agencyrewardid);
            return (CustomReward)customreward;
        }
        public async Task<PointReward> GetPointRewardDetail(string agencyrewardid)
        {

            var agencyReward = await _db.AgencyRewards.FindAsync(agencyrewardid);
            if (agencyReward == null)
                return null;
            var pointreward = _db.CustomRewards.Where(i => i.AgencyRewardId == agencyrewardid);
            return (PointReward)pointreward;
        }
    }
    }
}

    