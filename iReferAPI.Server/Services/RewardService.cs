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

        
        Task<Reward> GetActiveReward(string agencyid);
        IEnumerable<Reward> GetAllAgencyRewards(string agencyid);
        Task<Reward> AddCouponRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId);
        Task<Reward> AddCustomRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, string message, bool noexpiration, string userId);
        Task<Reward> AddPointRewardAsync(string agencyId, int rewardreviewdays, decimal equivalentdollarAmount, int pointsamount, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId);
        Task<Reward> AddCashRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, decimal cashAmount, string message, bool noexpiration, string userId);
    }

    public class RewardsService : IRewardsService
    {

        private readonly ApplicationDbContext _db;

        public RewardsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Reward> AddCouponRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = RewardTypes.Coupon,
                UserId = userId,           
                Description = description,
                ExpirationDate = expirationDate,
                DiscountRate = discount,
                Message = message, 
                NoExpiration = noexpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddCustomRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate,  string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = RewardTypes.Custom,
                UserId = userId,
                Description = description,
                ExpirationDate = expirationDate,
                Message = message,
                NoExpiration = noexpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddCashRewardAsync(string agencyId, int rewardreviewdays, string description, DateTime expirationDate, decimal cashAmount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = RewardTypes.Custom,
                UserId = userId,
                Description = description,
                ExpirationDate = expirationDate,
                Message = message,
                NoExpiration = noexpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow,
                CashAmount=cashAmount
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddPointRewardAsync(string agencyId, int rewardreviewdays, decimal equivalentdollarAmount, int pointsamount, string description, DateTime expirationDate, float discount, string message, bool noexpiration, string userId)
        {
            var agency = await _db.Agencies.FindAsync(agencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = agencyId,
                RewardReviewDays = rewardreviewdays,
                RewardType = RewardTypes.Points,

                UserId = userId,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow,
                Description = description,
                ExpirationDate = expirationDate,
                Message = message,
                EquivalentDollarAmount = equivalentdollarAmount,
                PointsAmount = pointsamount,
                NoExpiration = noexpiration
                
        };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
            public IEnumerable<Reward> GetAllAgencyRewards(string agencyid)
            {
                var AgencyRewards = _db.Rewards.Where(i => i.AgencyId== agencyid).ToArray();

                return AgencyRewards;
            }
        public async Task<Reward> GetActiveReward(string agencyid)
        {
            var agency = await _db.Agencies.FindAsync(agencyid);
            if (agency == null)
                return null;
            var agencyreward =  _db.Rewards.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            return (Reward)agencyreward;
        }
        //public async Task<Reward> GetCouponRewardDetail(string agencyrewardid)
        //{
           
        //    var agencyReward = await _db.Rewards.FindAsync(agencyrewardid);
        //    if (agencyReward == null)
        //        return null;
        //    var Couponreward = _db.Rewards.Where(i => i.AgencyId == agencyrewardid);
        //    return (Reward)Couponreward;
        //}
        //public async Task<Reward> GetCustomRewardDetail(string agencyrewardid)
        //{

        //    var agencyReward = await _db.Rewards.FindAsync(agencyrewardid);
        //    if (agencyReward == null)
        //        return null;
        //    var customreward = _db.Rewards.Where(i => i.AgencyId == agencyrewardid);
        //    return (Reward)customreward;
        //}
        //public async Task<Reward> GetPointRewardDetail(string agencyrewardid)
        //{

        //    var agencyReward = await _db.Rewards.FindAsync(agencyrewardid);
        //    if (agencyReward == null)
        //        return null;
        //    var pointreward = _db.Rewards.Where(i => i.AgencyId == agencyrewardid);
        //    return (Reward)pointreward;
        //}
    }
    }


    