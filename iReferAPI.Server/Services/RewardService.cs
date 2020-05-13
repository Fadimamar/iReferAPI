
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

        
        Reward GetActiveReward(string agencyid);
        IEnumerable<Reward> GetAllAgencyRewards(string agencyid);
        Task<Reward> AddCouponRewardAsync(CouponRewardRequest model, string userId);
        Task<Reward> AddCustomRewardAsync(CustomRewardRequest model, string userId);
        Task<Reward> AddPointRewardAsync (PointsRewardRequest model, string userId);
        Task<Reward> AddCashRewardAsync(CashRewardRequest model,  string userId);
        Task<Reward> DeleteRewardAsync(string Id, string UserId);
    }

    public class RewardsService : IRewardsService
    {

        private readonly ApplicationDbContext _db;

        public RewardsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Reward> AddCouponRewardAsync(CouponRewardRequest model, string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = model.AgencyId,
                RewardReviewDays = model.RewardReviewDays,
                RewardType = RewardTypes.Coupon,
                UserId = userId,           
                Description =model.Description,
                ExpirationDate = model.ExpirationDate,
                DiscountRate = model.DiscountRate,
                Message = model.Message, 
                NoExpiration = model.NoExpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddCustomRewardAsync(CustomRewardRequest model, string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = model.AgencyId,
                RewardReviewDays = model.RewardReviewDays,
                RewardType = RewardTypes.Custom,
                UserId = userId,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,
                Message = model.Message,
                NoExpiration = model.NoExpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddCashRewardAsync(CashRewardRequest model,  string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = model.AgencyId,
                RewardReviewDays = model.RewardReviewDays,
                RewardType = RewardTypes.Custom,
                UserId = userId,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,
                Message = model.Message,
                NoExpiration = model.NoExpiration,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow,
                CashAmount=model.CashAmount
            };
            await _db.Rewards.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Reward> AddPointRewardAsync(PointsRewardRequest model,  string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;
            var item = new Reward
            {
                AgencyId = model.AgencyId,
                RewardReviewDays = model.RewardReviewDays,
                RewardType = RewardTypes.Points,

                UserId = userId,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,
                Message = model.Message,
                EquivalentDollarAmount = model.EquivalentDollarAmount,
                PointsAmount = model.PointsAmount,
                NoExpiration = model.NoExpiration
                
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
        public  Reward GetActiveReward(string agencyid)
        {
            
            var agencyreward =  _db.Rewards.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            return (Reward)agencyreward;
        }

        public async Task<Reward> DeleteRewardAsync(string Id, string userId)
        {

            
                var item = await _db.Rewards.FindAsync(Id);
                if (item == null || userId != item.UserId)
                    return null;

                item.IsDeleted = true;
                item.ModifiedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return item;
            
        }
        
        
    
    }


    