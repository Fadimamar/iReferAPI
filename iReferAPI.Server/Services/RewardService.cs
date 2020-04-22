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

        Task<CouponReward> GetCouponRewardDetail(string agencyrewardid, string userId);
        Task<CustomReward> GetCustomRewardDetail(string agencyrewardid, string userId);
        Task<PointReward> GetPointRewardDetail(string agencyrewardid, string userId);
        Task<AgencyReward> GetActiveReward(string agencyid, string userId);
        IEnumerable<AgencyReward> GetAllAgencyRewards(string agencyid, string userId);
        Task<CouponReward> AddCouponRewardAsync(string agencyId, string Description, DateTime ExpirationDate, float discount, string Message, string userId);
        Task<CustomReward> AddCustomRewardAsync(string agencyId, string Description, DateTime ExpirationDate, float discount, string Message, string userId);
        Task<PointReward> AddPointRewardAsync(string agencyId, string Description, DateTime ExpirationDate, float discount, string Message, string userId);

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
                NoExpiration = noexpiration
            };
            await _db.CouponRewards.AddAsync(citem);
            await _db.SaveChangesAsync();
            return citem;
        }
        public async Task<CustomReward> AddCustomRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var item = new AgencyReward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = "Custom",

                UserId = userId,

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
                NoExpiration = noexpiration
            };
            await _db.CustomRewards.AddAsync(citem);
            await _db.SaveChangesAsync();
            return citem;
        }
        public async Task<PointReward> AddPointRewardAsync(string agencyId, int rewardreviewdays, decimal equivalentdollarAmount, int pointsamount, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var item = new AgencyReward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = "Point",

                UserId = userId,

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
                NoExpiration = noexpiration
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
        public Task<AgencyReward> GetActiveReward(string agencyid, string userId)
        {
            var agencyreward = _db.AgencyRewards.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            return agencyreward;
        }
    }
    }
}

    