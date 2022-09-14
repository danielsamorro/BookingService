using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntitiesConfigurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservation");

            builder.Property(x => x.Id)
                .IsRequired();

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.HotelRoom)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.HotelRoomId);

            builder.HasOne(x => x.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(x => x.UserId);
        }
    }
}
