
using iReferAPI.Models;
using iReferAPI.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iReferAPI.Server.Services
{
    public interface IAgencyInvitationsService
    {


        IEnumerable<AgencyInvitation> GetNotViewedInvitations(string agencyid);
        IEnumerable<AgencyInvitation> GetNotSentInvitations(string agencyid);
        IEnumerable<AgencyInvitation> GetNotSubscribedInvitations(string agencyid);
        Task<AgencyInvitation> InviteCustomer(AgencyInvitationRequest model, string userId);
        IEnumerable<AgencyInvitation> GetAllAgencyInvitations(string agencyid);
        Task<AgencyInvitation> DeleteInvitationAsync(string Id, string userId);
        Task<AgencyInvitation> MarkSentAsync(string Id, string userId);
        Task<AgencyInvitation> MarkViewedAsync(string Id, string userId);
        Task<AgencyInvitation> MarkSubscribed(string Id, string userId);
    }

    public class AgencyInvitationsService : IAgencyInvitationsService
        {

            private readonly ApplicationDbContext _db;
            private IMailService _mailService;

            public AgencyInvitationsService(ApplicationDbContext db, IMailService mailservice)
            {
                _db = db;
                _mailService = mailservice;
            }

            public async Task<AgencyInvitation> InviteCustomer(AgencyInvitationRequest model, string userId)


            {


                var agency = await _db.Agencies.FindAsync(model.AgencyId);
                if (agency == null)
                    return null;

                var item = new AgencyInvitation
                {
                    AgencyId = model.AgencyId,
                    Email = model.Email,
                    UserId = userId,
                    IsSent = false,
                    IsViewed = false,
                    CreatedDate = DateTime.UtcNow,
                    DateInvited = DateTime.UtcNow,
                    HasSubscribed = false,
                    Agency = agency,
                    IsDeleted = false,

                };
                string url = model.EnrollPageURL.Trim();
                if (url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }
                url = url + "?Id=" + item.Id;

                var result = await _mailService.SendEmailAsync(model.Email, "Welcome To " + agency.AgencyName + " Referral Program!", "<h1>Welcome To " + agency.AgencyName + " Referral Program!</h1>" +
                        $"<p> Please subscribe to our program by <a href='{url}'> clicking Here</p>");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted || result.StatusCode == System.Net.HttpStatusCode.OK)

                {
                    item.IsSent = true;
                };
                await _db.AgencyInvitations.AddAsync(item);
                await _db.SaveChangesAsync();
                return item;
            }



            public IEnumerable<AgencyInvitation> GetAllAgencyInvitations(string agencyid)
            {
                var AgencyInvitations = _db.AgencyInvitations.Where(i => i.AgencyId == agencyid).ToArray();

                return AgencyInvitations;
            }
            public IEnumerable<AgencyInvitation> GetNotSentInvitations(string agencyid)
            {

                var AgencyInvitations = _db.AgencyInvitations.Where(i => i.AgencyId == agencyid && !i.IsSent);

                return AgencyInvitations;
            }

            public IEnumerable<AgencyInvitation> GetNotViewedInvitations(string agencyid)
            {

                var AgencyInvitations = _db.AgencyInvitations.Where(i => i.AgencyId == agencyid && !i.IsViewed);

                return AgencyInvitations;
            }
            public IEnumerable<AgencyInvitation> GetNotSubscribedInvitations(string agencyid)
            {

                var AgencyInvitations = _db.AgencyInvitations.Where(i => i.AgencyId == agencyid && !i.HasSubscribed);

                return AgencyInvitations;
            }

            public async Task<AgencyInvitation> DeleteInvitationAsync(string Id, string userId)
            {


                var item = await _db.AgencyInvitations.FindAsync(Id);
                if (item == null || userId != item.UserId)
                    return null;

                item.IsDeleted = true;
                item.ModifiedDate = DateTime.UtcNow;
                item.UserId = userId;
                await _db.SaveChangesAsync();

                return item;

            }
            public async Task<AgencyInvitation> MarkSentAsync(string Id, string userId)
            {


                var item = await _db.AgencyInvitations.FindAsync(Id);
                if (item == null)
                    return null;

                item.IsSent = true;
                item.ModifiedDate = DateTime.UtcNow;
                item.UserId = userId;
                await _db.SaveChangesAsync();

                return item;

            }

            public async Task<AgencyInvitation> MarkViewedAsync(string Id, string userId)
            {


                var item = await _db.AgencyInvitations.FindAsync(Id);
                if (item == null)
                    return null;

                item.IsViewed = true;
                item.ModifiedDate = DateTime.UtcNow;
                item.UserId = userId;
                await _db.SaveChangesAsync();

                return item;

            }

            public async Task<AgencyInvitation> MarkSubscribed(string Id, string userId)
            {


                var item = await _db.AgencyInvitations.FindAsync(Id);
                if (item == null)
                    return null;

                item.HasSubscribed = true;
                item.ModifiedDate = DateTime.UtcNow;
                item.UserId = userId;
                await _db.SaveChangesAsync();

                return item;

            }



        }
   
}


    