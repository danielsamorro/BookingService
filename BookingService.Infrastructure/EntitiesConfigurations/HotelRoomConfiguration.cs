using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntitiesConfigurations
{
    public class HotelRoomConfiguration : IEntityTypeConfiguration<HotelRoom>
    {
        public void Configure(EntityTypeBuilder<HotelRoom> builder)
        {
            builder.ToTable("HotelRoom");

            builder.Property(x => x.Id);
            
            builder.Property(x => x.Number)
                .IsRequired();

            builder.HasKey(x => x.Id);
        }
    }
}
