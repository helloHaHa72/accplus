using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class ven01vendorEntityConfiguration : IEntityTypeConfiguration<ven01vendors>
    {
        public void Configure(EntityTypeBuilder<ven01vendors> builder)
        {
            builder
                .HasMany(e => e.pur01purchases)
                .WithOne(x => x.ven01vendors)
                .HasForeignKey(x => x.pur01ven01uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.BranchData)
                .WithMany(e => e.Vendor)
                .HasForeignKey(e => e.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
