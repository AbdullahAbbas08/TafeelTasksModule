using System;
using System.Collections.Generic;
using System.Text;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.Domains;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;

namespace DbContexts.AuditDbContext
{
    public class AuditContext : DbContext
    {
        public static string connectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<CommitteAudit> Audits { get; set; }
        //public DbSet<LookupValues> LookupValues { get; set; }
    }
}
