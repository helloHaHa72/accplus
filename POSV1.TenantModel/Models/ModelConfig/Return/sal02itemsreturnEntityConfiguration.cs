using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Return
{
    partial class sal02itemsreturnEntityConfiguration : IEntityTypeConfiguration<sal02itemsreturn>
    {
        public void Configure(EntityTypeBuilder<sal02itemsreturn> builder)
        {
            // Configure relationship with sal01salesreturn
            builder
               .HasOne(e => e.sal01salesreturn)
               .WithMany(e => e.sal02itemsreturn)
               .HasForeignKey(e => e.sal02sal01uin)
               .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with pro02products
            builder
               .HasOne(e => e.pro02products)
               .WithMany()
               .HasForeignKey(e => e.sal02pro02uin)
               .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with un01units
            builder
               .HasOne(e => e.un01units)
               .WithMany()
               .HasForeignKey(e => e.sal02un01uin)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
