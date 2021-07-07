using JobbApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Data.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasOne(x => x.Job).WithMany(x => x.Candidates).HasForeignKey(x => x.JobId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Candidates).HasForeignKey(x => x.AppUserId);
        }
    }
}
