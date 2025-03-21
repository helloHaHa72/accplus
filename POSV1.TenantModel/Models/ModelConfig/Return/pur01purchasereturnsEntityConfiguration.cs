using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    partial class pur01purchasereturnsEntityConfiguration : IEntityTypeConfiguration<pur01purchasereturns>
    {
        public void Configure(EntityTypeBuilder<pur01purchasereturns> builder)
        {
            // Configure relationship with pur02returnitems
            builder
               .HasMany(e => e.pur02returnitems)
               .WithOne(e => e.pur01purchasereturns)
               .HasForeignKey(e => e.pur02returnpur01uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(e => e.pur01purchase)
               .WithMany(e => e.pur01purchasereturns)
               .HasForeignKey(e => e.pur01uin)
               .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with ven01vendors
            builder
               .HasOne(e => e.ven01vendors)
               .WithMany()
               .HasForeignKey(e => e.pur01ven01uin)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
