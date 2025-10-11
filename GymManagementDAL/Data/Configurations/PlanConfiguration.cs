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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(x => x.Price)
                .HasPrecision(10, 2);

            builder.Property(x => x.Description)
                   .HasColumnType("varchar")
                   .HasMaxLength(200);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("CK_PlanDurationDayes", "DurationDays BETWEEN 1 AND 265");

            });
        }
    }
}
