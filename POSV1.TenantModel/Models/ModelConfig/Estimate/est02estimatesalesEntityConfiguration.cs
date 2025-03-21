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
    public class est02estimatesalesEntityConfiguration : IEntityTypeConfiguration<est02estimatesales>
    {
        public void Configure(EntityTypeBuilder<est02estimatesales> builder)
        {
            builder
                .HasMany(e => e.est02estimatesalesitems)
                .WithOne(e => e.est02estimatesales)
                .HasForeignKey(e => e.est02estimatesalesuin)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //    .HasOne(e => e.BranchData)
            //    .WithMany(b => b.EstimateSale)
            //    .HasForeignKey(e => e.BranchCode)
            //    .HasPrincipalKey(b => b.BranchCode);

            builder
                .HasOne(e => e.est01estimate) // Navigation property
                .WithMany(e => e.est02estimatesales) // Parent collection property
                .HasForeignKey(e => e.est02est01uin) // Foreign key in est02estimatesales
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
