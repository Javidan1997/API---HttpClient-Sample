using JobbApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Data.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Desc).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Salary).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Experience).HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.Category).WithMany(x => x.Jobs);
            builder.HasOne(x => x.City).WithMany(x => x.Jobs);
            builder.HasOne(x => x.Country).WithMany(x => x.Jobs);
            builder.HasOne(x => x.Company).WithMany(x => x.Jobs);

        }
    }
}
