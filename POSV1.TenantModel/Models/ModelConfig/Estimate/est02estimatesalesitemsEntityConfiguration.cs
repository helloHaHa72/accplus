using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Estimate
{
    public class est02estimatesalesitemsEntityConfiguration : IEntityTypeConfiguration<est02estimatesalesitems>
    {
        public void Configure(EntityTypeBuilder<est02estimatesalesitems> builder)
        {
            builder
                .HasOne(e => e.est02estimatesales)
                .WithMany(e => e.est02estimatesalesitems)
                .HasForeignKey(e => e.est02estimatesalesuin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.pro02products)
                .WithMany()
                .HasForeignKey(e => e.est02pro02uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.un01units)
                .WithMany()
                .HasForeignKey(e => e.est02un01uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
