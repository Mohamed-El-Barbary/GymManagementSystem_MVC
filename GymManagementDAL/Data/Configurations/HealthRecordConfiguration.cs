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
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {

            builder.ToTable("Members");

            builder.HasOne<Member>()
                   .WithOne(m => m.HealthRecord)
                   .HasForeignKey<HealthRecord>(hr => hr.Id);

            builder.Ignore(hr => hr.CreatedAt);

        }
    }
}
