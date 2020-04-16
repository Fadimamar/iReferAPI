using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using iReferAPI.Models;
using iReferAPI.Server.Models;

namespace iReferAPI.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Agency> Agencies { get; set; }
    }
}
