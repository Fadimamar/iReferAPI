using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using iReferAPI.Models;
using iReferAPI.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace iReferAPI.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Reward>Rewards { get; set; }
        public DbSet<AgencyRole> AgencyRoles { get; set; }
        public DbSet<FriendOffer> FriendOffers { get; set; }
    }
}
