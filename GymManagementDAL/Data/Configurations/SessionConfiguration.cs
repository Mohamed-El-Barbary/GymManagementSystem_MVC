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
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("CK_SessionValidEndDate", "EndDate > StartDate");
                Tb.HasCheckConstraint("CK_SessionCapacity", "Capacity Between 1 AND 25");
            });

            builder.HasOne(x => x.SessionCategory)
                   .WithMany(x => x.Sessions)
                   .HasForeignKey(x => x.CategoryId);


            builder.HasOne(x => x.SessionTrainer)
                   .WithMany(x => x.TrainerSessions)
                   .HasForeignKey(x => x.TrainerId);

        }
    }
}
