using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    partial class pur01purchaseEntityConfiguration : IEntityTypeConfiguration<pur01purchases>
    {
        public void Configure(EntityTypeBuilder<pur01purchases> builder)
        {
            builder
               .HasMany(e => e.pur02items)
               .WithOne(e => e.pur01purchases)
               .HasForeignKey(e => e.pur02pur01uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.BranchData)
                .WithMany(e => e.Purchases)
                .HasForeignKey(e => e.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
