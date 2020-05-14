
using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iReferAPI.Server.Services
{
    public interface IOffersService
    {


        FriendOffer GetActiveOffer(string agencyid);
        IEnumerable<FriendOffer> GetAllAgencyOffers(string agencyid);
        Task<FriendOffer> AddCouponOfferAsync(CouponOfferRequest model, string userId);
        
        Task<FriendOffer> AddOnlineOfferAsync(OnlineOfferRequest model, string userId);
        Task<FriendOffer> DeleteofferAsync(string Id, string UserId);
    }

    public class FriendOffersService : IOffersService
    {

        private readonly ApplicationDbContext _db;

        public FriendOffersService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<FriendOffer> AddCouponOfferAsync(CouponOfferRequest model, string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;
            var AgencyOffers = GetAllAgencyOffers(model.AgencyId);

            foreach (FriendOffer itm in AgencyOffers)
            {
                itm.IsDeleted = true;
                itm.ModifiedDate = DateTime.UtcNow;
                itm.UserId = userId;
                            }
            await _db.SaveChangesAsync();
            var item = new FriendOffer
            {
                AgencyId = model.AgencyId,
                OfferType = OfferTypes.Coupon,
                UserId = userId,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,
                DiscountRate = model.DiscountRate,
                Message = model.Message,
                NoExpiration = model.NoExpiration,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                Agency=agency
            };
            await _db.FriendOffers.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<FriendOffer> AddOnlineOfferAsync(OnlineOfferRequest model, string userId)
        {
            var agency = await _db.Agencies.FindAsync(model.AgencyId);
            if (agency == null)
                return null;

            var AgencyOffers = GetAllAgencyOffers(model.AgencyId);

            foreach (FriendOffer itm in AgencyOffers)
            {
                itm.IsDeleted = true;
                itm.ModifiedDate = DateTime.UtcNow;
                itm.UserId = userId;
               
            }
            await _db.SaveChangesAsync();
            var item = new FriendOffer
            {
                AgencyId = model.AgencyId,
                OfferType = OfferTypes.Link,
                UserId = userId,
                LandingPage = model.LandingPage,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,
                Message = model.Message,
                NoExpiration = model.NoExpiration,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                Agency = agency
        };
            await _db.FriendOffers.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
       
        
        public IEnumerable<FriendOffer> GetAllAgencyOffers(string agencyid)
        {
            var AgencyOffers = _db.FriendOffers.Where(i => i.AgencyId == agencyid).ToArray();
            
            return AgencyOffers;
        }
        public FriendOffer GetActiveOffer(string agencyid)
        {

            var friendoffer = _db.FriendOffers.Where(i => i.AgencyId == agencyid && !i.IsDeleted);

            return (FriendOffer)friendoffer;
        }

        public async Task<FriendOffer> DeleteofferAsync(string Id, string userId)
        {


            var item = await _db.FriendOffers.FindAsync(Id);
            if (item == null || userId != item.UserId)
                return null;

            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;
            item.UserId = userId;
            await _db.SaveChangesAsync();

            return item;

        }



    }
}


    