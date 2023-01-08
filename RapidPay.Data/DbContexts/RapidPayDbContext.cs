using Microsoft.EntityFrameworkCore;
using RapidPay.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Data.DbContexts
{
    public class RapidPayDbContext: DbContext, IRapidPayDbContext
    {
        public RapidPayDbContext(DbContextOptions<RapidPayDbContext> options) : base(options)
        {

        }

        public DbSet<Card> Cards { get; set; }

        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
