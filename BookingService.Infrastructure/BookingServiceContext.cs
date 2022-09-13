using BookingService.Domain.Entities;
using BookingService.Infrastructure.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure
{
    public class BookingServiceContext : DbContext
    {
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        public BookingServiceContext(DbContextOptions<BookingServiceContext> options) : base( options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HotelRoomConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
