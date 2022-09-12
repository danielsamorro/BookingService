using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Infrastructure.EntitiesConfigurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservation");

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.HotelRoom)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.HotelRoomId);
        }
    }
}
