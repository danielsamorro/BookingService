using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure
{
    public class BookingServiceContext : DbContext
    {
        public BookingServiceContext(DbContextOptions<BookingServiceContext> options) : base( options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
