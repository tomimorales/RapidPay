using Microsoft.EntityFrameworkCore;
using RapidPay.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Data.DbContexts
{
    public interface IRapidPayDbContext
    {
        DbSet<Card> Cards { get; set; }
        Task<int> SaveChanges();
    }
}
