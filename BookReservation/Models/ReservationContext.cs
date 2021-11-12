using Microsoft.EntityFrameworkCore;

namespace BookApi.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
           : base(options)
        {
        }

        public DbSet<ReservationRecord> ReservationRecords { get; set; }
    }
}
