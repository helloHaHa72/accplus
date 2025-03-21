using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig.Return
{
    partial class pur02returnitemsEntityConfiguration : IEntityTypeConfiguration<pur02returnitems>
    {
        public void Configure(EntityTypeBuilder<pur02returnitems> builder)
        {
            builder
               .HasOne(e => e.pur01purchasereturns)
               .WithMany(e => e.pur02returnitems)
               .HasForeignKey(e => e.pur02returnpur01uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(e => e.pro02products)
               .WithMany()
               .HasForeignKey(e => e.pur02returnpro02uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(e => e.un01units)
               .WithMany()
               .HasForeignKey(e => e.pur02returnun01uin)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
