using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {


            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDay")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.MemberId, x.SessionId });

            builder.Ignore(x => x.Id);


        }
    }
}
