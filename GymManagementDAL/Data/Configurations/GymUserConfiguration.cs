using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
    
        public void Configure(EntityTypeBuilder<T> builder)
        {

            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(x => x.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.ToTable(Tb => {
                Tb.HasCheckConstraint("CK_GymUserValidEmail", "Email LIKE '_%@_%._%'");
                Tb.HasCheckConstraint("CK_GymUserValidPhone", "Phone LIKE '01%' AND Phone Not LIKE '%[^0-9]%' AND LEN(Phone) = 11");
            });

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, Address =>
            {

                Address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(50);
                Address.Property(a => a.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar")
                       .HasMaxLength(100);

            });


        }
    
    }
}
