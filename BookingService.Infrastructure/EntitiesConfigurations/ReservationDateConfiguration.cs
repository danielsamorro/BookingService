using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntitiesConfigurations
{
    public class ReservationDateConfiguration : IEntityTypeConfiguration<ReservationDate>
    {
        public void Configure(EntityTypeBuilder<ReservationDate> builder)
        {
            builder.ToTable("ReservationDate");

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.ReservedOn)
                .IsRequired();

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Reservation)
                .WithMany(x => x.ReservationDates)
                .HasForeignKey(x => x.ReservationId);
        }
    }
}
